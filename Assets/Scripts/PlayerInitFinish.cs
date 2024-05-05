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

    public INetworkManager networkManager;

    private void Start()
    {
        female = transform.GetChild(1).GetChild(0).gameObject;
        male = transform.GetChild(1).GetChild(1).gameObject;
        networkManager = NetworkManager.singleton.transform.GetComponent<INetworkManager>();
    }

    public void Finish()
    {
        if (male.activeSelf)
        {
            var maleCC = male.GetComponent<CharacterCustomization>();
            maleCC.SaveToJSON();
            
            var message = new CharacterCustomizationNetworkMessage
            {
                PrefabName = PlayerPrefabName.Male,
                CharacterName = maleCC.CharacterName,
                CharacterData = maleCC.StoredCharacterData
            };
            // spawning character
            NetworkClient.Send(message);
            networkManager.myCharacterMessage = message;
            networkManager.hasInit = true;
        }
        else
        {
            var femaleCC = female.GetComponent<CharacterCustomization>();
            femaleCC.SaveToJSON();
            
            var message = new CharacterCustomizationNetworkMessage
            {
                PrefabName = PlayerPrefabName.Female,
                CharacterName = femaleCC.CharacterName,
                CharacterData = femaleCC.StoredCharacterData
            };
            // spawning character
            NetworkClient.Send(message);
            networkManager.myCharacterMessage = message;
            networkManager.hasInit = true;
        }
        
        // return to menu
        if (mainMenu && playerCustomization)
        {
            playerCustomization.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
