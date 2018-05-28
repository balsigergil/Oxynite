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

    private Weapon currentWeapon;

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
        weaponManager = GetComponent<WeaponManager>();
    }

    /// <summary>
    /// Handles shooting when firing
    /// </summary>
    void Update()
    {
        if (!isLocalPlayer)
            return;

        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon != null)
        {
            if (currentWeapon.fireRate <= 0f && Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            else
            {
                // Rapid fire
                if (Input.GetButtonDown("Fire1") && !GameMenu.isOn)
                {
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                    GetComponent<Animator>().SetBool("Fire", true);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                    GetComponent<Animator>().SetBool("Fire", false);
                }
            }
        }
    }

    /// <summary>
    /// Adds shoot effect on all clients from the server
    /// </summary>
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    /// <summary>
    /// Adds hit effect on all clients from the server
    /// </summary>
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    /// <summary>
    /// Instantiates muzzle flash
    /// </summary>
    [ClientRpc]
    void RpcDoShootEffect()
    {
        if (weaponManager.GetCurrentGraphics())
            weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    /// <summary>
    /// Instantiates hit particles
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_normal"></param>
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hitParticles = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(hitParticles, 1f);
    }

    /// <summary>
    /// Ray-casts on the client only
    /// </summary>
    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer)
            return;

        if (!weaponManager.isReloading)
        {
            if (currentWeapon.ammunition > 0)
            {
                CmdOnShoot();

                weaponManager.CmdDecreaseAmmo();

                if (currentWeapon.ammunition == 0)
                    weaponManager.StartCoroutine(weaponManager.ReloadWeapon());

                RaycastHit _hit;
                Camera currentCamera = GetComponent<PlayerController>().GetActiveCam();
                if (Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out _hit, currentWeapon.range, mask))
                {
                    if (_hit.collider.tag == PLAYER_TAG)
                    {
                        CmdPlayerShot(_hit.collider.name, currentWeapon.damage, transform.name);
                    }

                    CmdOnHit(_hit.point, _hit.normal);
                }
            }
            else
            {
                weaponManager.StartCoroutine(weaponManager.ReloadWeapon());
            }
        }

    }

    /// <summary>
    /// Calls take damage on the server
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="damage"></param>
    [Command]
    void CmdPlayerShot(string playerID, int damage, string sourcePlayerID)
    {
        Debug.Log(playerID + " has been shot.");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage, sourcePlayerID);
    }

}
