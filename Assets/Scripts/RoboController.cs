using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoboController : NetworkBehaviour, ICanTakeDamage
{
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
    float countDownFire=1f;
    [SerializeField]
    GameObject roboVisual;
    float lives = 3;
    bool visualChanged;
    [Networked]
    TickTimer respawnCount {  get; set; }
    public override void Spawned()
    {
        base.Spawned();
        health = maxHealth;
        characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        headMeshRenderer.material= headMaterial[Object.InputAuthority.PlayerId];
        if(Object.InputAuthority.PlayerId==Runner.LocalPlayer.PlayerId)
        {
            Singleton<CameraController>.Instance.SetFollowRobo(transform);
        }
        Singleton<PlayerManager>.Instance.AddRobo(this);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (respawnCount.RemainingTicks(Runner) <= 1 && visualChanged)
        {
            visualChanged = false;
            headTransform.gameObject.SetActive(true);
            bodyTransform.gameObject.SetActive(true);
        }
        if (respawnCount.IsRunning && !respawnCount.Expired(Runner))
        {
            if (visualChanged) { return; }
            visualChanged = true;
            headTransform.gameObject.SetActive(false);
            bodyTransform.gameObject.SetActive(false);
        }
        
        if(visualChanged) { return; }
        CalculateMove();
        CalculateHeadRotation();
        CalculateBodyRotation();
        
        if (isFire &&HasInputAuthority && countDownFire==1)
        {
            Runner.Spawn(bullet,bulletPoint.position, Quaternion.identity, inputAuthority: Object.InputAuthority,
                onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
                {
                    obj.GetComponent<Bullet>().SetDirection(headTransform.forward);
                });
            countDownFire -= Runner.DeltaTime;
        }
        isFire = false;
        if (countDownFire > 0 && countDownFire<1)
        {
            countDownFire -= Runner.DeltaTime;
        }
        else
        {
            countDownFire = 1f;
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
        if (HasInputAuthority &&HasStateAuthority)
        {
            mousePos = roboInput.RoboActions.MousePosition.ReadValue<Vector2>();
            inputDirection = roboInput.RoboActions.Move.ReadValue<Vector2>();
            if (!isFire)
            {
                isFire = roboInput.RoboActions.Fire.triggered;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            health += 5;
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
                roboVisual.SetActive(false);
                lives -= 1;
                health = maxHealth;

                GetComponent<CharacterController>().enabled = false;
                GetComponent<CharacterController>().radius = 0;
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
        roboVisual.SetActive(true);
        GetComponent<CharacterController>().enabled = true;
        GetComponent<CharacterController>().radius = 1;
    }
    public void ApplyDamage(int damage, PlayerRef playerAttack, Action callback = null)
    {
        CalculateHealth_Rpc(damage, playerAttack);
        callback?.Invoke();
    }
}
