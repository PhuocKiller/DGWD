using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReadyPanel : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI secondsRemainText;
    TickTimer secondsRemain;
    bool isAllRoboReady;
    
    UnityAction onPlayGame;
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
       
            secondsRemainText.text = ((int)secondsRemain.RemainingTime(Runner)).ToString();
            if (secondsRemain.ExpiredOrNotRunning(Runner) && isAllRoboReady)
            {
                Singleton<ManagerScene>.Instance.PlayGameScene();
                foreach (var ready in FindObjectsOfType<TrackingReady>())
                {
                    ready.gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
                }
                SetActiveReadyPanel(false);
                onPlayGame=Singleton<CameraController>.Instance.SetFollowRobo;
                onPlayGame?.Invoke();
                foreach (var robo in Singleton<PlayerManager>.Instance.roboControllers)
                {
                    robo.DelayRelocate();
                }
            }
       
    }

    public override void Spawned()
    {
        base.Spawned();
        secondsRemain = TickTimer.CreateFromSeconds(Runner, 3.9f);
        secondsRemainText =GetComponentInChildren<TextMeshProUGUI>();
        transform.parent = Singleton<GameNetworkRunner>.Instance.transform;
        GetComponent<Image>().enabled = false;
        secondsRemainText.enabled = false;
    }
    public void SetActiveReadyPanel(bool isActive)
    {
        GetComponent<Image>().enabled=isActive;
        secondsRemainText.enabled = isActive;
        isAllRoboReady=isActive;
        secondsRemain = TickTimer.CreateFromSeconds(Runner, 3.9f);
    }
    public void SetAllRoboReady(bool isAllReady)
    {
        isAllRoboReady=isAllReady;
    }

}
