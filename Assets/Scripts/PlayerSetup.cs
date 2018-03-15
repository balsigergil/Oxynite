using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] Behaviour[] componentsToDisable;

    private Camera sceneCam;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour component in componentsToDisable)
            {
                component.enabled = false;
            }
        }
        else
        {
            sceneCam = Camera.main;
            if(sceneCam)
                sceneCam.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (sceneCam)
            sceneCam.gameObject.SetActive(true);
    }
}
