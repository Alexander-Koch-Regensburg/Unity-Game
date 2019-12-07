using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class Person : MonoBehaviour, IDamageable {

	// configurable properties

	/// <summary>
	/// The health of the character
	/// </summary>
	public int health;

	/// <summary>
	/// The maximum running speed of the character
	/// </summary>
	public float maxVelocity = 12f;

	public float turnVelocity = 8f;

	/// <summary>
	/// The acceleration of the character
	/// </summary>
	public float acceleration = 650f;

	/// <summary>
	/// The offset (anchor) at which weapons will be placed.
	/// This could e.g. be the relative position of a characters hand.
	/// </summary>
	public Vector2 weaponAnchor;

    public PersonAudioComponent audioComponent;

	// end configurable properties

    protected Weapon weapon = null;
	public Weapon Weapon {
		get {
			return weapon;
		}
		set {
			weapon = value;
		}
	}

	private Fist fist = null;
	public Fist Fist {
		get {
			return fist;
		}
		set {
			fist = value;
		}
	}

	/// <summary>
	/// Whether person is dead
	/// </summary>
	protected bool isDead = false;
	public bool IsDead {
		get {
			return isDead;
		}
	}

	public delegate void PersonDied();
	/// <summary>
	/// This event gets fired when the <code>NavAgent</code>s destination changes
	/// </summary>
	public event PersonDied OnPersonDied;

	public void Damage(int damage, GameObject trigger) {
		health -= damage;

		if (health <= 0) {
			Die();
			OnPersonDied?.Invoke();
		}
	}

	/// <summary>
	/// Calculates the actual weapon anchor world-position based on the persons rotation and location
	/// </summary>
	/// <returns></returns>
	public Vector3 GetWeaponAnchorPos() {
		Vector2 rotatedAnchor = VectorUtil.RotateVector2ByDeg(weaponAnchor, transform.rotation.eulerAngles.z);
		float xPos = transform.position.x + rotatedAnchor.x;
		float yPos = transform.position.y + rotatedAnchor.y;
		Vector3 weaponWorldPos = new Vector3(xPos, yPos, 0);
		return weaponWorldPos;
	}

	/// <summary>
	/// Fires the persons weapon or launches a close combat attack if the person has no weapon.
	/// </summary>
	public abstract void Attack();

	/// <summary>
	/// Performs a close combat attack
	/// </summary>
	public virtual void CloseCombatAttack() {
		if (fist) {
			fist.Fire();
		}
	}

	/// <summary>
	/// Makes person drop weapon
	/// </summary>
	public void DropWeapon() {
		if (weapon) {
			weapon.SetPerson(null);
			weapon = null;
		}
	}

	/// <summary>
	/// Makes the character die - literally
	/// </summary>
	public abstract void Die();
}
