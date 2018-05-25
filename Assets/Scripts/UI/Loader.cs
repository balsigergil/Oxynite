using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour {

    Image img;

    float reloadTime = 5.0f;
    bool reloading = false;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.fillAmount = 0;
    }

    private void FixedUpdate()
    {
        if(reloading)
        {
            img.fillAmount += (1/reloadTime) * Time.deltaTime;
            if (img.fillAmount == 1)
            {
                img.fillAmount = 0;
                reloading = false;
            }
        }
    }

    public void StartReloading(float reloadTime)
    {
        this.reloadTime = reloadTime;
        reloading = true;
    }

    public void StopReloading()
    {
        reloading = false;
        img.fillAmount = 0;
    }

}
