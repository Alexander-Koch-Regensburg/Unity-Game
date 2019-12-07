using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Person {

	// configurable properties

	/// <summary>
	/// The <code>NavAgent</code> used by the <code>Enemy</code> to navigate the Level
	/// </summary>
	public NavAgent navAgent;

	/// <summary>
	/// Angle in degrees which defines enemy's field of vision
	/// </summary>
	public float visionAngle = 60f;

	/// <summary>
	/// How far the enemy can see
	/// </summary>
	public float visionRange = 10f;

	/// <summary>
	/// The desired range for the enemy to attack
	/// </summary>
	public float preferredAttackRange = 7f;

	/// <summary>
	/// Angle in degrees which defines how good a shot the enemy is
	/// (e.g. 0 for perfect shot)
	/// </summary>
	public float shootAngle = 3f;

	/// <summary>
	/// How long it takes for the enemy to react after discovering the player in seconds
	/// </summary>
	public float reactionTime = 1f;

	/// <summary>
	/// How long it takes for the enemy to disappear after death in seconds
	/// </summary>
	public float fadeout = 1f;

	/// <summary>
	/// The speed of the enemy while patrolling
	/// </summary>
	public float patrolVelocity = 3f;

	public float inciteEnemiesRange = 5f;

	public EnemyAwarenessSystem awarenessSystem;

	// end configurable properties

	/// <summary>
	/// The points between which the enemy patrols
	/// </summary>
	private IList<Vector2> patrolPoints;
	public IList<Vector2> PatrolPoints {
		get {
			return patrolPoints;
		}
		set {
			patrolPoints = value;
		}
	}

	/// <summary>
	/// Whether enemy can see player
	/// </summary>
	private bool hasPlayerInSight = false;
	public bool HasPlayerInSight {
		get {
			return hasPlayerInSight;
		}
	}

	/// <summary>
	/// Whether enemy is attacking player
	/// </summary>
	protected bool isAttacking = false;

	private int nextPointIndex = 0;

	private readonly float minCloseCombatRange = 1.5f;

	/// <summary>
	/// The behaviour defining how the <code>Enemy</code> follows the <code>Player</code>
	/// </summary>
	private IPlayerFollowBehaviour playerFollowBehaviour;

	private bool visualizeFieldOFView = false;

	private FieldOfViewHelper fieldOfViewHelper;

	private Player player;

	protected Vector2 lastKnownPlayerPosition;
	public Vector2 LastKnownPlayerPosition {
		get {
			return lastKnownPlayerPosition;
		}
		set {
			lastKnownPlayerPosition = value;
		}
	}

	protected bool statusChanged = false;

	/// <summary>
	/// Whether enemy's actions are being overridden by ai controller
	/// </summary>
	private bool isOverridden = false;
	public bool IsOverridden {
		get {
			return isOverridden;
		}
		set {
			isOverridden = value;
		}
	}

	/// <summary>
	/// Enemy's unique identifier
	/// </summary>
	private string id = Guid.NewGuid().ToString();
	public string ID {
		get {
			return id;
		}
	}

	/// <summary>
	/// Enemy's type
	/// </summary>
	private EnemyType type;
	public EnemyType Type {
		get {
			return type;
		}
		set {
			this.type = value;
		}
	}

	void Start() {
		if (navAgent != null) {
			navAgent.velocity = patrolVelocity;
			navAgent.acceleration = acceleration;
		}
		player = PersonController.instance.player;
		awarenessSystem.OnAwarenessIncreased += AwarenessIncreased;
		fieldOfViewHelper = GetComponent<FieldOfViewHelper>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (isDead) {
			return;
		}

		if (visualizeFieldOFView) {
			fieldOfViewHelper.DrawFieldOfView();
		}
		CheckLineOfSight();
		if (!isOverridden) {
			if (awarenessSystem.IsAboveThreshold() && !player.IsDead) {
				Attack();
				InciteNearbyEnemies();
			} else {
				if (playerFollowBehaviour != null) {
					playerFollowBehaviour.Reset();
				}
				Patrol();
			}
		}
	}

	public void SetPlayerFollowBehaviour(IPlayerFollowBehaviour behaviour) {
		this.playerFollowBehaviour = behaviour;
	}

	public void VisualizeFieldOfView() {
		visualizeFieldOFView = true;
	}

	/// <summary>
	/// Checks if player is in the line of sight
	/// </summary>
	protected virtual void CheckLineOfSight() {
		// perform check
		bool oldStatus = hasPlayerInSight;
		hasPlayerInSight = PlayerPosUtil.GetIsPlayerInSight(transform, visionAngle, visionRange);
		statusChanged = (oldStatus == hasPlayerInSight);

		if (hasPlayerInSight) {
			lastKnownPlayerPosition = player.transform.position;
		}

		// react
		if (hasPlayerInSight && awarenessSystem.Awareness != awarenessSystem.MaxAwareness) {
			awarenessSystem.Awareness = awarenessSystem.MaxAwareness;
		} else if (statusChanged && !hasPlayerInSight && !awarenessSystem.IsCoolingDown && awarenessSystem.Awareness != 0) {
			awarenessSystem.StartCooldown();
		}
	}

	/// <summary>
	/// Routine for shooting at the player
	/// </summary>
	protected IEnumerator AttackPlayer() {
		ChangeDestination(null, 0);
		yield return new WaitForSeconds(reactionTime);
		while (isAttacking && !isDead && !player.IsDead) {
			yield return new WaitForEndOfFrame();
			if (playerFollowBehaviour != null) {
				playerFollowBehaviour.FollowPlayer(this, player);
			}
			if (weapon != null && weapon is FireArm) {
				if (Weapon.GetAmmo() == 0) {
					DropWeapon();
				} else {
					ShootAtPlayer();
				}
			} else {
				CloseCombatAttack();
			}
		}
	}

	protected virtual void ShootAtPlayer() {
		if (PlayerPosUtil.GetAngleToPlayer(transform) <= shootAngle && hasPlayerInSight) {
			weapon.Fire();
		}
	}

	public override void CloseCombatAttack() {
		if (PlayerPosUtil.GetDistanceFromPlayer(transform) > minCloseCombatRange) {
			return;
		}
		if (weapon != null) {
			weapon.Fire();
		} else {
			base.CloseCombatAttack();
		}
	}

	public override void Attack() {
		if (!isAttacking) {
			isAttacking = true;
			StartCoroutine(AttackPlayer());
		}
	}

	/// <summary>
	/// Moves enemy towards the next patrol point
	/// </summary>
	public virtual void Patrol() {
		isAttacking = false;
		if (GetHasReachedPatrolPoint()) {
			UpdatePatrolPointIndex();
		} else {
			LookAtPatrolPoint();
			ChangeDestination(GetNextPatrolPoint(), patrolVelocity);
		}
	}

	/// <summary>
	/// Rotates enemy towards the given angle
	/// </summary>
	public void RotateEnemyToAngle(float targetRotation) {
		Rigidbody2D enemyRigidBody = GetComponent<Rigidbody2D>();
		float angleLerp = Mathf.LerpAngle(enemyRigidBody.rotation, targetRotation, turnVelocity * Time.deltaTime) % 360;
		enemyRigidBody.SetRotation(angleLerp);
	}

	/// <summary>
	/// Changes the navmesh agent's destination
	public void ChangeDestination(Vector2? destination, float velocity) {
		navAgent.velocity = velocity;
		navAgent.Destination = destination;

		if (destination == null) {
			StopAudio();
		} else {
			PlayAudio();
		}
	}

	/// <summary>
	/// Makes enemy look towards player
	/// </summary>
	public virtual void LookAtPlayer() {
		float? angle = PlayerPosUtil.GetSignedAngleToPlayer(transform);
		RotateEnemyByAngle(angle);
	}

	/// <summary>
	/// Makes enemy look towards next patrolpoint
	/// </summary>
	protected void LookAtPatrolPoint() {
		Vector2 direction = GetNextPatrolPoint() - (Vector2)transform.position;
		float angle = Vector2.SignedAngle(direction, transform.right);
		RotateEnemyByAngle(angle);
	}

	/// <summary>
	/// Rotates enemy by an angle
	/// </summary>
	protected void RotateEnemyByAngle(float? angle) {
		if (angle == null) {
			return;
		}
		Rigidbody2D enemyRigidBody = GetComponent<Rigidbody2D>();
		float targetRotation = enemyRigidBody.rotation - angle.Value;
		RotateEnemyToAngle(targetRotation);
	}

	/// <summary>
	/// Moves enemy towards the next patrol point
	/// </summary>
	protected void UpdatePatrolPointIndex() {
		nextPointIndex++;
		if (nextPointIndex >= patrolPoints.Count) {
			nextPointIndex = 0;
		}
	}

	/// <summary>
	/// Moves enemy towards the next patrol point
	/// </summary>
	protected bool GetHasReachedPatrolPoint() {
		if (Vector2.Distance(transform.position, GetNextPatrolPoint()) > 0.3f) {
			return false;
		}
		return true;
	}

	protected Vector2 GetNextPatrolPoint() {
		Vector2 point = patrolPoints[nextPointIndex];
		Vector2 offset = new Vector2(0.5f, 0.5f);
		return point + offset;
	}

	private void PlayAudio() {
		audioComponent.Play("moving");
	}

	private void StopAudio() {
		audioComponent.StopPlaying();
	}

	public override void Die() {
		if (weapon != null) {
			weapon.SetPerson(null);
		}
		GetComponent<CircleCollider2D>().enabled = false;
		ChangeDestination(null, 0);
		PersonController.instance.KillEnemy(this);
		isDead = true;
	}

	public Vector2 getActualPosition() {
		return transform.position;
	}

	/// <summary>
	/// Get nearby enemies in range of the enemy
	/// </summary>
	/// <param name="distance"></param>
	/// <returns></returns>
	protected List<Enemy> GetNearbyEnemies() {
		List<Enemy> enemies = new List<Enemy>();
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, inciteEnemiesRange);

		if (colliders == null) {
			return enemies;
		}
		foreach (Collider2D collider in colliders) {
			Enemy enemy = collider.gameObject.GetComponentInChildren<Enemy>();
			if (enemy != null && enemy != this) {
				enemies.Add(enemy);
			}
		}
		return enemies;
	}

	/// <summary>
	///  Incite nearby enemies to attack the player
	/// </summary>
	protected virtual void InciteNearbyEnemies() {
		List<Enemy> enemies = GetNearbyEnemies();
		foreach (Enemy enemy in enemies) {
			enemy.awarenessSystem.Awareness = this.awarenessSystem.Awareness;
		}
	}

	private void AwarenessIncreased() {
		lastKnownPlayerPosition = player.transform.position;
		if (playerFollowBehaviour != null) {
			playerFollowBehaviour.Reset();
		}
	}
}
