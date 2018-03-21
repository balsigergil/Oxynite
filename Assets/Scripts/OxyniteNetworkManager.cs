using UnityEngine;
using UnityEngine.Networking;

public class OxyniteNetworkManager : NetworkManager {

    [SerializeField]
    OxyniteNetworkDiscovery networkDiscovery;

    public override void OnStartHost()
    {
        base.OnStartHost();
        networkDiscovery.StartBroadcast();
        Debug.Log("Server started !");
    }
}
