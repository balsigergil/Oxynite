using UnityEngine;

[CreateAssetMenu(fileName = "MyWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

    public new string name = "SMG";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0f;

    public GameObject modelPrefab;

}
