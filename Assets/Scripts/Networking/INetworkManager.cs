using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using CC;

public class INetworkManager : NetworkManager
{
    public CharacterCustomizationNetworkMessage myCharacterMessage;
    
    public CharacterOutlookSync characterOutlookSyncManager;

    public List<GameObject> playersList = new ();
    public List<CharacterCustomizationNetworkMessage> messagesList = new();

    public bool hasInit = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CharacterCustomizationNetworkMessage>(OnCreateCharecter, false);
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
        //NetworkClient.Send(localCharacterDataMessage);
        Debug.Log("Connected to Server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from Server");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("Server Scene Changed");
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        Debug.Log("Client Scene Changed");
        if(hasInit)
            NetworkClient.Send(myCharacterMessage);
    }

    void OnCreateCharecter(NetworkConnectionToClient conn, CharacterCustomizationNetworkMessage message) 
    {
        Debug.Log("Creating Character");
        var player = Instantiate(message.PrefabName is PlayerPrefabName.Female ? spawnPrefabs[1] : spawnPrefabs[0]);
        /*var playerCustom = player.GetComponent<CharacterCustomization>();
        playerCustom.CharacterName = message.CharacterName;
        playerCustom.PureInitialize();
        playerCustom.ApplyCharacterVars(message.CharacterData);*/
        NetworkServer.Spawn(player, conn);
        NetworkServer.AddPlayerForConnection(conn, player);
        for (int i = playersList.Count - 1; i > 0; i--)
        {
            if (!playersList[i])
            {
                playersList.RemoveAt(i);
                messagesList.RemoveAt(i);
            }
        }
        
        playersList.Add(player);
        messagesList.Add(message);

        var messages = new ServerCharacterOutlookMessages
        {
            Players = playersList,
            Messages = messagesList
        };
        characterOutlookSyncManager.CmdSetPlayerOutlook(messages);
    }
    
    public void InstantiateCharacterByName(CC_CharacterData characterData)
    {
        //Instantiate character from resources folder, set name and initialize the script
        var newCharacter = (GameObject)Instantiate(Resources.Load(characterData.CharacterPrefab));
        newCharacter.GetComponent<CharacterCustomization>().CharacterName = characterData.CharacterName;
        newCharacter.GetComponent<CharacterCustomization>().Initialize();
    }
}
