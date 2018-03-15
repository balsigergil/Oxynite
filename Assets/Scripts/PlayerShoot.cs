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
                    CmdPlayerShot(_hit.collider.name);
                }
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + " has been shot.");
        Destroy(GameObject.Find(_ID));
    }

}
