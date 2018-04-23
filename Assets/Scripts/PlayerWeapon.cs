using System;
using UnityEngine;

/// <summary>
/// Player weapon properties
/// </summary>
[Serializable]
public class PlayerWeapon {

    public string name = "Glock";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0f;

    public GameObject modelPrefab;

}
