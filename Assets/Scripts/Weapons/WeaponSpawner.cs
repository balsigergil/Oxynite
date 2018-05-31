/// ETML
/// Author: Gil Balsiger
/// Date: 18.05.2018
/// Summary: Handles the weapons spawning

using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles the weapons spawning
/// </summary>
public class WeaponSpawner : NetworkBehaviour {

    /// <summary>
    /// Different weapons to instantiate randomly
    /// </summary>
    [SerializeField] private Weapon[] weaponsPrefab;

    /// <summary>
    /// Spawn weapons
    /// </summary>
    [Command]
	public void CmdSpawnWeapons()
    {
        Debug.Log("Spawning weapons !");
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapon weaponToSpawn = weaponsPrefab[Random.Range(0, weaponsPrefab.Length)];
            Weapon weaponInstance = Instantiate(weaponToSpawn, transform.GetChild(i).transform);
            NetworkServer.Spawn(weaponInstance.gameObject);
        }
    }

    /// <summary>
    /// Debug spawn of weapons
    /// </summary>
    private void Update()
    {
        if (hasAuthority && Input.GetKeyDown(KeyCode.G))
        {
            CmdSpawnWeapons();
        }
    }

}
