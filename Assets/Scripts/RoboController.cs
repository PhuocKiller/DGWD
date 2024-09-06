using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : NetworkBehaviour
{
    RoboInput roboInput;
    Vector2 mousePos, inputDirection;
    Vector3 headDirection, myInputVec3;
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    Transform headTransform, bodyTransform;
    [SerializeField]
    float angle;
    CharacterController characterController;

    public override void Spawned()
    {
        base.Spawned();
        if (Object.InputAuthority.PlayerId == 0)
        {
            a = 0;
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(Object.InputAuthority.PlayerId == 0 )
        {
            Debug.Log(a);
        }
    }
    public override void Render()
    {
        base.Render();
    }

    private void Awake()
    {
        roboInput = new RoboInput();
        characterController = GetComponent<CharacterController>();
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
    [Networked]
    int a  { get; set; }
    // Update is called once per frame
    void Update()
    {
        mousePos= roboInput.RoboActions.MousePosition.ReadValue<Vector2>();
        inputDirection = roboInput.RoboActions.Move.ReadValue<Vector2>();
        if (Object.InputAuthority.PlayerId == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                a = a + 1;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                a = a + 10;
            }
        }
    }
    private void FixedUpdate()
    {
        CalculateHeadRotation();
        CalculateBodyRotation();
        CalculateMove();
    }
    void CalculateHeadRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200, groundLayerMask))
        {
            headDirection = (new Vector3(hitInfo.point.x, 0, hitInfo.point.z) - new Vector3(headTransform.position.x, 0, headTransform.position.z)).normalized;
        }

       // headTransform.forward = headDirection;
       Quaternion look= Quaternion.LookRotation(headDirection);
        headTransform.rotation = Quaternion.RotateTowards(headTransform.rotation, look, 360 * Time.fixedDeltaTime);
    }
    void CalculateBodyRotation()
    {
        /*Vector3 axisRotate = new Vector3(inputDirection.y, 0, -inputDirection.x);
        bodyTransform.rotation= Quaternion.AngleAxis(angle, axisRotate);*/

        Vector3 direction= new Vector3(inputDirection.y, 0, -inputDirection.x);
        Quaternion rotate = Quaternion.Euler(direction * angle);
        bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, rotate, 360 * Time.deltaTime);



    }
    void CalculateMove()
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        characterController.Move(moveDirection*4*Time.fixedDeltaTime);
    }

    
}
