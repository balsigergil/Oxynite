using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OxyniteNetworkDiscovery : NetworkDiscovery
{

    private List<LanEntry> serverEntries = new List<LanEntry>();

    /// <summary>
    /// Refresh timeout
    /// </summary>
    private const int TIMEOUT = 1;

    // Use this for initialization
    void Start()
    {
        Initialize();
        StartAsClient();
        StartCoroutine(CleanUpEntries());
    }

    public void StartBroadcast()
    {
        StopAllCoroutines();
        StopBroadcast();
        Initialize();
        broadcastData = SystemInfo.deviceName;
        StartAsServer();
    }

    public void StartListening()
    {
        StartCoroutine(CleanUpEntries());
        Initialize();
        StartAsClient();
    }

    public void StopListening()
    {
        StopAllCoroutines();
        StopBroadcast();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        LanEntry le = new LanEntry(fromAddress, data);

        bool exist = false;
        foreach (LanEntry entry in serverEntries)
        {
            if (entry.hostName == le.hostName)
                exist = true;
        }
        if (!exist)
            serverEntries.Add(le);
    }

    private IEnumerator CleanUpEntries()
    {
        while (true)
        {
            MenuManager mm = FindObjectOfType<MenuManager>();
            if (mm != null)
            {
                Debug.Log("Updating entries...");
                mm.CleanServers();
                if (serverEntries.Count > 0)
                {
                    int i = 0;
                    foreach (LanEntry server in serverEntries)
                    {
                        i++;
                        mm.AddServer(server);
                        Debug.Log(i + ": " + server.hostName);
                    }
                }
            }
            yield return new WaitForSeconds(TIMEOUT);
        }
    }

    public List<LanEntry> GetLanEntries()
    {
        return serverEntries.Count > 0 ? serverEntries : null;
    }

    public void CleanEntries()
    {
        serverEntries.Clear();
    }


}
