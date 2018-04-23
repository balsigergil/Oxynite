using TMPro;
using UnityEngine;

/// <summary>
/// Handles server properties and action in the main menu
/// </summary>
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
        transform.Find("Name").GetComponent<TMP_Text>().text = "LAN: " + server.hostName;
    }
}
