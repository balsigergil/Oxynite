using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;


    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponInstance = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation) as GameObject;
        _weaponInstance.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            _weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
    }

}
