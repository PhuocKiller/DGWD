using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoboController : NetworkBehaviour, ICanTakeDamage
{
    bool isDetectInput, isDetectReady;
    [SerializeField]
    float maxHealth;
    [Networked]
    float health { get; set; }
    RoboInput roboInput;
    Vector2 mousePos { get; set; }

    [Networked]
    Vector2 inputDirection { get; set; }

    [Networked]
    Vector3 headDirection { get; set; }
    Vector3 myInputVec3;
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    Transform headTransform, bodyTransform;
    [SerializeField]
    float angle;
    [SerializeField]
    MeshRenderer headMeshRenderer;
    [SerializeField]
    Material[] headMaterial;
    [Networked]
    Vector3 syncPosition { get; set; }
    Interpolator<Vector3> interpolationPosition;
    NetworkCharacterControllerPrototype characterControllerPrototype;
    [SerializeField]
    Transform bulletPoint;
    [SerializeField]
    GameObject bullet;
    bool isFire;
    float countDownFire=0.5f;
    [SerializeField]
    GameObject roboVisual;
    float lives = 3;
    bool visualChanged = false;
    TrackingReady trackingReady;

    //0 la binh thuong
    // 1 la respawn visualchange false
    //2 respawn visualchange true
    [Networked(OnChanged =nameof(ListenState))]
    int state {  get; set; }
    TickTimer respawnCount {  get; set; }
    [Networked]
    private Vector3 spawnPoint {  get; set; }
    Vector3 spawnPointlocal;
    bool flagState;
    public void SetTrackingReady(TrackingReady trackingReady)
    {
        this.trackingReady = trackingReady; 
    }
    public void SetSpawnPoint(Vector3 setSpawnPoint)
    {
        spawnPointlocal = setSpawnPoint;
    }
    public int GetCurrentState()
    {
        return state;
    }
    protected static void ListenState(Changed<RoboController> changed)
    {
        if (changed.Behaviour.state==1)
        {
            changed.Behaviour.headTransform.gameObject.SetActive(true);
            changed.Behaviour.bodyTransform.gameObject.SetActive(true);
            if (changed.Behaviour.Object.HasStateAuthority)
            {
                changed.Behaviour.characterControllerPrototype.TeleportToPosition(changed.Behaviour.spawnPointlocal);
            }
            changed.Behaviour.CheckCamera(changed.Behaviour.Object.InputAuthority, true);
            
            changed.Behaviour.StartCoroutine(changed.Behaviour.CheckFlag());
            return;
        }
        if (changed.Behaviour.state==2) 
        {
            changed.Behaviour.headTransform.gameObject.SetActive(false);
            changed.Behaviour.bodyTransform.gameObject.SetActive(false);
            changed.Behaviour.flagState = true;
            if (changed.Behaviour.Object.HasStateAuthority)
            {
                changed.Behaviour.characterControllerPrototype.TeleportToPosition(new Vector3(-500, -500, -500));

            }
            changed.Behaviour.CheckCamera(changed.Behaviour.Object.InputAuthority, false);
           
            return; 
        }
    }
    void CheckCamera(PlayerRef player, bool isFollow)
    {
        if (player == Runner.LocalPlayer)
        {
            if(isFollow)
            {
                Singleton<CameraController>.Instance.SetFollowRobo(transform);
            }
            else
            {
                Singleton<CameraController>.Instance.RemoveFollowRobo();
            }
            
        }

    }
    public IEnumerator CheckFlag()
    {
        yield return new WaitForSeconds(0.1f);
        flagState = false;

    }
    public override void Spawned()
    {
        base.Spawned();
        if (HasStateAuthority)
        {
            state = 0;
        }
        health = maxHealth;
        characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        headMeshRenderer.material= headMaterial[Object.InputAuthority.PlayerId];
        if(Object.InputAuthority.PlayerId==Runner.LocalPlayer.PlayerId)
        {
            Singleton<CameraController>.Instance.SetFollowRobo(transform);
            FindObjectOfType<GameManager>().RegisterOnGameStateChanged(OnGameStateChanged);
        }
        Singleton<PlayerManager>.Instance.AddRobo(this);
    }
    public void OnGameStateChanged(GameState oldState, GameState newState)
    {
        if (HasStateAuthority)
        {
            if (newState == GameState.Transition || newState == GameState.None)
            {
                isDetectInput = false;
            }
            else
            {
                isDetectInput = true;
            }
            isDetectReady = newState == GameState.Lobby;
            if (newState != GameState.Lobby && trackingReady!=null)
            {
                trackingReady.HideReady();
            }
        }
        
        
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        {
            
        }
        if (HasStateAuthority)
        {
            if (respawnCount.RemainingTicks(Runner) <= 1 && visualChanged)
            {
                state = 1;
                visualChanged = false;
                
            }
            if (respawnCount.IsRunning && !respawnCount.Expired(Runner))
            {
                if (visualChanged) { return; }
                state = 2;
                visualChanged = true;
                
            }
        }
        
        if(visualChanged) { return; }
        CalculateMove();
        CalculateHeadRotation();
        CalculateBodyRotation();
        
        if (isFire &&HasInputAuthority && countDownFire==0.5)
        {
            Runner.Spawn(bullet,bulletPoint.position, Quaternion.identity, inputAuthority: Object.InputAuthority,
                onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
                {
                    obj.GetComponent<Bullet>().SetDirection(headTransform.forward);
                });
            countDownFire -= Runner.DeltaTime;
        }
        isFire = false;
        if (countDownFire > 0 && countDownFire<0.5)
        {
            countDownFire -= Runner.DeltaTime;
        }
        else
        {
            countDownFire = 0.5f;
        }
      
    }
    public override void Render()
    {
        base.Render();
        if (HasStateAuthority)
        {
            health = health;
        }
    }

    private void Awake()
    {
        roboInput = new RoboInput();
    }
    #region lifeinput
    private void OnEnable()
    {
        roboInput.Enable();
    }
    private void OnDisable()
    {
        roboInput.Disable();
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }
  
    // Update is called once per frame
    void Update()
    {
        if (HasStateAuthority&&Object.IsValid)
        {
            if (!isDetectInput)
            {
                return;
            }
        }
        if (HasInputAuthority &&HasStateAuthority)
        {
            mousePos = roboInput.RoboActions.MousePosition.ReadValue<Vector2>();
            inputDirection = roboInput.RoboActions.Move.ReadValue<Vector2>();
            if (!isFire)
            {
                isFire = roboInput.RoboActions.Fire.triggered;
            }
        }
        if (HasInputAuthority && isDetectReady && Object.IsValid 
            &&roboInput.RoboActions.Ready.triggered &&trackingReady!=null)
        {
            trackingReady.OnChangeReady();
        }
    }
    private void FixedUpdate()
    {
      
    }
    void CalculateHeadRotation()
    {
        if (HasInputAuthority && HasStateAuthority)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 200, groundLayerMask))
            {
                headDirection = (new Vector3(hitInfo.point.x, 0, hitInfo.point.z) - new Vector3(headTransform.position.x, 0, headTransform.position.z)).normalized;
            }
        }

       // headTransform.forward = headDirection;
       Quaternion look= Quaternion.LookRotation(headDirection);
        headTransform.rotation = Quaternion.RotateTowards(headTransform.rotation, look, 360 * Runner.DeltaTime);
    }
    void CalculateBodyRotation()
    {
        /*Vector3 axisRotate = new Vector3(inputDirection.y, 0, -inputDirection.x);
        bodyTransform.rotation= Quaternion.AngleAxis(angle, axisRotate);*/

        Vector3 direction = inputDirection.sqrMagnitude > 0 ? new Vector3(inputDirection.y, 0, -inputDirection.x) : Vector3.zero;


            Quaternion rotate = Quaternion.Euler(direction * angle);
            bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, rotate, 360 * Time.deltaTime);

    }
    void CalculateMove()
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        if (flagState) { return; }
        characterControllerPrototype.Move(moveDirection * Runner.DeltaTime * 4);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]   
    public void CalculateHealth_Rpc(int damage, PlayerRef playerAttack)
    {
        if (health - damage > 0)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            if (lives>0)
            {
                lives -= 1;
                health = maxHealth;
                if (HasStateAuthority)
                {
                    RespawnRobo(3);
                }
            }
            else
            {
                Debug.Log("No live");
            }
        }
        Debug.Log($"Player: {playerAttack.PlayerId} Apply damage to Player: {Object.InputAuthority.PlayerId} | current health: {health}");
    }
    void RespawnRobo(int second)
    {
        respawnCount=TickTimer.CreateFromSeconds(Runner,second);
        
    }
    public void ApplyDamage(int damage, PlayerRef playerAttack, Action callback = null)
    {
        CalculateHealth_Rpc(damage, playerAttack);
        callback?.Invoke();
    }
}
