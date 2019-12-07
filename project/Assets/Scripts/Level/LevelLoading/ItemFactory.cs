using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory {

	public static Weapon InstantiateWeapon(WeaponData data, Transform parent) {
		if (data == null) {
			return null;
		}
		Weapon weapon;
		switch (data.type) {
			case WeaponType.PISTOL:
				weapon = GameObject.Instantiate(WeaponPrefabTable.instance.pistolPrefab, parent);
				break;
            case WeaponType.MACHINEGUN:
                weapon = GameObject.Instantiate(WeaponPrefabTable.instance.machineGunPrefab, parent);
                break;
            case WeaponType.SHOTGUN:
                weapon = GameObject.Instantiate(WeaponPrefabTable.instance.shotgunPrefab, parent);
                break;
			case WeaponType.BAT:
                weapon = GameObject.Instantiate(WeaponPrefabTable.instance.batPrefab, parent);
                break;
            case WeaponType.INVALID:
                // This weapon should not be created: ignore
                return null;
			default:
				Debug.Log("Weapon of type: " + data.type + " cannot be instantiated");
				return null;
		}
		if (weapon == null) {
			return null;
		}
		weapon.transform.position = new Vector3(data.pos.x, data.pos.y, 0);
		weapon.transform.eulerAngles = new Vector3(weapon.transform.eulerAngles.x, weapon.transform.eulerAngles.y, data.rotation);

		weapon.SetAmmo(data.ammo);

		return weapon;
	}
}
