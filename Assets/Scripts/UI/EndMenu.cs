/// ETML
/// Author: Gil Balsiger
/// Date: 24.04.2018
/// Summary: End menu UI

using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// End menu UI
/// </summary>
public class EndMenu : MonoBehaviour {

    [SerializeField]
    private TMP_Text killText;

    [SerializeField]
    private TMP_Text specText;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private RectTransform panel;

    private string sourcePlayerName = "";

    /// <summary>
    /// When the player click on "Menu"
    /// </summary>
    public void BackToMenu()
    {
        NetworkManager.singleton.StopHost();
    }

    /// <summary>
    /// When the player click on "Quit"
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableButtons()
    {
        menuButton.interactable = false;
        quitButton.interactable = false;
    }

    /// <summary>
    /// Update the killer name on HUD
    /// </summary>
    /// <param name="playerName"></param>
    public void SetSourcePlayerName(string playerName)
    {
        sourcePlayerName = playerName;
        killText.text = sourcePlayerName + " eliminated you !";
        specText.text = "Spectating " + sourcePlayerName;
    }

    public void Respawn()
    {
        GameManager.GetLocalPlayer().CmdRespawn();
    }

}
