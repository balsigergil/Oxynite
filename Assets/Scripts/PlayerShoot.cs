using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles the shooting
/// </summary>
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    // Player tag, same for all player
    private const string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;

    /// <summary>
    /// Player camera for ray casting
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// Layer mask for ray casting
    /// </summary>
    [SerializeField] private LayerMask mask;

    /// <summary>
    /// WeaponManager instance, handles the weapon spawning
    /// </summary>
    private WeaponManager weaponManager;

    void Start()
    {
        if (!cam)
        {
            enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }

    /// <summary>
    /// Handles shooting when firing
    /// </summary>
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (!isLocalPlayer)
            return;

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            // Rapid fire
            if (Input.GetButtonDown("Fire1") && !GameMenu.isOn)
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    /// <summary>
    /// Adds shoot effect on all clients from the server
    /// </summary>
    [Command] void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    /// <summary>
    /// Adds hit effect on all clients from the server
    /// </summary>
    [Command] void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    /// <summary>
    /// Instantiates muzzle flash
    /// </summary>
    [ClientRpc] void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    /// <summary>
    /// Instantiates hit particles
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_normal"></param>
    [ClientRpc] void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hitParticles = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(hitParticles, 1f);
    }

    /// <summary>
    /// Ray-casts on the client only
    /// </summary>
    [Client] private void Shoot()
    {
        if (!isLocalPlayer)
            return;

        CmdOnShoot();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }

            CmdOnHit(_hit.point, _hit.normal);
        }
    }

    /// <summary>
    /// Calls take damage on the server
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="damage"></param>
    [Command] void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot.");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }

}
