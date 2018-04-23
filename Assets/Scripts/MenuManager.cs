using UnityEngine;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// The MenuManager class handles all interactions with the main menu
/// </summary>
public class MenuManager : MonoBehaviour {

    // Singletons
    OxyniteNetworkManager networkManager;
    OxyniteNetworkDiscovery networkDiscovery;

    /// <summary>
    /// Server slot prefab
    /// </summary>
    [SerializeField] ServerSlot serverItem;

    /// <summary>
    /// Server slots container
    /// </summary>
    [SerializeField] GameObject serverList;

    // Nickname pop-up panel
    [SerializeField] RectTransform nicknamePanel;

    // Nickname input field
    [SerializeField] TMP_InputField nicknameTextField;

    // Nickname text in header of the main menu
    [SerializeField] TMP_Text nicknameText;

    /// <summary>
    /// Gets nickname or asks for new one
    /// </summary>
    void Start () {
        networkManager = OxyniteNetworkManager.GetInstance();
        networkDiscovery = OxyniteNetworkDiscovery.GetInstance();

        if (PlayerPrefs.HasKey("nickname"))
        {
            nicknamePanel.gameObject.SetActive(false);
            nicknameText.text = PlayerPrefs.GetString("nickname");
        }
        else
        {
            nicknamePanel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Handles "host" button action, start new host
    /// </summary>
    public void HostGame()
    {
        networkManager.StartHost();
    }

    /// <summary>
    /// Handles "find" button action, refresh the UI
    /// </summary>
    public void FindGame()
    {
        networkDiscovery = FindObjectOfType<OxyniteNetworkDiscovery>();
        networkDiscovery.StartListening();
        networkDiscovery.CleanEntries();
        CleanServersList();
        Debug.Log("List cleared");
    }

    /// <summary>
    /// Handles "ready" button action
    /// </summary>
    public void ReadyGame()
    {
        networkDiscovery = FindObjectOfType<OxyniteNetworkDiscovery>();
        if (networkDiscovery.GetLanEntries() != null)
        {
            List<LanEntry> entries = networkDiscovery.GetLanEntries();
            LanEntry server = entries[Random.Range(0, entries.Count)];
            networkManager.StartGame(server);
        }
        else
        {
            HostGame();
        }
    }

    /// <summary>
    /// Handles "quit" button action
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Adds new slot to the UI
    /// </summary>
    /// <param name="server"></param>
    public void AddServerSlot(LanEntry server)
    {
        if(serverList != null)
        {
            ServerSlot item = Instantiate(serverItem);
            item.Initialize(server);
            item.transform.SetParent(serverList.transform);
        }
    }

    /// <summary>
    /// Removes all server slots
    /// </summary>
    public void CleanServersList()
    {
        if (serverList)
        {
            for (int i = 0; i < serverList.transform.childCount; i++)
            {
                Destroy(serverList.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Registers nickname in player preferences
    /// </summary>
    public void SaveNickname()
    {
        PlayerPrefs.SetString("nickname", nicknameTextField.text);
        PlayerPrefs.Save();
        nicknameText.text = nicknameTextField.text;
        nicknamePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the pop-up
    /// </summary>
    public void ShowNicknamePanel()
    {
        nicknamePanel.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey("nickname"))
        {
            nicknameTextField.text = PlayerPrefs.GetString("nickname");
        }
    }

}
