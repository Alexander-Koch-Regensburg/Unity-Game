using UnityEngine;

public interface IDamageable {

	/// <summary>
	/// Deals the given amount of damage to an entity
	/// </summary>
	/// <param name="damage">The amount of damage</param>
	/// <param name="trigger">The entity causing the damage</param>
	void Damage(int damage, GameObject trigger);
}
