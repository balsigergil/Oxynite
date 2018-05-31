/// ETML
/// Author: Gil Balsiger
/// Date: 18.04.2018
/// Summary: Handles server properties and action in the main menu

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

    /// <summary>
    /// Setup WAN server slot in the UI
    /// </summary>
    /// <param name="match"></param>
    /// <param name="joinRoom"></param>
    public void SetupWAN(MatchInfoSnapshot match, JoinRoomDelegate joinRoom)
    {
        isLAN = false;
        this.match = match;
        joinRoomCallback = joinRoom;
        transform.Find("Name").GetComponent<TMP_Text>().text = "WAN: " + match.name;
        gameObject.tag = "WAN";
    }

    /// <summary>
    /// Invoke join callback
    /// </summary>
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
