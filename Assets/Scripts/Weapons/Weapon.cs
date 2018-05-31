/// ETML
/// Author: Gil Balsiger
/// Date: 15.05.2018
/// Summary: Handles weapon properties and actions

using UnityEngine.Networking;
using UnityEngine;

/// <summary>
/// Handles weapon properties and action, used in the pickup object
/// </summary>
public class Weapon : NetworkBehaviour {

    /// <summary>
    /// Prevent the pickup to fall through the ground
    /// </summary>
    public SphereCollider gravityCollider;

    /// <summary>
    /// Damage per hit
    /// </summary>
    public int damage = 10;

    /// <summary>
    /// Raycast range
    /// </summary>
    public float range = 100f;

    public int magazineSize = 50;

    public int totalAmmunitionCount = 200;

    /// <summary>
    /// Current ammunitions count
    /// </summary>
    [SyncVar] public int ammunition = 50;

    /// <summary>
    /// Current ammunitions in reserve
    /// </summary>
    [SyncVar] public int ammunitionCount = 200;

    public float reloadTime = 2.0f;

    public float fireRate = 0f;

    /// <summary>
    /// Model to instantiate when picking the weapon up
    /// </summary>
    public GameObject modelPrefab;

    [ClientRpc]
    public void RpcDecreaseAmmo()
    {
        if(ammunition > 0)
            ammunition -= 1;
    }

    [ClientRpc]
    public void RpcSetAmmo(int ammo)
    {
        ammunition = ammo;
    }

    [ClientRpc]
    public void RpcSetAmmoTot(int ammo)
    {
        ammunitionCount = ammo;
    }

}
