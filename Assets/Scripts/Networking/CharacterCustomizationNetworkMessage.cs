using System.Collections;
using System.Collections.Generic;
using CC;
using Mirror;
using UnityEngine;

public struct CharacterCustomizationNetworkMessage : NetworkMessage
{
    public CC_CharacterData CharacterData;
}
