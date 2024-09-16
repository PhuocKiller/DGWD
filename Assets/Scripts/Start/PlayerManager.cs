using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerManager : NetworkBehaviour
{
    List<RoboController> roboControllers = new List<RoboController>();
    [SerializeField]
    bool isEndGame;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (!isEndGame)
        {
            if (roboControllers.Count == 1 &&SceneManager.GetActiveScene().name!= "Start")
            {
                isEndGame = true;
                int a = roboControllers[0].Object.InputAuthority.PlayerId;
                Debug.Log("Robo win: " + a);
                Singleton<WinPanel>.Instance.SetVictoryText(a);


            }
        }
        
    }

    public void AddRobo(RoboController roboController)
    {
        this.roboControllers.Add(roboController);
    }
    public void RemoveRobo(RoboController roboController)
    {
        this.roboControllers.Remove(roboController);
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
        if (currentReady==roboControllers.Count && currentReady>1)
        {
            Debug.Log("All Robo Ready");
            Runner.SessionInfo.IsOpen = false;
            Runner.SessionInfo.IsVisible = false;
            Singleton<ManagerScene>.Instance.PlayGameScene();
            foreach (var ready in FindObjectsOfType<TrackingReady>())
            {
                ready.gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
            }

            // Singleton<Loading>.Instance.ShowLoading();
        }
        else
        {
            Debug.Log($" {currentReady} / {roboControllers.Count}");
        }
    }
}
