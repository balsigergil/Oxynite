﻿/// ETML
/// Author: Gil Balsiger
/// Date: 20.04.2018
/// Summary: Handles server properties and action in the main menu

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

    [SyncVar] public string playerName;

    /// <summary>
    /// Maximum health
    /// </summary>
    [SerializeField] private int maxHealth = 100;

    /// <summary>
    /// Player current health
    /// </summary>
    [SyncVar] private int health;

    /// <summary>
    /// Kill count
    /// </summary>
    [SyncVar] private int kills;

    /// <summary>
    /// Player HUD
    /// </summary>
    private HUD hud;

    /// <summary>
    /// End menu instance
    /// </summary>
    private EndMenu endMenu;

    /// <summary>
    /// Game menu instance
    /// </summary>
    private GameMenu gameMenu;

    /// <summary>
    /// Components to disable when player die
    /// </summary>
    [SerializeField] private Behaviour[] disableOnDeath;
    [SerializeField] private Transform rightGunBone;

    /// <summary>
    /// Components that were enabled before death
    /// </summary>
    private bool[] wasEnabled;

    /// <summary>
    /// Use to create the end menu
    /// </summary>
    [SerializeField] private EndMenu endMenuPrefab;

    /// <summary>
    /// To deactivate when dying
    /// </summary>
    private GameObject cam;

    /// <summary>
    /// Player who killed this player
    /// </summary>
    private Player sourcePlayer;

    /// <summary>
    /// Update kill count on HUD
    /// </summary>
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
    /// Disables components to disable and activate the spectator camera
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
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        if(rightGunBone.transform.childCount > 0)
            rightGunBone.transform.GetChild(0).gameObject.SetActive(false);

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

            sourcePlayer.GetComponent<PlayerController>().GetTpsCam().enabled = true;

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

        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;

        if (rightGunBone.transform.childCount > 0)
            rightGunBone.transform.GetChild(0).gameObject.SetActive(true);

        cam.SetActive(true);

        if (isLocalPlayer)
        {
            Destroy(endMenu.gameObject);

            sourcePlayer.GetComponent<PlayerController>().GetTpsCam().enabled = false;

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
        if(isLocalPlayer)
            SetPlayerName();
    }

    /// <summary>
    /// Set the player name and informes all clients
    /// </summary>
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

}
