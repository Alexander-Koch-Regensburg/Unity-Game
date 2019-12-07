using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ItemData {
	public WeaponType type;
	public int ammo;

	public WeaponData(Vector2 pos, float rotation, WeaponType type, int ammo) : base(pos, rotation) {
		this.type = type;
		this.ammo = ammo;
	}
}
