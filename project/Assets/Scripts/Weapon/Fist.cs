using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MeleeWeapon {

    void Start() {
		person = transform.parent.GetComponent<Person>();
        person.Fist = this;
		ammo = -1;
	}
    public override void Fire() {
		if (onCooldown) {
			return;
		}

		CreateProjectile();
        PlayAudio();
		StartCoroutine(WaitForCooldown());
	}
}
