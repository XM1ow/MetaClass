using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Networking.Types;

[DisallowMultipleComponent]
[AddComponentMenu("Network/I Network HUD")]
public class INetworkHUD : MonoBehaviour
{
    [Header("Main Buttons")]
    public Button startButton;
    public Button clientButton;
    public Button teachingButton;
    public Button quitButton; 
    [SerializeField] private GameObject _buttonParent;
    [Header("Join Room Panel")] 
    public GameObject joinRoomPanel;
    private TMP_InputField _serverAddress;
    private Button _connectButton;
    private Button _returnButton;
    private Button _teachingReturnButton;
    [Header("Settings Panel")]
    public GameObject teachingPanel;
    void Awake()
    {
        // Main Panel
        //_buttonParent = GameObject.Find("Buttons");
        if (_buttonParent)
        {
            startButton = _buttonParent.transform.Find("Host Room").GetComponent<Button>();
            clientButton = _buttonParent.transform.Find("Join Room").GetComponent<Button>();
            teachingButton = _buttonParent.transform.Find("Teaching").GetComponent<Button>();
            quitButton = _buttonParent.transform.Find("Quit").GetComponent<Button>();
        }
        if (startButton)
        {
            startButton.onClick.AddListener(OnStartButton);
        }
        if(clientButton)
        {
            clientButton.onClick.AddListener(CallClientUI);
        }
        if(teachingButton)
        {
            teachingButton.onClick.AddListener(OnSettingButton);
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
                    NetworkManager.singleton.networkAddress = _serverAddress.text == "" ?"localhost" : _serverAddress.text;
                    //_manager.localCharacterDataMessage = MyCharactermMessage;
                    NetworkManager.singleton.StartClient();
                    Debug.Log($"Connecting to {NetworkManager.singleton.networkAddress}");
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
        if (teachingPanel)
        {
            _teachingReturnButton = teachingPanel.transform.Find("Back").GetComponent<Button>();
            if (_teachingReturnButton)
            {
                _teachingReturnButton.onClick.AddListener(() =>
                {
                    teachingPanel.SetActive(false);
                    _buttonParent.SetActive(true);
                });
            }
        }
    }

    public void OnStartButton()
    {
        if(!NetworkClient.isConnected && !NetworkServer.active)
        {
            NetworkManager.singleton.StartHost(); // client + server
            //_manager.localCharacterDataMessage = MyCharactermMessage;
            //Debug.Log($"Starting server at {_manager.networkAddress}");
        }
    }
    public void OnSettingButton()
    {
        _buttonParent.SetActive(false);
        teachingPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
    
    public void CallClientUI()
    {
        _buttonParent.SetActive(false);
        joinRoomPanel.SetActive(true);
    }
}
