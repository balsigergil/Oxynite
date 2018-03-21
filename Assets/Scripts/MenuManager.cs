using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    NetworkManager nm;

    [SerializeField]
    ServerItem serverItem;

    [SerializeField]
    GameObject serverList;

    void Awake () {
        nm = NetworkManager.singleton;
    }

    public void HostGame()
    {
        nm.StartHost();
    }

    public void FindGame()
    {

    }

    public void AddServer(LanEntry server)
    {
        ServerItem item = Instantiate(serverItem);
        item.ip = server.ipAddress;
        item.transform.Find("Name").GetComponent<Text>().text = "LAN: " + server.hostName;
        item.transform.SetParent(serverList.transform);
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
