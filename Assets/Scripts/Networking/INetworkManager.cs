using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using CC;

public class INetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server Stopped");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        var savePath = Application.dataPath + "/CharacterCustomizer.json";
        
        Debug.Log("Connected to Server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from Server");
    }

    void OnCreateCharecter()
    {
        
    }
    
    public void InstantiateCharacterByName(string savePath, string characterName, Transform parentTransform)
    {
        if (File.Exists(savePath))
        {
            //Load CC_SaveData from JSON file
            string jsonLoad = File.ReadAllText(savePath);
            var ccSaveData = JsonUtility.FromJson<CC_SaveData>(jsonLoad);

            //Find character index by CharacterName and load character data
            int index = ccSaveData.SavedCharacters.FindIndex(t => t.CharacterName == characterName);
            if (index != -1)
            {
                //Instantiate character from resources folder, set name and initialize the script
                var newCharacter = (GameObject)Instantiate(Resources.Load(ccSaveData.SavedCharacters[index].CharacterPrefab), parentTransform);
                newCharacter.GetComponent<CharacterCustomization>().CharacterName = characterName;
                newCharacter.GetComponent<CharacterCustomization>().Initialize();
            }
        }
    }
    
    public void InstantiateCharacterByName(CC_CharacterData characterData, Transform parentTransform)
    {
        //Instantiate character from resources folder, set name and initialize the script
        var newCharacter = (GameObject)Instantiate(Resources.Load(characterData.CharacterPrefab), parentTransform);
        newCharacter.GetComponent<CharacterCustomization>().CharacterName = characterData.CharacterName;
        newCharacter.GetComponent<CharacterCustomization>().Initialize();
    }
}
