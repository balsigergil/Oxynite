using UnityEngine;
using TMPro;

/// <summary>
/// Displays the number of players on the HUD
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class PlayerCount : MonoBehaviour {

    public void UpdatePlayerCount(int count)
    {
        GetComponent<TMP_Text>().SetText(count + " joueur(s)");
    }

}
