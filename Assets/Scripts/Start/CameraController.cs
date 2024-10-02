using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   
    CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera=GetComponent<CinemachineVirtualCamera>();
        Singleton<CinemachineBrain>.Instance.m_ShowCameraFrustum = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetFollowRobo(Transform roboTransform)
    {
        virtualCamera.Follow = roboTransform;
    }
    public void RemoveFollowRobo ()
    {
        virtualCamera.Follow=null;
    }
}
