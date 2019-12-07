using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerFollowBehaviour : IPlayerFollowBehaviour {

	private bool reachedLastKnownPos = false;

	public void FollowPlayer(Enemy enemy, Player player) {
		float attackRange = (enemy.Weapon is FireArm) ? enemy.preferredAttackRange : 0f;
		if (enemy.HasPlayerInSight && (PlayerPosUtil.GetDistanceFromPlayer(enemy.transform) < attackRange)) {
			enemy.ChangeDestination(null, 0);
		} else {
			enemy.ChangeDestination(enemy.LastKnownPlayerPosition, enemy.maxVelocity);
		}
		if (Vector2.Distance(enemy.transform.position, enemy.LastKnownPlayerPosition) < 0.5f) {
			reachedLastKnownPos = true;
		}
		if (reachedLastKnownPos == false) {
			enemy.LookAtPlayer();
		}
	}

	public bool LostTrackOfPlayer() {
		return reachedLastKnownPos;
	}

	public void Reset() {
		reachedLastKnownPos = false;
	}
}
