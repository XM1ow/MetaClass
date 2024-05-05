using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCPanel : MonoBehaviour
{
    [Header("Main Buttons")]
    public Button returnButton;
    public Button returnToMenuButton;
    public Button sceneSelectionButton;
    public Button settingButton;
    public GameObject _buttonParent;
    [Header("Scene Selection")] 
    public GameObject sceneSelectionSubPanel;
    public Button returnToEscButton;
    public GameObject sceneButtons;
    private void Awake()
    {
        _buttonParent = GameObject.Find("Buttons");
        if (_buttonParent)
        {
            returnButton = _buttonParent.transform.Find("Return").GetComponent<Button>();
            returnToMenuButton = _buttonParent.transform.Find("Return To Menu").GetComponent<Button>();
            sceneSelectionButton = _buttonParent.transform.Find("Change Scene").GetComponent<Button>();
            settingButton = _buttonParent.transform.Find("Settings").GetComponent<Button>();
        }

        if (returnButton)
        {
            returnButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }
        if (returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(()=>
            {
                Destroy(NetworkManager.singleton);
                SceneManager.LoadScene("Scenes/Menu");
            });
        }

        if (sceneSelectionButton)
        {
            sceneSelectionButton.onClick.AddListener(() =>
            {
                if (sceneSelectionSubPanel)
                {
                    sceneSelectionSubPanel.SetActive(true);
                    _buttonParent.SetActive(false);
                }
            });
        }
        
        // scene selection panel
        if (sceneSelectionSubPanel)
        {
            returnToEscButton = sceneSelectionSubPanel.transform.Find("ReturnToEsc").GetComponent<Button>();
            sceneButtons = sceneSelectionSubPanel.transform.Find("SceneButtons").gameObject;
            if (returnToEscButton)
            {
                returnToEscButton.onClick.AddListener(() =>
                {
                    sceneSelectionSubPanel.SetActive(false);
                    _buttonParent.SetActive(true);
                });
            }
            
        }
    }

    
}
