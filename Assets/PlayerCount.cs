using UnityEngine;
using TMPro;
public class PlayerCount : MonoBehaviour {

    [SerializeField]
    private TMP_Text text;

    public void UpdatePlayerCount(int count, int total)
    {
        text.SetText(count + " / " + total);
    }

}
