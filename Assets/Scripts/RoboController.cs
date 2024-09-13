using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : NetworkBehaviour
{
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

    public override void Spawned()
    {
        base.Spawned();
        characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        headMeshRenderer.material= headMaterial[Object.InputAuthority.PlayerId];
        if(Object.InputAuthority.PlayerId==Runner.LocalPlayer.PlayerId)
        {
            Singleton<CameraController>.Instance.SetFollowRobo(transform);
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        CalculateMove();
        CalculateHeadRotation();
        CalculateBodyRotation();
        if (HasStateAuthority)
        {
            
        }
    }
    public override void Render()
    {
        base.Render();
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

    
}
