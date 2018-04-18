using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";
    public const int MIN_PLAYER = 2;
    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX  + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;

        FindObjectOfType<PlayerCount>().UpdatePlayerCount(players.Count, MIN_PLAYER);
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);

        FindObjectOfType<PlayerCount>().UpdatePlayerCount(players.Count, MIN_PLAYER);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
        {
            GUILayout.Label(playerID + " - " + players[playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

}
