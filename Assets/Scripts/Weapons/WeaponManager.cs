using System.Collections;
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
    [SerializeField] private Transform rightGunBone;

    /// <summary>
    /// Equipped weapon
    /// </summary>
    public Weapon currentWeapon;

    private GameObject currentWeaponPickup;

    private GameObject weaponInstance;

    public bool isReloading = false;

    private HUD hud;

    /// <summary>
    /// Graphics of the current weapon
    /// </summary>
    private WeaponGraphics currentGraphics;

    [SerializeField] private RuntimeAnimatorController controllerWithWeapon;
    [SerializeField] private RuntimeAnimatorController controllerWithoutWeapon;

    void Awake()
    {
        GetComponent<Animator>().runtimeAnimatorController = controllerWithoutWeapon;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            hud = GetComponent<PlayerSetup>().GetHudInstance();
            hud.SetAmmunitionText("");
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Q) && currentWeapon != null)
        {
            GetComponent<PlayerSetup>().GetHudInstance().GetLoader().StopReloading();
            StopAllCoroutines();
            isReloading = false;
            CmdDropWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentWeapon != null && currentWeapon.ammunitionCount > 0 && currentWeapon.ammunition < currentWeapon.magazineSize)
        {
            StartCoroutine(ReloadWeapon());
        }

        if (hud != null && currentWeapon != null)
            hud.SetAmmunitionText(currentWeapon.ammunition + " / " + currentWeapon.ammunitionCount);

        if (currentWeapon == null)
            hud.SetAmmunitionText("");
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
        weaponInstance = Instantiate(weapon.modelPrefab, rightGunBone);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localScale = weapon.modelPrefab.transform.localScale;
        weaponInstance.transform.localRotation = Quaternion.identity;
        GetComponent<Animator>().runtimeAnimatorController = controllerWithWeapon;
        // Set graphics if exists
        currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if (!currentGraphics)
            Debug.Log("No weapon graphics component on " + weaponInstance.name);

        // Defines layer for local player
        if (isLocalPlayer)
            weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    public IEnumerator ReloadWeapon()
    {
        isReloading = true;
        if (currentWeapon.ammunitionCount > 0 && currentWeapon.ammunition < currentWeapon.magazineSize)
            hud.GetLoader().StartReloading(currentWeapon.reloadTime);

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        if (currentWeapon != null)
        {
            int delta = currentWeapon.magazineSize - currentWeapon.ammunition;
            if (currentWeapon.ammunitionCount - delta >= 0)
            {
                CmdSetAmmo(currentWeapon.magazineSize);
                CmdSetAmmoTot(currentWeapon.ammunitionCount - delta);
            }
            else
            {
                CmdSetAmmo(currentWeapon.ammunitionCount + currentWeapon.ammunition);
                CmdSetAmmoTot(0);
            }
        }
        isReloading = false;
    }

    [Command]
    public void CmdDecreaseAmmo()
    {
        currentWeapon.RpcDecreaseAmmo();
    }

    [Command]
    public void CmdSetAmmo(int ammo)
    {
        currentWeapon.RpcSetAmmo(ammo);
    }

    [Command]
    public void CmdSetAmmoTot(int ammo)
    {
        currentWeapon.RpcSetAmmoTot(ammo);
    }

    [Command]
    void CmdDropWeapon()
    {
        RpcDropWeapon();
    }

    [ClientRpc]
    void RpcDropWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon = null;
            currentGraphics = null;
            currentWeaponPickup.gameObject.SetActive(true);
            currentWeaponPickup.transform.position = transform.position + transform.forward;
            Quaternion newRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(-90, 0, 0);
            currentWeaponPickup.transform.rotation = newRotation;
            GetComponent<PlayerShoot>().CancelInvoke("Shoot");
            GetComponent<Animator>().runtimeAnimatorController = controllerWithoutWeapon;
            Destroy(weaponInstance.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "WeaponItem" && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            if (other.GetComponent<Weapon>())
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
        if (currentWeapon == null)
        {
            Debug.Log("Pick weapon for player: " + name);
            EquipWeapon(weaponPickup.GetComponent<Weapon>());
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
