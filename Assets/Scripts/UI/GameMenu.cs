using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameMenu : MonoBehaviour
{

    public static bool isOn = false;
    public static bool lockCursor = true;

    public void QuitGame()
    {
        LeaveGame();
        Application.Quit();
    }

    public void LeaveGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (NetworkManager.singleton.matchInfo != null)
        {
            MatchInfo matchInfo = NetworkManager.singleton.matchInfo;
            NetworkManager.singleton.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, NetworkManager.singleton.OnDropConnection);
        }
        NetworkManager.singleton.StopHost();
    }

    void Start()
    {
        lockCursor = true;
        Off();
    }

    private void OnEnable()
    {
        lockCursor = true;
        Off();
    }

    private void OnDisable()
    {
        lockCursor = false;
        Off();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOn)
            {
                On();
            }
            else
            {
                Off();
            }
        }
    }

    void On()
    {
        isOn = true;
        GetComponent<Canvas>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Off()
    {
        isOn = false;
        GetComponent<Canvas>().enabled = false;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
