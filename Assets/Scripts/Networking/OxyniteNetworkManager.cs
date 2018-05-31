/// ETML
/// Author: Gil Balsiger
/// Date: 18.04.2018
/// Summary: Handle networking actions

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

    /// <summary>
    /// NetworkDiscovery singleton
    /// </summary>
    private OxyniteNetworkDiscovery networkDiscovery = null;

    void Start()
    {
        s_Instance = this;
        networkDiscovery = OxyniteNetworkDiscovery.GetInstance();
    }

    public static OxyniteNetworkManager GetInstance()
    {
        return s_Instance;
    }

    /// <summary>
    /// Begin listening when client is disconnected
    /// </summary>
    public override void OnStopClient()
    {
        base.OnStopClient();
        networkDiscovery.StartListening();
    }

    /// <summary>
    /// Stop listening only if the client isn't the server
    /// </summary>
    /// <param name="client"></param>
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        if (networkDiscovery.isClient)
            networkDiscovery.StopListening();
    }

    /// <summary>
    /// Connects the player to a server
    /// </summary>
    /// <param name="server"></param>
    public void StartGame(string ipAddress)
    {
        networkAddress = ipAddress;
        StartClient();
    }
}
