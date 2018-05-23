using UnityEngine;
using UnityEngine.Networking;

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

    [SyncVar] private int kills;

    /// <summary>
    /// Player HUD
    /// </summary>
    private HUD hud;

    private EndMenu endMenu;

    private GameMenu gameMenu;

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

    private GameObject cam;

    private Player sourcePlayer;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        hud.UpdateKills(kills);
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

        if(isLocalPlayer)
            hud.UpdateHealth(health, maxHealth);

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
        Debug.Log(transform.name + " is dead!");
        isDead = true;
        GetComponent<PlayerShoot>().CancelInvoke("Shoot");

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        cam = GetComponentInChildren<Camera>().gameObject;
        cam.SetActive(false);

        sourcePlayer = GameManager.GetPlayer(sourceID);
        sourcePlayer.AddKill();

        if(hud)
            hud.UpdateKills(kills);


        if (isLocalPlayer)
        {
            // Show end menu
            endMenu = Instantiate(endMenuPrefab);
            endMenu.SetSourcePlayerName(sourcePlayer.playerName);
            GameMenu.lockCursor = false;

            if (isServer)
                endMenu.DisableButtons();

            sourcePlayer.GetComponentInChildren<Camera>().enabled = true;

            hud.gameObject.SetActive(false);
        }
    }

    [Command]
    public void CmdRespawn()
    {
        RpcRespawn();
    }

    /// <summary>
    /// Wait and moves the player to a spawn point
    /// </summary>
    /// <returns></returns>
    [ClientRpc]
    private void RpcRespawn()
    {
        SetDefaults();
        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;

        cam.SetActive(true);

        if (isLocalPlayer)
        {
            Destroy(endMenu.gameObject);

            sourcePlayer.GetComponentInChildren<Camera>().enabled = false;

            GameMenu.lockCursor = true;
            hud.gameObject.SetActive(true);
            hud.UpdateHealth(health, maxHealth);
        }

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
    }

    public void AddKill()
    {
        kills += 1;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public HUD GetHUD()
    {
        return hud;
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
