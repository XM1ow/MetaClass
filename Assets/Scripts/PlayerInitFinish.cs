using CC;
using Dissonance.Integrations.MirrorIgnorance;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInitFinish : MonoBehaviour
{
    private GameObject male;
    private GameObject female;

    public GameObject malePrefab;
    public GameObject femalePrefab;

    public GameObject mainMenu;
    public GameObject playerCustomization;

    private void Start()
    {
        female = transform.GetChild(1).GetChild(0).gameObject;
        male = transform.GetChild(1).GetChild(1).gameObject;
    }

    public void Finish()
    {
        
        if (male.activeSelf)
        {
            male.GetComponent<CharacterCustomization>().SaveToJSON();
            NetworkManager.singleton.playerPrefab = malePrefab;
        }
        else
        {
            female.GetComponent<CharacterCustomization>().SaveToJSON();
            NetworkManager.singleton.playerPrefab = femalePrefab;
        }
        // return to menu
        if (mainMenu && playerCustomization)
        {
            playerCustomization.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
