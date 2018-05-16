using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

/// <summary>
/// The MenuManager class handles all interactions with the main menu
/// </summary>
public class MenuManager : MonoBehaviour
{

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
    [SerializeField] RectTransform roomNamePanel;

    // Nickname input field
    [SerializeField] TMP_InputField nicknameTextField;

    // Nickname text in header of the main menu
    [SerializeField] TMP_Text nicknameText;

    [SerializeField] Toggle isLAN;

    [SerializeField] TMP_InputField roomNameText;

    private List<ServerSlot> roomList = new List<ServerSlot>();

    /// <summary>
    /// Gets nickname or asks for new one
    /// </summary>
    void Start()
    {
        networkManager = OxyniteNetworkManager.GetInstance();
        networkDiscovery = OxyniteNetworkDiscovery.GetInstance();

        networkManager.StartMatchMaker();
        RefreshRoomList();

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

    public void RefreshRoomList()
    {
        CleanWANServersList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchCreate);
    }

    public void OnMatchCreate(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (!success || matchList == null)
        {
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            ServerSlot roomListItem = Instantiate(serverItem);
            roomListItem.transform.SetParent(serverList.transform);

            roomList.Add(roomListItem);

            if (roomListItem != null)
            {
                roomListItem.SetupWAN(match, JoinRoom);
            }

        }
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }

    /// <summary>
    /// Handles "host" button action, start new host
    /// </summary>
    public void HostGame()
    {
        if (!isLAN.isOn)
        {
            roomNamePanel.gameObject.SetActive(true);
        }
        else
        {
            networkManager.StartHost();
            networkDiscovery.StartBroadcasting();
        }
    }

    public void HostInternetGame()
    {
        networkManager.StartMatchMaker();
        networkManager.matchMaker.CreateMatch(roomNameText.text, 16, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        networkDiscovery.StopListening();
    }

    /// <summary>
    /// Handles "find" button action, refresh the UI
    /// </summary>
    public void FindGame()
    {
        networkDiscovery = FindObjectOfType<OxyniteNetworkDiscovery>();
        networkDiscovery.StartListening();
        networkDiscovery.CleanEntries();
        CleanLANServersList();
        RefreshRoomList();
        Debug.Log("List cleared");
    }

    /// <summary>
    /// Handles "ready" button action
    /// </summary>
    public void ReadyGame()
    {
        networkDiscovery = FindObjectOfType<OxyniteNetworkDiscovery>();
        
        if (roomList.Count > 0)
        {
            ServerSlot serverSlot = roomList[Random.Range(0, roomList.Count)];
            JoinRoom(serverSlot.GetMatch());
        }
        else if(networkDiscovery.GetLanEntries() != null)
        {
            List<LanEntry> entries = networkDiscovery.GetLanEntries();
            LanEntry server = entries[Random.Range(0, entries.Count)];
            networkManager.StartGame(server.ipAddress);
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
    public void AddLANServerSlot(LanEntry server)
    {
        if (serverList != null)
        {
            ServerSlot item = Instantiate(serverItem);
            item.SetupLAN(server);
            item.transform.SetParent(serverList.transform);
        }
    }

    /// <summary>
    /// Removes all server slots
    /// </summary>
    public void CleanLANServersList()
    {
        if (serverList)
        {
            for (int i = 0; i < serverList.transform.childCount; i++)
            {
                if (serverList.transform.GetChild(i).gameObject.tag == "LAN")
                    Destroy(serverList.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Removes all server slots
    /// </summary>
    public void CleanWANServersList()
    {
        if (serverList)
        {
            for (int i = 0; i < serverList.transform.childCount; i++)
            {
                if (serverList.transform.GetChild(i).gameObject.tag == "WAN")
                    Destroy(serverList.transform.GetChild(i).gameObject);
            }
        }
        roomList.Clear();
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
