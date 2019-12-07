using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPosUtil {

    /// <summary>
	/// Returns the direction of the player in relation to a GameObject's transform.
	/// Return <code>null</code> if the direction to the player cannot be calculated.
	/// </summary>
    /// <param name="transform">The transform of a GameObject</param>
	/// <returns>Vector 2</returns>
    public static Vector2? GetPlayerDirection(Transform transform) {
		if (IsPlayerPresent() == false) {
			return null;
		}
		Player player = PersonController.instance.player;
		return player.transform.position - transform.position;
	}

    /// <summary>
	/// Returns the angle between a GameObject's transform and the player position.
	/// Returns <code>null</code> if the angle cannot be calculated.
	/// </summary>
    /// <param name="transform">The transform of a GameObject</param>
	/// <returns>float angle in degrees</returns>
	public static float? GetAngleToPlayer(Transform transform) {
		Vector2? playerDirection = GetPlayerDirection(transform);
		if (playerDirection == null) {
			return null;
		}
		return Vector2.Angle(playerDirection.Value, transform.right);
	}

	//// <summary>
	/// Returns the signed angle between a GameObject's transform and the player position.
	/// Returns <code>null</code> if the angle cannot be calculated.
	/// </summary>
	/// <param name="transform">The transform of a GameObject</param>
	/// <returns>float angle in degrees</returns>
	public static float? GetSignedAngleToPlayer(Transform transform) {
		Vector2? playerDirection = GetPlayerDirection(transform);
		if (playerDirection == null) {
			return null;
		}
		return Vector2.SignedAngle(playerDirection.Value, transform.right);
	}

	/// <summary>
	/// Returns the distance between a GameObject's transform and the player.
	/// Returns <code>null</code> if the angle cannot be calculated.
	/// </summary>
	/// <param name="transform">The transform of a GameObject</param>
	/// <returns>float distance</returns>
	public static float? GetDistanceFromPlayer(Transform transform) {
		if (IsPlayerPresent() == false) {
			return null;
		}
		Player player = PersonController.instance.player;
		return Vector2.Distance(transform.position, player.transform.position);
	}
	
	/// <summary>
	/// Returns whether the player can be seen by an NPC
	/// </summary>
    /// <param name="transform">The transform of an NPC</param>
	/// <param name="visionAngle">Defines the NPCs's field of vision</param>
	/// <returns>bool isInSight</returns>
	public static bool GetIsPlayerInSight(Transform nPCTransform, float visionAngle, float visionRange) {
		Vector2? playerDirection = GetPlayerDirection(nPCTransform);
		if (playerDirection == null) {
			return false;
		}
		float? playerAngle = GetAngleToPlayer(nPCTransform);
		float? playerDistance = GetDistanceFromPlayer(nPCTransform);
		if ((playerAngle == null) || (playerDistance == null)) {
			return false;
		}

		if (playerAngle.Value <= (visionAngle / 2) && playerDistance.Value <= visionRange) {
			int layerMask = LayerMask.GetMask("Obstacle");
			float raycastLength = Math.Min(visionRange, playerDistance.Value);
			RaycastHit2D hit = Physics2D.Raycast(nPCTransform.position, playerDirection.Value.normalized, raycastLength, layerMask);
			if (!hit) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Calculates future position of player
	/// </summary>
	/// <param name="time">The time for the player's movement</param>
	/// <returns>bool isInSight</returns>
	public static Vector2? CalculateFuturePosition(float? time) {
		if (!IsPlayerPresent() || time == null) {
			return null;
		}
		Player player = PersonController.instance.player;
		Vector2 movement = player.GetCurrentVelocity() * (float) time;
		return (Vector2) player.transform.position + movement;
	}

	/// <summary>
	/// Checks whether a <code>Player</code> is present in the Scene
	/// </summary>
	/// <returns></returns>
	public static bool IsPlayerPresent() {
		if (PersonController.instance == null || PersonController.instance.player == null) {
			return false;
		}
		return true;
	}
}