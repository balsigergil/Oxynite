using UnityEngine;
using UnityEngine.Networking;

public class ServerItem : MonoBehaviour {

    public string ip;

	public void ConnectToServer()
    {
        Debug.Log("Connecting to " + ip);
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.StartClient();
    }
}
