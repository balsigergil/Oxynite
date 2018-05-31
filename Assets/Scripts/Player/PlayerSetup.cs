/// ETML
/// Author: Gil Balsiger
/// Date: 20.04.2018
/// Summary: Handles player preparation

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    /// <summary>
    /// Components to disable when a new player joins the game
    /// </summary>
    [SerializeField] private Behaviour[] componentsToDisable;

    /// <summary>
    /// For remote players
    /// </summary>
    [SerializeField] private string remoteLayerName = "RemotePlayer";

    /// <summary>
    /// Player HUD prefab
    /// </summary>
    [SerializeField] private HUD hudPrefab;

    private HUD hudInstance;

    /// <summary>
    /// Disable components to disable if not the local player
    /// </summary>
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            hudInstance = Instantiate(hudPrefab, hudPrefab.transform.position, hudPrefab.transform.rotation);
        }

        GetComponent<Player>().Setup();
    }

    /// <summary>
    /// Register the player
    /// </summary>
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
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
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
        GameManager.UnregisterPlayer(transform.name);
    }

    public HUD GetHudInstance()
    {
        return hudInstance;
    }

}
