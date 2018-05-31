/// ETML
/// Author: Gil Balsiger
/// Date: 24.05.2018
/// Summary: Animated loader used when reloading

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animated loader used when reloading
/// </summary>
public class Loader : MonoBehaviour {

    /// <summary>
    /// Loader image
    /// </summary>
    Image img;

    /// <summary>
    /// Animation duration
    /// </summary>
    float reloadTime = 5.0f;

    bool reloading = false;

    /// <summary>
    /// Reset the image filling
    /// </summary>
    private void Awake()
    {
        img = GetComponent<Image>();
        img.fillAmount = 0;
    }

    /// <summary>
    /// Reload animation per frame
    /// </summary>
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
