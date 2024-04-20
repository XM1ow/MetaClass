using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitFinish : MonoBehaviour
{
    private GameObject man;
    private GameObject female;
    private GameObject mainCamera;
    private GameObject enviroment;

    private void Start()
    {
        mainCamera = transform.GetChild(0).GetChild(0).gameObject;
        female = transform.GetChild(1).GetChild(0).gameObject;
        man = transform.GetChild(1).GetChild(1).gameObject;
        enviroment = transform.GetChild(2).gameObject;
    }

    public void Finish()
    {
        if (man.activeSelf) man.transform.SetParent(null);
        else female.transform.SetParent(null);
        mainCamera.SetActive(true); 
        mainCamera.transform.SetParent(null);
        enviroment.transform.SetParent(null);
        Destroy(gameObject);
    }
}
