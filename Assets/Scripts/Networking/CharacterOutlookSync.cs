using System.Collections;
using System.Collections.Generic;
using CC;
using Mirror;
using UnityEngine;

public class CharacterOutlookSync : NetworkBehaviour
{

    [Command(requiresAuthority = false)]
    public void CmdSetPlayerOutlook(ServerCharacterOutlookMessages messages)
    {
        SetCharacterOutlook(messages);
    }

    [ClientRpc]
    public void SetCharacterOutlook(ServerCharacterOutlookMessages messages)
    {
        var keys = messages.Players;
        var values = messages.Messages;
        if (keys.Count != values.Count)
        {
            Debug.LogError("Saved Character Outlook Players Count is not Equal to Messages!");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            var playerObj = keys[i];
            if (!playerObj) continue;
            var message = values[i];
            var playerCustom = playerObj.GetComponent<CharacterCustomization>();
            playerCustom.CharacterName = message.CharacterName;
            playerCustom.PureInitialize();
            playerCustom.ApplyCharacterVars(message.CharacterData);
        }
        
    }
    
    /*[Command(requiresAuthority = false)]
    public void CmdInitNetworkManager(CharacterCustomizationNetworkMessage message)
    {
        InitNetworkManager(message);
    }

    [ClientRpc]
    public void InitNetworkManager(CharacterCustomizationNetworkMessage message)
    {
        var INetworkManager = NetworkManager.singleton.GetComponent<INetworkManager>();
        if (INetworkManager)
        {
            INetworkManager.playerPrefab = message.PrefabName is PlayerPrefabName.Female ? 
                INetworkManager.spawnPrefabs[1] : INetworkManager.spawnPrefabs[0];
            //autoCreatePlayer = true;
            INetworkManager.hasInit = true;
        }
    }*/
}
