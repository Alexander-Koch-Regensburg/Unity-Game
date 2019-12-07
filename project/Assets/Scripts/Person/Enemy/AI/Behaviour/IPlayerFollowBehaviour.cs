using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerFollowBehaviour {
	/// <summary>
	/// Makes the enemy follow the player
	/// </summary>
	void FollowPlayer(Enemy enemy, Player player);

	/// <summary>
	/// Returns whether the enemy lost track of the player
	/// </summary>
	bool LostTrackOfPlayer();

	/// <summary>
	/// Notifies the behaviour that the enemy should no longer follow the player
	/// </summary>
	void Reset();
}
