/// ETML
/// Author: Gil Balsiger
/// Date: 16.05.2018
/// Summary: Scoreboard on the game menu

using UnityEngine;
using TMPro;

/// <summary>
/// Scoreboard on the game menu
/// </summary>
public class Scoreboard : MonoBehaviour {

    /// <summary>
    /// Parent for all items
    /// </summary>
    [SerializeField] private RectTransform content;

    /// <summary>
    /// To instantiate when a player joins
    /// </summary>
    [SerializeField] private TMP_Text scoreboardItemPrefab;

    /// <summary>
    /// Update scoreboard each frame
    /// </summary>
    void Update()
    {
        ClearScoreboard();
        foreach(Player player in GameManager.players.Values)
        {
            TMP_Text scoreboardItem = Instantiate(scoreboardItemPrefab);
            scoreboardItem.text = player.playerName;
            scoreboardItem.transform.SetParent(content);
        }
    }

    /// <summary>
    /// Removes all entries
    /// </summary>
    void ClearScoreboard()
    {
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

}
