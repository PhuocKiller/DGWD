using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour
{
    NetworkRunner runner;
    [SerializeField]
    UnityEvent onServerStart;
    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void StartGame()
    {
       await runner.StartGame( new StartGameArgs{ GameMode = GameMode.Shared,
        SessionName="Hello Photon"
        }); //đợi sau khi khởi tạo sever mới chạy code ở dưới
        onServerStart?.Invoke();
    }
}
