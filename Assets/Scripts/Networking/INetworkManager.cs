using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using CC;

public class INetworkManager : NetworkManager
{
    public CharacterCustomizationNetworkMessage localCharacterDataMessage;
    public override void Start()
    {
        base.Start();
        NetworkServer.RegisterHandler<CharacterCustomizationNetworkMessage>(OnCreateCharecter);
    }

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
        NetworkClient.Send(localCharacterDataMessage);
        Debug.Log("Connected to Server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from Server");
    }

    void OnCreateCharecter(NetworkConnectionToClient conn, CharacterCustomizationNetworkMessage message)
    {
        var femalePrefab = spawnPrefabs[0];
        var malePrefab = spawnPrefabs[1];
        Debug.Log($"message prefab is {message.PrefabName}");
        var player = Instantiate(message.PrefabName is PlayerPrefabName.Female ? femalePrefab : malePrefab);

        var playerCustom = player.GetComponent<CharacterCustomization>();
        playerCustom.CharacterName = message.CharacterName;
        playerCustom.PureInitialize();
        playerCustom.ApplyCharacterVars(message.CharacterData);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public void InstantiateCharacterByName(CC_CharacterData characterData)
    {
        //Instantiate character from resources folder, set name and initialize the script
        var newCharacter = (GameObject)Instantiate(Resources.Load(characterData.CharacterPrefab));
        newCharacter.GetComponent<CharacterCustomization>().CharacterName = characterData.CharacterName;
        newCharacter.GetComponent<CharacterCustomization>().Initialize();
    }
}
