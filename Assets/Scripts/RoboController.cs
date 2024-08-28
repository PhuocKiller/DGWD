using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : MonoBehaviour
{
    RoboInput roboInput;
    Vector2 mousePos;
    Vector3 headDirection;
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    Transform headTransform;
    private void Awake()
    {
        roboInput=new RoboInput();
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
        mousePos= roboInput.RoboActions.MousePosition.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        CalculateHeadRotation();
    }
    void CalculateHeadRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200, groundLayerMask))
        {
            headDirection = (new Vector3(hitInfo.point.x, 0, hitInfo.point.z) - new Vector3(headTransform.position.x, 0, headTransform.position.z)).normalized;
        }
        headTransform.forward = headDirection;
    }
}
