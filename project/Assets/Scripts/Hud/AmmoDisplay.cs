using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour {
	public Animator animator;
	public PersonController personController;

	public TMP_Text ammoText;

	private const string AMMO_LVL_INT = "AmmoLevel";

	private void Update() {
		UpdateAmmoDisplay();	
	}

	private void UpdateAmmoDisplay() {
		Player player = personController.player;
		if (player == null) {
			animator.SetInteger(AMMO_LVL_INT, 0);
			ammoText.text = "";
			return;
		}
		IWeapon weapon = player.Weapon;
		if (weapon == null) {
			animator.SetInteger(AMMO_LVL_INT, 0);
			ammoText.text = "";
		} else {
			int ammo = weapon.GetAmmo();
			if (ammo < 0) {
				animator.SetInteger(AMMO_LVL_INT, 3);
				ammoText.text = "∞";
				return;
			}
			if (ammo > 16) {
				animator.SetInteger(AMMO_LVL_INT, 3);
			} else if (ammo > 8) {
				animator.SetInteger(AMMO_LVL_INT, 2);
			} else if (ammo > 0) {
				animator.SetInteger(AMMO_LVL_INT, 1);
			} else {
				animator.SetInteger(AMMO_LVL_INT, 0);
			}
			ammoText.text = ammo.ToString();
		}
	}
}
