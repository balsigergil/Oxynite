using UnityEngine;
using UnityEngine.UI;

public class ServerSlot : MonoBehaviour {

    private string ipAddress;

    /// <summary>
    /// Handles the "join" button action on the server slot
    /// </summary>
	public void ConnectToServer()
    {
        OxyniteNetworkManager.GetInstance().StartGame(new LanEntry(ipAddress, null));
    }

    /// <summary>
    /// Set server slot text
    /// </summary>
    /// <param name="server"></param>
    public void Initialize(LanEntry server)
    {
        ipAddress = server.ipAddress;
        transform.Find("Name").GetComponent<Text>().text = "LAN: " + server.hostName;
    }
}
