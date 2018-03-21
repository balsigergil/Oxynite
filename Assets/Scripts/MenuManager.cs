using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    NetworkManager nm;

    [SerializeField]
    Button serverItem;

    [SerializeField]
    GameObject serverList;

    // Use this for initialization
    void Start () {
        nm = NetworkManager.singleton;
    }

    public void HostGame()
    {
        nm.StartHost();
    }

    public void FindGame()
    {

    }

    public void AddServer(string server)
    {
        Button item = Instantiate(serverItem);
        item.GetComponentInChildren<Text>().text = server;
        item.transform.SetParent(serverList.transform);
    }

    public void CleanServers()
    {
        for(int i = 0; i < serverList.transform.childCount; i++)
        {
            Destroy(serverList.transform.GetChild(i).gameObject);
        }
    }

}
