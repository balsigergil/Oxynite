using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] private Behaviour[] componentsToDisable;

    [SerializeField] private string remoteLayerName = "RemotePlayer";

    [SerializeField] private HUD hudPrefab;

    private HUD hudInstance;

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

            hudInstance = Instantiate(hudPrefab, hudPrefab.transform.position, hudPrefab.transform.rotation);
        }

        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, player);
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

        GameManager.UnregisterPlayer(transform.name);
    }

    public HUD GetHudInstance()
    {
        return hudInstance;
    }

}
