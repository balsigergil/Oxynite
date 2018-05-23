using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour {

    [SerializeField] private Slider healthBar;

    [SerializeField] private TMP_Text healthText;

    [SerializeField] private PlayerCount playerCount;

    [SerializeField] private TMP_Text headerText;

    [SerializeField] private TMP_Text killText;

    public void UpdateHealth(int health, int maxHealth)
    {
        healthBar.value = (float) health / maxHealth;
        healthText.text = health.ToString();
    }

    private void Start()
    {
        if (GameManager.singleton.gameState == GameState.INGAME)
            headerText.SetText("");
    }

    void Update()
    {
        playerCount.UpdatePlayerCount(GameManager.players.Count);
    }

    public void SetHeaderText(string text)
    {
        headerText.SetText(text);
    }

    public void UpdateKills(int kills)
    {
        killText.SetText(kills + " kill(s)");
    }

}
