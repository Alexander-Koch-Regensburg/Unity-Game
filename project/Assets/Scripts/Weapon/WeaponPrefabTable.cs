using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefabTable : MonoBehaviour {
	public static WeaponPrefabTable instance;

	private void Awake() {
		instance = this;
	}

	public Pistol pistolPrefab;
    public MachineGun machineGunPrefab;
    public Shotgun shotgunPrefab;
	public Bat batPrefab;
}
