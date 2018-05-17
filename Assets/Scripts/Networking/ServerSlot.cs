using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;

/// <summary>
/// Handles server properties and action in the main menu
/// </summary>
public class ServerSlot : MonoBehaviour {

    private string ipAddress;
    private MatchInfoSnapshot match = null;

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;
    public bool isLAN = true;

    /// <summary>
    /// Set server slot text
    /// </summary>
    /// <param name="server"></param>
    public void SetupLAN(LanEntry server)
    {
        isLAN = true;
        ipAddress = server.ipAddress;
        transform.Find("Name").GetComponent<TMP_Text>().text = "LAN: " + server.hostName;
        gameObject.tag = "LAN";
    }

    public void SetupWAN(MatchInfoSnapshot match, JoinRoomDelegate joinRoom)
    {
        isLAN = false;
        this.match = match;
        joinRoomCallback = joinRoom;
        transform.Find("Name").GetComponent<TMP_Text>().text = "WAN: " + match.name;
        gameObject.tag = "WAN";
    }

    public void JoinRoom()
    {
        if(isLAN)
            OxyniteNetworkManager.GetInstance().StartGame(ipAddress);
        else
            joinRoomCallback.Invoke(match);
    }

    public MatchInfoSnapshot GetMatch()
    {
        return match;
    }

}
