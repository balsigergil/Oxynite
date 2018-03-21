using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OxyniteNetworkManager : NetworkManager {

    [SerializeField]
    FPSNetworkDiscovery networkDiscovery;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStartHost()
    {
        base.OnStartHost();
        networkDiscovery.StartBroadcast();
        Debug.Log("Server started !");
    }
}
