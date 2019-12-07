using UnityEngine;
using UnityEditor;

public class PlayerData {
    public WeaponType weapon;
    public int ammo;

    public PlayerData(WeaponType weaponType, int ammo) {
        this.weapon = weaponType;
        this.ammo = ammo;
    }

    public PlayerData() { }
}