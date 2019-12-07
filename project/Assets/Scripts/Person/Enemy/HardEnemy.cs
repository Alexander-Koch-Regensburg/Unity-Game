using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HardEnemy : Enemy {
    protected override void CheckLineOfSight() {
        base.CheckLineOfSight();
        if (HasPlayerInSight && statusChanged) {
            CallHardEnemies();
        }
    }
    private void CallHardEnemies() {
        foreach(HardEnemy hardEnemy in PersonController.instance.HardEnemies) {
            hardEnemy.SetLastKnownPlayerPosition(lastKnownPlayerPosition);
        }
    }

    public void SetLastKnownPlayerPosition(Vector2 position) {
        lastKnownPlayerPosition = position;
        awarenessSystem.Awareness = awarenessSystem.MaxAwareness;
    }

    protected override void ShootAtPlayer() {
		if (GetAngleToTarget() <= shootAngle && HasPlayerInSight) {
			weapon.Fire();
		}
	}

    /// <summary>
	  /// Calculates direction of player when projectile hits
    /// </summary>
    private Vector2? GetTargetDirection() {
        float? timeTillImpact = PlayerPosUtil.GetDistanceFromPlayer(transform) / weapon.GetProjectileVelocity();
        Vector2? target = PlayerPosUtil.CalculateFuturePosition(timeTillImpact);
        return (target == null) ? null : target - transform.position;
    }

    private float? GetSignedAngleToTarget() {
        if (weapon == null || weapon is MeleeWeapon) {
            return PlayerPosUtil.GetSignedAngleToPlayer(transform);
        }
        Vector2? targetDirection = GetTargetDirection();
		    return Vector2.SignedAngle(targetDirection.Value, transform.right);
    }

    private float? GetAngleToTarget() {
        if (weapon == null || weapon is MeleeWeapon) {
            return PlayerPosUtil.GetAngleToPlayer(transform);
        }
        Vector2? targetDirection = GetTargetDirection();
		    return Vector2.Angle(targetDirection.Value, transform.right);
    }
}
