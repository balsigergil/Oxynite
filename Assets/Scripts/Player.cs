using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// Handles player global properties and actions
/// </summary>
public class Player : NetworkBehaviour
{
    [SyncVar] private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SyncVar]
    public string playerName;

    [SerializeField] private int maxHealth = 100;

    /// <summary>
    /// Player current health
    /// </summary>
    [SyncVar] private int health;

    /// <summary>
    /// Player HUD
    /// </summary>
    private HUD hud;

    /// <summary>
    /// Components to disable when player die
    /// </summary>
    [SerializeField] private Behaviour[] disableOnDeath;

    /// <summary>
    /// Components that were enabled before death
    /// </summary>
    private bool[] wasEnabled;

    [SerializeField]
    private EndMenu endMenuPrefab;

    public void Update()
    {
        if (!isLocalPlayer)
            return;

        // Update HUD
        hud.UpdateHealth(health, maxHealth);
    }

    /// <summary>
    /// Initializes player components
    /// </summary>
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        hud = GetComponent<PlayerSetup>().GetHudInstance();

        SetDefaults();
    }

    /// <summary>
    /// Replicates the damage taken over the network
    /// </summary>
    /// <param name="damage"></param>
    [ClientRpc]
    public void RpcTakeDamage(int damage, string sourceID)
    {
        if (isDead)
            return;

        health -= damage;
        Debug.Log(transform.name + " now has " + health + " health.");

        if (health <= 0)
        {
            Die(sourceID);
        }
    }

    /// <summary>
    /// Disables components to disable and respawn the player
    /// </summary>
    private void Die(string sourceID)
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name + " is dead!");

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<Camera>().gameObject.SetActive(false);

        if (isLocalPlayer)
        {
            Player sourcePlayerName = GameManager.GetPlayer(sourceID);

            //Show end menu
            EndMenu endMenu = Instantiate(endMenuPrefab);
            endMenu.SetSourcePlayerName(sourcePlayerName.playerName);

            if (isServer)
                endMenu.DisableButtons();

            sourcePlayerName.GetComponentInChildren<Camera>().enabled = true;
            sourcePlayerName.GetComponentInChildren<Camera>().tag = "MainCamera";

            hud.gameObject.SetActive(false);
            GameObject gameMenu = GameObject.Find("GameMenu");
            gameMenu.SetActive(false);
            endMenu.SetGameMenu(gameMenu);
        }

        

        // StartCoroutine(Respawn());
    }

    /// <summary>
    /// Wait and moves the player to a spawn point
    /// </summary>
    /// <returns></returns>
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        SetDefaults();
        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;

        Debug.Log(transform.name + " re-spawned");
    }

    /// <summary>
    /// Resets health
    /// </summary>
    public void SetDefaults()
    {
        isDead = false;
        health = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

    void Start()
    {
        SetPlayerName();
    }

    [Client]
    void SetPlayerName()
    {
        if (hasAuthority)
        {
            string name = PlayerPrefs.GetString("nickname");
            CmdSetPlayerName(name);
        }
    }

    [Command]
    void CmdSetPlayerName(string playerName)
    {
        RpcSetPlayerName(playerName);
    }

    [ClientRpc]
    void RpcSetPlayerName(string playerName)
    {
        this.playerName = playerName;
        Debug.Log("Setup " + name + " name: " + playerName);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    //TODO: Method currently not implemented
    [ClientRpc]
    public void RpcEnableFire()
    {
        if (!isLocalPlayer)
            return;

        GetComponent<PlayerShoot>().enabled = true;
    }

}
