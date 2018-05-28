using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour {

    public SphereCollider gravityCollider;

    public int damage = 10;
    public float range = 100f;

    public int magazineSize = 50;
    public int totalAmmunitionCount = 200;

    [SyncVar]
    public int ammunition = 50;

    [SyncVar]
    public int ammunitionCount = 200;

    public float reloadTime = 2.0f;

    public float fireRate = 0f;

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
