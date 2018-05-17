using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles weapon spawning
/// </summary>
public class WeaponManager : NetworkBehaviour
{

    /// <summary>
    /// Weapon layer name for the 2nd camera
    /// </summary>
    [SerializeField] private string weaponLayerName;

    /// <summary>
    /// Spawn point for the weapon
    /// </summary>
    [SerializeField] private Transform weaponHolder;

    /// <summary>
    /// Equipped weapon
    /// </summary>
    private Weapon currentWeapon;

    private GameObject currentWeaponPickup;

    private GameObject weaponInstance;

    /// <summary>
    /// Graphics of the current weapon
    /// </summary>
    private WeaponGraphics currentGraphics;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentWeapon && isLocalPlayer)
        {
            CmdDropWeapon();
        }
    }

    /// <summary>
    /// Instantiates weapon and sets graphics
    /// </summary>
    /// <param name="_weapon"></param>
    void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        if (weaponInstance)
            Destroy(weaponInstance.gameObject);
        weaponInstance = Instantiate(weapon.modelPrefab, weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);

        // Set graphics if exists
        currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if (!currentGraphics)
            Debug.Log("No weapon graphics component on " + weaponInstance.name);

        // Defines layer for local player
        if (isLocalPlayer)
            weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    [Command]
    void CmdDropWeapon()
    {
        RpcDropWeapon();
    }

    [ClientRpc]
    void RpcDropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon = null;
            currentGraphics = null;
            currentWeaponPickup.gameObject.SetActive(true);
            currentWeaponPickup.transform.position = transform.position + transform.forward;
            Quaternion newRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(-90, 0, 0);
            currentWeaponPickup.transform.rotation = newRotation;
            GetComponent<PlayerShoot>().CancelInvoke("Shoot");
            Destroy(weaponInstance.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "WeaponItem" && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            if (other.GetComponent<WeaponPickup>())
            {
                CmdPickWeapon(other.gameObject);
            }
            else
                Debug.LogError("No WeaponItem for " + other.name);
        }
    }

    [Command]
    void CmdPickWeapon(GameObject weaponPickup)
    {
        RpcPickWeapon(weaponPickup);
    }

    [ClientRpc]
    void RpcPickWeapon(GameObject weaponPickup)
    {
        if (!currentWeapon)
        {
            Debug.Log("Pick weapon for player: " + name);
            EquipWeapon(weaponPickup.GetComponent<WeaponPickup>().weapon);
            currentWeaponPickup = weaponPickup.gameObject;
            weaponPickup.SetActive(false);
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

}
