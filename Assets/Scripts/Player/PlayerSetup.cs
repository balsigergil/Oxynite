using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] Behaviour[] componentsToDisable;

    [SerializeField] string remoteLayerName = "RemotePlayer";

    private Camera sceneCam;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCam = Camera.main;
            if(sceneCam)
                sceneCam.gameObject.SetActive(false);
        }

        RegisterPlayerName();
    }

    void RegisterPlayerName()
    {
        string id = "Player " + GetComponent<NetworkIdentity>().netId.ToString();
        name = id;
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        foreach (Behaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
    }

    private void OnDisable()
    {
        if (sceneCam)
            sceneCam.gameObject.SetActive(true);
    }
}
