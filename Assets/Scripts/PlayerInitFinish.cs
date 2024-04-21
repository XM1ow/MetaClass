using CC;
using Dissonance.Integrations.MirrorIgnorance;
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
        
        mainCamera.SetActive(true); 
        mainCamera.transform.SetParent(null);
        enviroment.transform.SetParent(null); if (man.activeSelf)
        {
            man.transform.SetParent(null);
            man.GetComponent<CharacterCustomization>().SaveParameterJsonString();
            man.GetComponent<CharaController>().SetCamera();
            //man.AddComponent<NetworkIdentity>();
            //man.AddComponent<MirrorIgnorancePlayer>();
        }
        else
        {
            female.transform.SetParent(null);
            female.GetComponent<CharacterCustomization>().SaveParameterJsonString();
            female.GetComponent<CharaController>().SetCamera();
            //female.AddComponent<NetworkIdentity>();
            //female.AddComponent<MirrorIgnorancePlayer>();
        }
        Destroy(gameObject);
    }
}
