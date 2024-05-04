using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct ServerCharacterOutlookMessages : NetworkMessage
{
    public List<GameObject> Players;
    public List<CharacterCustomizationNetworkMessage> Messages;
}
