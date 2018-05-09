using UnityEngine;
using UnityEngine.Networking;

public class GameMenu : MonoBehaviour
{

    public static bool isOn = false;
    public static bool lockCursor = true;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        NetworkManager.singleton.StopHost();
    }

    void Start()
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
