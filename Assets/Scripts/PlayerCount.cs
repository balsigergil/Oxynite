using UnityEngine;
using TMPro;

/// <summary>
/// Displays the number of players on the HUD
/// </summary>
public class PlayerCount : MonoBehaviour {

    [SerializeField]
    private TMP_Text text;

    public void UpdatePlayerCount(int count)
    {
        text.SetText(count + " joueur(s)");
    }

}
