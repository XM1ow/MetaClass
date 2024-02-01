using System;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[AddComponentMenu("Network/I Network HUD")]
[RequireComponent(typeof(NetworkManager))]
public class INetworkHUD : MonoBehaviour
{
    NetworkManager _manager;
    [Header("Buttons")]
    public Button startButton;
    public Button clientButton;
    public Button settingButton;
    public Button quitButton;
    private Dictionary<Transform, Vector3> _buttonOriginalPositions = new Dictionary<Transform, Vector3>();
    [Header("Sub Panels")] 
    public GameObject joinRoomPanel;

    public GameObject settingPanel;
    void Awake()
    {
        _manager = GetComponent<NetworkManager>();
        if (startButton)
        {
            startButton.onClick.AddListener(OnStartButton);
            var t = startButton.transform;
            _buttonOriginalPositions.Add(t, t.position);
        }
        if(clientButton)
        {
            clientButton.onClick.AddListener(OnClientButton);
            var t = clientButton.transform;
            _buttonOriginalPositions.Add(t, t.position);
        }
        if(settingButton)
        {
            settingButton.onClick.AddListener(OnSettingButton);
            var t = settingButton.transform;
            _buttonOriginalPositions.Add(t, t.position);
        }
        if(quitButton)
        {
            quitButton.onClick.AddListener(OnQuitButton);
            var t = quitButton.transform;
            _buttonOriginalPositions.Add(t, t.position);
        }
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
        foreach (var button in _buttonOriginalPositions.Keys)
        {
            button.DOMove(_buttonOriginalPositions[button] + Vector3.left * 200f, 3f);
        }
        joinRoomPanel.SetActive(true);
        /*if(!NetworkClient.isConnected && !NetworkServer.active)
            _manager.StartClient();*/
    }

    public void StartClientWithIP()
    {
        
    }
}
