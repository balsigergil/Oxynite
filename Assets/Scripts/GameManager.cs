/// ETML
/// Author: Gil Balsiger
/// Date: 08.05.2018
/// Summary: Handles online players, weapon spawner and start countdown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameState
{
    WAITING,
    INGAME
}

/// <summary>
/// Handles online players, weapon spawner and start countdown
/// </summary>
public class GameManager : NetworkBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    // Minimum of players to start the game
    public const int MIN_PLAYER = 2;

    // Online players
    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    /// <summary>
    /// Weapon spawner game object
    /// </summary>
    [SerializeField] private WeaponSpawner weaponSpawner;

    public static GameManager singleton;

    /// <summary>
    /// Countdown time in seconds
    /// </summary>
    private const float COUNTDOWN = 5;

    /// <summary>
    /// Countdown synchronization
    /// </summary>
    [SyncVar] private float currCountdown;

    private HUD hudInstance;

    [SyncVar] public GameState gameState = GameState.WAITING;

    void Awake()
    {
        singleton = this;
    }

    [Command]
    private void CmdStartGame()
    {
        RpcStartGame();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        gameState = GameState.INGAME;
        StartCoroutine(LoopStartGame());
    }

    /// <summary>
    /// Loops until the countdown is finished, spawn weapons after that
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoopStartGame()
    {
        yield return new WaitForSeconds(2f);

        currCountdown = COUNTDOWN;
        hudInstance = FindObjectOfType<HUD>();

        while (currCountdown > 0)
        {
            hudInstance.SetHeaderText("Lancement dans " + currCountdown);
            yield return new WaitForSeconds(1f);
            currCountdown--;
        }
        hudInstance.SetHeaderText("GOOOO ! ");
        if (hasAuthority)
            weaponSpawner.CmdSpawnWeapons();

        yield return new WaitForSeconds(2f);
        hudInstance.SetHeaderText("");
    }

    /// <summary>
    /// Register a new player when it joined
    /// </summary>
    /// <param name="netID"></param>
    /// <param name="player"></param>
    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX  + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
        if (players.Count >= MIN_PLAYER && singleton.hasAuthority && singleton.gameState == GameState.WAITING)
            singleton.CmdStartGame();
    }

    /// <summary>
    /// Delete player when it disconnected
    /// </summary>
    /// <param name="playerID"></param>
    public static void UnregisterPlayer(string playerID)
    {
        if (players.ContainsKey(playerID))
            players.Remove(playerID);
        else
            Debug.LogError("Player ID '" + playerID + "' not found");
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static Player GetLocalPlayer()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer && player.GetComponent<Player>() != null)
            {
                return player.GetComponent<Player>();
            }
        }
        return null;
    }

}
