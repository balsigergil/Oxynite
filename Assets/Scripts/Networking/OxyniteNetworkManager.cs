using UnityEngine;
using UnityEngine.Networking;

public class OxyniteNetworkManager : NetworkManager
{

    private OxyniteNetworkDiscovery nd = null;

    void Start()
    {
        nd = FindObjectOfType<OxyniteNetworkDiscovery>();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        nd.StartBroadcast();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        nd.StopBroadcast();
        nd.StartListening();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if(conn.address != "localServer")
            nd.StopListening();
    }

    public void StartGame(LanEntry server)
    {
        networkAddress = server.ipAddress;
        StartClient();
    }
}
