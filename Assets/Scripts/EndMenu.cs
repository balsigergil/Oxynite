using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    private GameObject gameMenu;

    public void Spectate()
    {
        panel.gameObject.SetActive(false);
        gameMenu.SetActive(true);
        GameMenu.lockCursor = false;
        specText.enabled = true;
        specText.text = "Spectating " + sourcePlayerName;
    }

    public void BackToMenu()
    {
        NetworkManager.singleton.StopHost();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableButtons()
    {
        menuButton.interactable = false;
        quitButton.interactable = false;
    }

    public void SetSourcePlayerName(string playerName)
    {
        sourcePlayerName = playerName;
        killText.text = sourcePlayerName + " eliminated you !";
    }

    public void SetGameMenu(GameObject gameMenu)
    {
        this.gameMenu = gameMenu;
    }

    public void Respawn()
    {
        GameManager.GetLocalPlayer().CmdRespawn();
    }

}
