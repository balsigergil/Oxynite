using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameStateType
{
    Waiting,
    Playing
}

public class GameState : NetworkBehaviour {

    [Server]
    public void Check()
    {
        if(GameManager.players.Count >= GameManager.MIN_PLAYER && hasAuthority)
        {
            CmdEnableFire();
            Debug.Log("Enabling fire on all players");
        }
        else
        {
            Debug.Log("Not enough players");
        }
    }

    [Command]
    void CmdEnableFire()
    {
        foreach (Player p in GameManager.players.Values)
        {
            p.RpcEnableFire();
        }
    }
}
