using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour {

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private PlayerCount playerCount;

    public void UpdateHealth(int health, int maxHealth)
    {
        healthBar.value = (float) health / maxHealth;
        healthText.text = health.ToString();
    }

    void Update()
    {
        playerCount.UpdatePlayerCount(GameManager.players.Count);
    }

}
