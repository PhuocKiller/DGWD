using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    List<RoboController> roboControllers = new List<RoboController>();
    public void AddRobo(RoboController roboController)
    {
        this.roboControllers.Add(roboController);
    }
    public void GetCountRobo( out int count)
    {
        count= roboControllers.Count;
    }
    public RoboController GetRobo (int id)
    {
        for (int i = 0;i<roboControllers.Count; i++)
        {
            if (roboControllers[i].Object.InputAuthority.PlayerId == id)
            {
                return roboControllers[i];
            }

        }
        return null;
    }
    public void OnAllRoboReady(int currentReady)
    {
        if (currentReady==roboControllers.Count)
        {
            Debug.Log("All Robo Ready");
            Runner.SessionInfo.IsOpen = false;
            Runner.SessionInfo.IsVisible = false;
            //  Singleton<Loading>.Instance.ShowLoading();
            if (Runner.IsSharedModeMasterClient)
            {
                Runner.SetActiveScene(1);
            }

            //SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log($" {currentReady} / {roboControllers.Count}");
        }
    }
}
