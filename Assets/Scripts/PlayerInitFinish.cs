using CC;
using Dissonance.Integrations.MirrorIgnorance;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInitFinish : MonoBehaviour
{
    private GameObject man;
    private GameObject female;

    private void Start()
    {
        female = transform.GetChild(1).GetChild(0).gameObject;
        man = transform.GetChild(1).GetChild(1).gameObject;
    }

    public void Finish()
    {
        
        if (man.activeSelf)
        {
            man.GetComponent<CharacterCustomization>().SaveParameterJsonString();
        }
        else
        {
            female.GetComponent<CharacterCustomization>().SaveParameterJsonString();
        }
        Destroy(gameObject);
        SceneManager.LoadScene("Scenes/Menu");
    }
}
