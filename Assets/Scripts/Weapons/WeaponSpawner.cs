using UnityEngine;
using UnityEngine.Networking;

public class WeaponSpawner : NetworkBehaviour {

    [SerializeField]
    private WeaponPickup[] weaponsPrefab;

    [Command]
	public void CmdSpawnWeapons()
    {
        Debug.Log("Spawning weapons !");
        for (int i = 0; i < transform.childCount; i++)
        {
            WeaponPickup weaponToSpawn = weaponsPrefab[Random.Range(0, weaponsPrefab.Length)];
            WeaponPickup weaponInstance = Instantiate(weaponToSpawn, transform.GetChild(i).transform);
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
