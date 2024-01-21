using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Network/I Network HUD")]
[RequireComponent(typeof(NetworkManager))]
public class INetworkHUD : MonoBehaviour
{
    NetworkManager _manager;
    
    void Awake()
    {
        _manager = GetComponent<NetworkManager>();
    }

    
}
