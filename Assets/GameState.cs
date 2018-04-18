using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : NetworkBehaviour {

    [Server]
    public void Check()
    {
        if(GameManager.players.Count >= GameManager.MIN_PLAYER && hasAuthority)
        {
            CmdEnableFire();
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
