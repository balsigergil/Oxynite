using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

    void Start()
    {
        if (!cam)
        {
            enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Shoot !");
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot.");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }

}
