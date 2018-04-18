using UnityEngine;
using TMPro;
public class PlayerCount : MonoBehaviour {

    [SerializeField]
    private TMP_Text text;

    public void UpdatePlayerCount(int count)
    {
        text.SetText(count + " joueur(s)");
    }

}
