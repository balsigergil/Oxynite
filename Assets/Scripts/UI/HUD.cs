/// ETML
/// Author: Gil Balsiger
/// Date: 26.04.2018
/// Summary: Head User Display

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Head User Display
/// </summary>
public class HUD : MonoBehaviour {

    /// <summary>
    /// Health slider component
    /// </summary>
    [SerializeField] private Slider healthBar;

    /// <summary>
    /// Ammount of health
    /// </summary>
    [SerializeField] private TMP_Text healthText;

    /// <summary>
    /// Player count component
    /// </summary>
    [SerializeField] private PlayerCount playerCount;

    /// <summary>
    /// Used to display the countdown
    /// </summary>
    [SerializeField] private TMP_Text headerText;

    /// <summary>
    /// Used to display who is the killer
    /// </summary>
    [SerializeField] private TMP_Text killText;

    /// <summary>
    /// Ammunition informations on the HUD
    /// </summary>
    [SerializeField] private TMP_Text ammunitionText;

    /// <summary>
    /// Reloading loader
    /// </summary>
    [SerializeField] private Loader loader;

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

    public void SetAmmunitionText(string text)
    {
        ammunitionText.SetText(text);
    }

    public Loader GetLoader()
    {
        return loader;
    }

}
