using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles the LAN server discovery
/// </summary>
public class OxyniteNetworkDiscovery : NetworkDiscovery
{
    /// <summary>
    /// Singleton
    /// </summary>
    private static OxyniteNetworkDiscovery s_Instance = null;

    /// <summary>
    /// List of online lan servers
    /// </summary>
    private List<LanEntry> serverEntries = new List<LanEntry>();

    /// <summary>
    /// Refresh timeout
    /// </summary>
    private const int TIMEOUT = 1;

    private bool cleanUpLoopRunning = false;

    /// <summary>
    /// Singleton assignation
    /// </summary>
    private void Awake()
    {
        s_Instance = this;
    }

    public static OxyniteNetworkDiscovery GetInstance()
    {
        return s_Instance;
    }

    /// <summary>
    /// Start discovery
    /// </summary>
    void Start()
    {
        s_Instance = this;
        Initialize();
        StartAsClient();
        StartCoroutine(CleanUpEntries());
    }

    /// <summary>
    /// Start broadcasting as server
    /// </summary>
    public void StartBroadcasting()
    {
        StopAllCoroutines();
        cleanUpLoopRunning = false;
        if (running)
            StopBroadcast();
        Initialize();
        broadcastData = SystemInfo.deviceName;
        StartAsServer();
    }

    /// <summary>
    /// Start listening as client
    /// </summary>
    public void StartListening()
    {
        if (!cleanUpLoopRunning)
            StartCoroutine(CleanUpEntries());

        if (!isClient)
        {
            if (running)
                StopBroadcast();

            Initialize();
            StartAsClient();
        }
    }

    /// <summary>
    /// Stop broadcasting
    /// </summary>
    public void StopListening()
    {
        StopAllCoroutines();
        cleanUpLoopRunning = false;
        if (running)
            StopBroadcast();
    }

    /// <summary>
    /// Add broadcaster to the list if it do not already exist
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="data"></param>
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

    /// <summary>
    /// Infinite loop to check for new entries and add them to the UI
    /// </summary>
    /// <returns></returns>
    private IEnumerator CleanUpEntries()
    {
        cleanUpLoopRunning = true;
        while (true)
        {
            MenuManager mm = FindObjectOfType<MenuManager>();
            if (mm != null)
            {
                mm.CleanServersList();
                if (serverEntries.Count > 0)
                {
                    int i = 0;
                    foreach (LanEntry server in serverEntries)
                    {
                        i++;
                        mm.AddServerSlot(server);
                    }
                }
            }
            yield return new WaitForSeconds(TIMEOUT);
        }
    }

    /// <summary>
    /// Delete all lan entries
    /// </summary>
    public void CleanEntries()
    {
        serverEntries.Clear();
    }

    public List<LanEntry> GetLanEntries()
    {
        return serverEntries.Count > 0 ? serverEntries : null;
    }

}
