using System.Collections;
using System.Collections.Generic;
using CC;
using Mirror;
using UnityEngine;

public struct CharacterCustomizationNetworkMessage : NetworkMessage
{
    public PlayerPrefabName PrefabName;
    public string CharacterName;
    public CC_CharacterData CharacterData;
}

public enum PlayerPrefabName
{
    Female,
    Male,
}
