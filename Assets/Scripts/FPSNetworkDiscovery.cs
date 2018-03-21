using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPSNetworkDiscovery : NetworkDiscovery {

    private List<string> serversIp = new List<string>();

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
        StartAsServer();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        serversIp.Add(fromAddress);
    }

    private IEnumerator CleanUpEntries()
    {
        MenuManager mm = FindObjectOfType<MenuManager>();
        while (true)
        {
            mm.CleanServers();
            if (serversIp.Count > 0)
            {
                foreach(string server in serversIp)
                {
                    mm.AddServer(server);
                }
            }
            yield return new WaitForSeconds(2);
        }
    }

}
