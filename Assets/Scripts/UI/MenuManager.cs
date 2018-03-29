using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The MenuManager class handles all interactions with the main menu
/// </summary>
public class MenuManager : MonoBehaviour {

    OxyniteNetworkManager nm;
    OxyniteNetworkDiscovery nd;

    /// <summary>
    /// Server slot prefab
    /// </summary>
    [SerializeField]
    ServerSlot serverItem;

    /// <summary>
    /// Server slots container
    /// </summary>
    [SerializeField]
    GameObject serverList;

    [SerializeField]
    RectTransform nicknamePanel;

    [SerializeField]
    TMP_InputField nicknameTextField;

    [SerializeField]
    TMP_Text nicknameText;

    void Start () {
        nm = OxyniteNetworkManager.GetInstance();
        nd = OxyniteNetworkDiscovery.GetInstance();

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
        nm.StartHost();
    }

    /// <summary>
    /// Handles "find" button action, refresh the UI
    /// </summary>
    public void FindGame()
    {
        nd = FindObjectOfType<OxyniteNetworkDiscovery>();
        nd.StartListening();
        nd.CleanEntries();
        CleanServers();
        Debug.Log("List cleared");
    }

    /// <summary>
    /// Handles "ready" button action
    /// </summary>
    public void ReadyGame()
    {
        nd = FindObjectOfType<OxyniteNetworkDiscovery>();
        if (nd.GetLanEntries() != null)
        {
            List<LanEntry> entries = nd.GetLanEntries();
            LanEntry server = entries[Random.Range(0, entries.Count)];
            nm.StartGame(server);
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
    /// Add new slot to the UI
    /// </summary>
    /// <param name="server"></param>
    public void AddServer(LanEntry server)
    {
        if(serverList != null)
        {
            ServerSlot item = Instantiate(serverItem);
            item.Initialize(server);
            item.transform.SetParent(serverList.transform);
        }
    }

    /// <summary>
    /// Remove all server slots
    /// </summary>
    public void CleanServers()
    {
        if (serverList)
        {
            for (int i = 0; i < serverList.transform.childCount; i++)
            {
                Destroy(serverList.transform.GetChild(i).gameObject);
            }
        }
    }

    public void SaveNickname()
    {
        PlayerPrefs.SetString("nickname", nicknameTextField.text);
        PlayerPrefs.Save();
        nicknameText.text = nicknameTextField.text;
        nicknamePanel.gameObject.SetActive(false);
    }

    public void ShowNicknamePanel()
    {
        nicknamePanel.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey("nickname"))
        {
            nicknameTextField.text = PlayerPrefs.GetString("nickname");
        }
    }

}
