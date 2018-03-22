using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    OxyniteNetworkManager nm;
    OxyniteNetworkDiscovery nd;

    [SerializeField]
    ServerItem serverItem;

    [SerializeField]
    GameObject serverList;

    void Start () {
        nm = (OxyniteNetworkManager)NetworkManager.singleton;
        nd = FindObjectOfType<OxyniteNetworkDiscovery>();
    }

    public void HostGame()
    {
        if(nm)
            nm.StartHost();
    }

    public void FindGame()
    {
        nd.CleanEntries();
        CleanServers();
        Debug.Log("List cleared");
    }

    public void ReadyGame()
    {
        if (nd.GetLanEntries() != null)
        {
            List<LanEntry> entries = nd.GetLanEntries();
            LanEntry server = entries[Random.Range(0, entries.Count)];
            nm.StartGame(server);
        }
        else
        {
            HostGame();
        }
    }

    public void AddServer(LanEntry server)
    {
        if(serverList != null)
        {
            ServerItem item = Instantiate(serverItem);
            item.ip = server.ipAddress;
            item.transform.Find("Name").GetComponent<Text>().text = "LAN: " + server.hostName;
            item.transform.SetParent(serverList.transform);
        }
        else
        {
            Debug.LogError("Server list is null");
        }
    }

    public void CleanServers()
    {
        if (serverList)
        {
            for (int i = 0; i < serverList.transform.childCount; i++)
            {
                Destroy(serverList.transform.GetChild(i).gameObject);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
