using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameNetworkRunner : MonoBehaviour
{
    [SerializeField]
    GameObject roomItem;
    [SerializeField]
    Transform parentRoomItem;
    NetworkRunner runner;
    GameNetworkCallBack gameNetworkCallBack;
    [SerializeField]
    Button createRoomBtn;
    [SerializeField]
    TMP_InputField fieldRoomName;
    string roomName="";
    [SerializeField]
    UnityEvent onConnected;
    [SerializeField]
    GameObject roboPlayer;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    GameObject readyText;
    [SerializeField]
    GameObject playerManagerObject, winPanel;

    void SpawnPlayer(NetworkRunner m_runner, PlayerRef player)
    {
        if(player == runner.LocalPlayer && runner.IsSharedModeMasterClient)
        {
            runner.Spawn(playerManagerObject, inputAuthority: player);
            NetworkObject winPanelClone= runner.Spawn(winPanel, inputAuthority: player);
           // winPanelClone.transform.parent = this.transform;
        }
        if(player==runner.LocalPlayer &&spawnPoints.Length >player.PlayerId)
        {
           NetworkObject robo= m_runner.Spawn(roboPlayer, spawnPoints[player.PlayerId].position, Quaternion.identity, inputAuthority: player,
               onBeforeSpawned: OnBeforeSpawned) ;
            void OnBeforeSpawned(NetworkRunner runner, NetworkObject roboObject)
            {
                NetworkObject textR = runner.Spawn(readyText, roboObject.transform.position, inputAuthority: player);
                TrackingReady trackingReady = textR.GetComponent<TrackingReady>();
                trackingReady.SetRoboTrans(roboObject.transform);
                
            }
        }
        else
        {
            Debug.Log("No spawn" + player.PlayerId);
        }
        
    }
    
   // UnityAction onClickCreateBtn;
    private void Awake()
    {
        // onClickCreateBtn = OnClickBtn;
        runner??=GetComponent<NetworkRunner>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
       // createRoomBtn.onClick.AddListener(onClickCreateBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void OnClickBtn(Button btn)
    {
        if (roomName.Length > 0 &&runner !=null)
        {
            btn.interactable = false;
            Singleton<Loading>.Instance.ShowLoading();
            gameNetworkCallBack??=GetComponent<GameNetworkCallBack>();
            gameNetworkCallBack.onPlayerJoin=SpawnPlayer;
           await runner.StartGame(new StartGameArgs
            {
                GameMode=GameMode.Shared,
                SessionName=roomName,
                CustomLobbyName="VN"
            });
            onConnected?.Invoke();
            Singleton<Loading>.Instance.HideLoading();
        }
    }
    public async void OnClickJoinBtn()
    {
        Singleton<Loading>.Instance.ShowLoading();
        gameNetworkCallBack ??= GetComponent<GameNetworkCallBack>();
        gameNetworkCallBack.StartGameRegister(OnSessionListChanged);
        await runner.JoinSessionLobby(SessionLobby.Custom, "VN");
        Singleton<Loading>.Instance.HideLoading();
    }
    void OnSessionListChanged(List<SessionInfo> sessionInfo)
    {
        foreach(Transform child in parentRoomItem)
        {
            Destroy(child.gameObject);
        }
        foreach( var item in sessionInfo )
        {
           GameObject room= Instantiate(roomItem, parentRoomItem);
            room.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
            Button btn= room.GetComponent<Button>();
            btn.onClick.AddListener(() => 
            { roomName = item.Name;
                OnClickBtn(btn); });
        }
    }
    public void TextChanged(string text)
    {
        roomName= text.Trim();
    }
}
