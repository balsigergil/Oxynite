using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles weapon spawning
/// </summary>
public class WeaponManager : NetworkBehaviour {

    /// <summary>
    /// Weapon layer name for the 2nd camera
    /// </summary>
    [SerializeField] private string weaponLayerName;

    /// <summary>
    /// Spawn point for the weapon
    /// </summary>
    [SerializeField] private Transform weaponHolder;

    /// <summary>
    /// Default weapon
    /// </summary>
    [SerializeField] private PlayerWeapon primaryWeaponPrefab;

    /// <summary>
    /// Equipped weapon
    /// </summary>
    private PlayerWeapon currentWeapon;

    /// <summary>
    /// Graphics of the current weapon
    /// </summary>
    private WeaponGraphics currentGraphics;

    /// <summary>
    /// Equips primary weapon
    /// </summary>
    void Start()
    {
        EquipWeapon(primaryWeaponPrefab);
    }

    /// <summary>
    /// Instantiates weapon and sets graphics
    /// </summary>
    /// <param name="_weapon"></param>
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponInstance = Instantiate(_weapon.modelPrefab, weaponHolder.position, weaponHolder.rotation);
        _weaponInstance.transform.SetParent(weaponHolder);

        // Set graphics if exists
        currentGraphics = _weaponInstance.GetComponent<WeaponGraphics>();
        if (!currentGraphics)
            Debug.Log("No weapon graphics component on " + _weaponInstance.name);

        // Defines layer for local player
        if (isLocalPlayer)
            _weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

}
