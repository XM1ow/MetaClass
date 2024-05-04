using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    private PlayerInput _input;
    public GameObject escPanel;

    private void Awake()
    {
        if (_input == null)
        {
            _input = new PlayerInput();
        }

        if (!escPanel)
        {
            escPanel = transform.Find("EscPanel").gameObject;
        }
        _input.Global.Enable();
        _input.Global.Menu.performed += _ =>
        {
            if (escPanel)
            {
                escPanel.SetActive(true);
            }
        };
    }
}
