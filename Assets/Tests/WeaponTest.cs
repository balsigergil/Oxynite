/// ETML
/// Author: Gil Balsiger
/// Date: 30.05.2018
/// Summary: Test weapon class

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Test weapon class
/// </summary>
public class NewTestScript
{
    /// <summary>
    /// Fake weapon
    /// </summary>
    Weapon weapon;

    /// <summary>
    /// Fake WeaponManager
    /// </summary>
    WeaponManager wm;

    /// <summary>
    /// Prepare the environment
    /// </summary>
    void Setup()
    {
        // NetworkManager creation
        var nm = new GameObject();
        nm.AddComponent<NetworkManager>();
        NetworkManager.singleton.autoCreatePlayer = false;

        // Starting the server
        NetworkManager.singleton.StartHost();

        // Creating a new weapon 
        var weaponGo = new GameObject();
        weapon = weaponGo.AddComponent<Weapon>();
        NetworkServer.Spawn(weaponGo);
    }

    [UnityTest]
    public IEnumerator TestSetup()
    {
        Setup();
        yield return new WaitForFixedUpdate();
    }

    [UnityTest]
    public IEnumerator TestWeaponDecreaseAmmoBasic()
    {
        weapon.ammunition = 10;

        // Decrease ammunition by 1
        CmdDecreaseAmmo();

        yield return new WaitForFixedUpdate();

        // Check if the ammunition is 9
        Assert.AreEqual(weapon.ammunition, 9);
    }

    [UnityTest]
    public IEnumerator TestWeaponDecreaseAmmoZero()
    {
        weapon.ammunition = 0;

        // Decrease ammunition by 1
        CmdDecreaseAmmo();

        yield return new WaitForFixedUpdate();

        // Check if the ammunition is not -1
        Assert.AreEqual(weapon.ammunition, 0);
    }

    [Command]
    void CmdDecreaseAmmo()
    {
        weapon.RpcDecreaseAmmo();
    }
}
