using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public static bool isOn = false;

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
        Off();
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
