using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
   
    CinemachineVirtualCamera virtualCamera;
    int roboFollowIndex;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera=GetComponent<CinemachineVirtualCamera>();
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().name == "Start")
        {
            virtualCamera.Follow = GameObject.Find("Plane").transform;
        };
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetFollowRobo()
    {
        virtualCamera.Follow = Singleton<PlayerManager>.Instance.roboControllers[roboFollowIndex].transform;
    }
    public void SetFollowRoboIndex(int roboIndex)
    {
        roboFollowIndex=roboIndex;
    }
    
}
