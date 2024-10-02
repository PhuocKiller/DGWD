using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackingReady : NetworkBehaviour
{
    [Networked]
    Vector3 roboPosition { get; set; }

    Transform roboTrans { get; set; }

    [Networked(OnChanged =nameof(OnTotalPlayerChanged))]
    int totalPlayer { get; set; }
    [Networked(OnChanged = nameof(ChangedVisualReady))]
    bool isReady { get; set; }
    TextMeshProUGUI text;
    public bool GetReady()
    {
        return isReady;
    }
    private void Awake()
    {
        text=GetComponent<TextMeshProUGUI>();
    }
    public void HideReady()
    {
        if(HasStateAuthority &&Object.IsValid)
        {
            this.isReady = false;
            text.enabled = false;
        }
        
    }
    public void OnChangeReady()
    {
        if (HasStateAuthority)
        {
            bool newReady = !isReady;
            this.isReady = newReady;
            text.enabled = newReady;
        }
        
    }
    protected static void OnTotalPlayerChanged(Changed<TrackingReady> changed)
    {
        changed.Behaviour.StartDetect();
    }
    protected static void ChangedVisualReady(Changed<TrackingReady> changed)
    {
        changed.Behaviour.text.enabled= changed.Behaviour.isReady;
        Singleton<ReadyManager>.Instance.GetCountReady(out int countCurrentReady);
        Debug.Log("count Current: " + countCurrentReady);
        PlayerManager pm= FindObjectOfType<PlayerManager>();
        if(pm)
        {
            pm.OnAllRoboReady(countCurrentReady);
        }

    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (HasStateAuthority && roboTrans != null)
        {
            roboPosition = roboTrans.position;
        }
    }
    public void SetRoboTrans(Transform roboTrans)
    {
        this.roboTrans = roboTrans;
    }

    public override void Render()
    {
        base.Render();
    }

    public override void Spawned()
    {
        base.Spawned();
        transform.parent = Singleton<ReadyManager>.Instance.transform;
        totalPlayer += 1;
        if(HasStateAuthority)
        {
            isReady = false;
        }
        Singleton<ReadyManager>.Instance.AddTrackingReady(this);
    }
    IEnumerator DetectRobo()
    {
        yield return new WaitForSeconds(0.3f);
        if (roboTrans == null)
        {
            RoboController[] allRobos = FindObjectsOfType<RoboController>();
            foreach (var robo in allRobos)
            {
                if (robo.Object.InputAuthority.PlayerId == Runner.LocalPlayer.PlayerId)
                {
                    SetRoboTrans(robo.transform);
                }
            }
        }
    }
    public void StartDetect()
    {
        StartCoroutine(DetectRobo());
    }
    private void Update()
    {
       
    }

    private void LateUpdate()
    {
        if (this.roboTrans != null)
        {
            transform.position = roboPosition + new Vector3(0,3,0);
        }
    }
}
