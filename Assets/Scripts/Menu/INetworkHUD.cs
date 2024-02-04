using System;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
[AddComponentMenu("Network/I Network HUD")]
[RequireComponent(typeof(NetworkManager))]
public class INetworkHUD : MonoBehaviour
{
    NetworkManager _manager;
    [Header("Main Buttons")]
    public Button startButton;
    public Button clientButton;
    public Button settingButton;
    public Button quitButton;
    private GameObject _buttonParent;
    [Header("Join Room Panel")] 
    public GameObject joinRoomPanel;

    private TMP_InputField _serverAddress;
    private Button _connectButton;
    private Button _returnButton;
    [Header("Settings Panel")]
    public GameObject settingPanel;
    void Awake()
    {
        _manager = GetComponent<NetworkManager>();
        // Main Panel
        _buttonParent = GameObject.Find("Buttons");
        if (_buttonParent)
        {
            startButton = _buttonParent.transform.Find("Host Room").GetComponent<Button>();
            clientButton = _buttonParent.transform.Find("Join Room").GetComponent<Button>();
            settingButton = _buttonParent.transform.Find("Settings").GetComponent<Button>();
            quitButton = _buttonParent.transform.Find("Quit").GetComponent<Button>();
        }
        if (startButton)
        {
            startButton.onClick.AddListener(() =>
            {
                if(!NetworkClient.isConnected && !NetworkServer.active)
                {
                    _manager.StartHost(); // client + server
                    Debug.Log($"Starting server at {NetworkManager.singleton.networkAddress}");
                }
            });
        }
        if(clientButton)
        {
            clientButton.onClick.AddListener(CallClientUI);
        }
        if(settingButton)
        {
            settingButton.onClick.AddListener(OnSettingButton);
        }
        if(quitButton)
        {
            quitButton.onClick.AddListener(OnQuitButton);
        }
        // Join Room Panel
        if (joinRoomPanel)
        {
            _serverAddress = joinRoomPanel.transform.Find("Server Address").GetComponentInChildren<TMP_InputField>();
            _connectButton = joinRoomPanel.transform.Find("Connect").GetComponent<Button>();
            _returnButton = joinRoomPanel.transform.Find("Back").GetComponent<Button>();
            if (_connectButton)
            {
                _connectButton.onClick.AddListener(() =>
                {
                    _manager.networkAddress = _serverAddress.text == "" ?"localhost" : _serverAddress.text;
                    _manager.StartClient();
                    Debug.Log($"Connecting to {_manager.networkAddress}");
                    joinRoomPanel.SetActive(false);
                });
            }
            if (_returnButton)
            {
                _returnButton.onClick.AddListener(() =>
                {
                    joinRoomPanel.SetActive(false);
                    _buttonParent.SetActive(true);
                });
            }
        }
    }
    private void OnSettingButton()
    {
    }

    private void OnQuitButton()
    {
    }
    
    private void CallClientUI()
    {
        _buttonParent.SetActive(false);
        joinRoomPanel.SetActive(true);
    }
    

}
