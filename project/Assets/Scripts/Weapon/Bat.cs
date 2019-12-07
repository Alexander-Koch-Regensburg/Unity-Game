using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MeleeWeapon {
    public float hitDuration = 0.5f;
    private const string animatorAttackingFlag = "Is_Attacking";

    public override void Fire() {
		if (onCooldown) {
			return;
		}

		CreateProjectile();
        PlayAudio();
		StartCoroutine(WaitForCooldown());
        StartCoroutine(ShowHitAnimation());
	}

    private IEnumerator ShowHitAnimation() {
        if (animator != null) {
			animator.SetBool(animatorAttackingFlag, true);
            yield return new WaitForSeconds(hitDuration);
            animator.SetBool(animatorAttackingFlag, false);
		}
	}

}
