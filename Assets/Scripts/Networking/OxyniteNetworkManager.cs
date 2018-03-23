using UnityEngine.Networking;

/// <summary>
/// Handle networking actions
/// </summary>
public class OxyniteNetworkManager : NetworkManager
{
    /// <summary>
    /// Singleton
    /// </summary>
    private static OxyniteNetworkManager s_Instance = null;

    private OxyniteNetworkDiscovery nd = null;

    void Start()
    {
        s_Instance = this;
        nd = OxyniteNetworkDiscovery.GetInstance();
    }

    public static OxyniteNetworkManager GetInstance()
    {
        return s_Instance;
    }

    /// <summary>
    /// Start broadcasting when starting a host
    /// </summary>
    public override void OnStartHost()
    {
        base.OnStartHost();
        nd.StartBroadcasting();
    }

    /// <summary>
    /// Begin listening when client is disconnected
    /// </summary>
    public override void OnStopClient()
    {
        base.OnStopClient();
        nd.StartListening();
    }

    /// <summary>
    /// Stop listening only if the client isn't the server
    /// </summary>
    /// <param name="client"></param>
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        if(nd.isClient)
            nd.StopListening();
    }

    /// <summary>
    /// Connects the player to a server
    /// </summary>
    /// <param name="server"></param>
    public void StartGame(LanEntry server)
    {
        networkAddress = server.ipAddress;
        StartClient();
    }
}
