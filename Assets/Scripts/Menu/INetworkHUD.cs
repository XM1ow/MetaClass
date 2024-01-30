using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[AddComponentMenu("Network/I Network HUD")]
[RequireComponent(typeof(NetworkManager))]
public class INetworkHUD : MonoBehaviour
{
    NetworkManager _manager;

    public Button startButton;
    public Button clientButton;
    public Button settingButton;
    public Button quitButton;
    
    void Awake()
    {
        _manager = GetComponent<NetworkManager>();
        if(startButton)
            startButton.onClick.AddListener(OnStartButton);
        if(clientButton)
            clientButton.onClick.AddListener(OnClientButton);
        if(settingButton)
            settingButton.onClick.AddListener(OnSettingButton);
        if(quitButton)
            quitButton.onClick.AddListener(OnQuitButton);
    }
    private void OnStartButton()
    {
        Debug.Log($"start button");
        if(!NetworkClient.isConnected && !NetworkServer.active)
        {
            _manager.StartHost(); // client + server
            Debug.Log($"Starting server at {NetworkManager.singleton.networkAddress}");
        }
    }

    private void OnClientButton()
    {
        Debug.Log($"client button");
        CallClientUI();
    }

    private void OnSettingButton()
    {
        Debug.Log($"setting button");
    }

    private void OnQuitButton()
    {
        Debug.Log($"quit button");

    }

    private void CallClientUI()
    {
        
        if(!NetworkClient.isConnected && !NetworkServer.active)
            _manager.StartClient();
    }
    
}
