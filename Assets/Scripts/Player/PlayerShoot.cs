using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	void Start () {
        if (!cam)
        {
            enabled = false;
        }
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        if (isLocalPlayer)
        {
            RaycastHit _hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
            {
                if(_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(_hit.collider.name, weapon.damage);
                }
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
