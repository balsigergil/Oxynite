using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OxyniteNetworkDiscovery : NetworkDiscovery {

    private List<LanEntry> serversIp = new List<LanEntry>();

    /// <summary>
    /// Refresh timeout
    /// </summary>
    private const int TIMEOUT = 1;

    // Use this for initialization
    void Awake () {
        Initialize();
        StartAsClient();
        StartCoroutine(CleanUpEntries());
	}

    public void StartBroadcast()
    {
        StopBroadcast();
        Initialize();
        broadcastData = SystemInfo.deviceName;
        StartAsServer();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        LanEntry le = new LanEntry(fromAddress, data);
        if(!serversIp.Contains(le))
            serversIp.Add(le);
    }

    private IEnumerator CleanUpEntries()
    {
        MenuManager mm = FindObjectOfType<MenuManager>();
        while (true)
        {
            mm.CleanServers();
            if (serversIp.Count > 0)
            {
                foreach(LanEntry server in serversIp)
                {
                    mm.AddServer(server);
                }
            }
            yield return new WaitForSeconds(TIMEOUT);
        }
    }

}
