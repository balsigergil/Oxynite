using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour {

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private TMP_Text scoreboardItemPrefab;

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

    void ClearScoreboard()
    {
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

}
