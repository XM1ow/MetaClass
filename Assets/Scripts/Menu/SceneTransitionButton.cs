using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionButton : MonoBehaviour
{
    public string sceneName;
    public Button button;
    public TMP_Text tmp;

    private void Awake()
    {
        button = GetComponent<Button>();
        tmp = GetComponentInChildren<TMP_Text>();
        if (button && sceneName != "")
        {
            tmp.text = sceneName;
            button.onClick.AddListener(() =>
            {
                NetworkServer.isLoadingScene = true;
                NetworkManager.singleton.ServerChangeScene(sceneName);
                /*if (NetworkServer.active)
                {
                    NetworkServer.SendToAll(new SceneMessage{sceneName = sceneName});
                }*/
            });
        }
    }
}
