using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Person {

    // Static instance for singleton pattern. 
    public static Player instance;

	const string horizontalAxis = "Horizontal";
	const string verticalAxis = "Vertical";

	/// <summary>
	/// The determines the maximum speed, when sneaking,
	/// by multiplying the maximum speed with this ration
	/// </summary>
	[Range(0f, 1f)]
	public float sneakVelocityRatio;

	private bool isSneaking = false;

	private Rigidbody2D playerRigidbody;
	private InteractableProximityChecker interactableChecker;

	private float inputHorizontalAxis = 0f;
	private float inputVerticalAxis = 0f;

    void Awake() {
        instance = this;
    }

    private void Start() {
		PlayerCam.instance.InitPos(this);
		playerRigidbody = GetComponent<Rigidbody2D>();
		interactableChecker = GetComponent<InteractableProximityChecker>();
		if (interactableChecker == null) {
			Debug.LogError("Player has no InteractableProximityChecker assigned!");
		}
	}

	private void Update() {
		HandleInput();
		CheckAmmoPickup();
	}

	private void HandleInput() {
		inputHorizontalAxis = Input.GetAxisRaw(horizontalAxis);
		inputVerticalAxis = Input.GetAxisRaw(verticalAxis);

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			isSneaking = true;
		} else if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isSneaking = false;
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			DropWeapon();
		}

		if (Input.GetKey(KeyCode.Mouse0)) {
			Attack();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			IInteractable currentHoverInteractable = InteractableHoverHandler.instance.currentHoverObject;
			if (currentHoverInteractable == null) {
				IInteractable nearestInteractable = interactableChecker.GetNearestInteractable();
				if (nearestInteractable != null) {
					nearestInteractable.Interact(this);
				}
			} else {
				currentHoverInteractable.Interact(this);
			}
		}
	}

	private void CheckAmmoPickup() {
		if (weapon == null) {
			return;
		}
		if (weapon.GetAmmo() < 0) {
			return;
		}

		IInteractable interactable = interactableChecker.GetNearestInteractable();
		if (interactable == null) {
			return;
		}
		if (interactable.Equals(weapon)) {
			return;
		}
		if (interactable.GetType().IsAssignableFrom(weapon.GetType())) {
			Weapon pickup = (Weapon)interactable;
			int pickupAmmo = pickup.GetAmmo();
			int newAmmo;
			if (pickupAmmo < 0) {
				newAmmo = -1;
			} else {
				newAmmo = pickupAmmo + weapon.GetAmmo();
			}
			weapon.SetAmmo(newAmmo);
            WeaponAudioComponent audioComp = weapon.GetAudioComponent();
            audioComp.Play("onCollecting");

            Destroy(pickup.gameObject);
		}
	}

	private void FixedUpdate() {
		UpdateVelocity();
		UpdateLookDir();
	}

	private void UpdateVelocity() {

		Vector2 runDir = new Vector2(inputHorizontalAxis, inputVerticalAxis).normalized;
		Vector2 targetVelocity = runDir * maxVelocity;
        if (isSneaking) {
			targetVelocity *= sneakVelocityRatio;
		}

		Vector2 forceToApply = (targetVelocity - playerRigidbody.velocity) * acceleration * Time.deltaTime;
        float distance = Vector2.Distance(forceToApply, runDir);
        if (distance > 0.1f) {
			if (!isSneaking) {
                audioComponent.Play("moving");
			}
        }
        playerRigidbody.AddForce(forceToApply);
	}

	/// <summary>
	/// Updates the rotation of the player to look towards the mouse-position
	/// </summary>
	private void UpdateLookDir() {
		Vector3 mousePos = Input.mousePosition;
		Camera cam = PlayerCam.instance.GetComponent<Camera>();
		Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

		float angle = Vector2.SignedAngle(Vector2.right, worldPos - transform.position);

		float angleLerp = Mathf.LerpAngle(playerRigidbody.rotation, angle, turnVelocity * Time.deltaTime) % 360;
		playerRigidbody.SetRotation(angleLerp);
	}

	public override void Attack() {
		if (weapon == null) {
			CloseCombatAttack();
		} else {
			weapon.Fire();
		}
	}
	
	public override void Die() {
		isDead = true;
		Debug.Log("Player died");
	}

	public Vector2 GetCurrentVelocity() {
        return playerRigidbody.velocity;
	}

    // Get the current position of the player. 
    public Vector2 getPlayerPosition() {
        return playerRigidbody.position;
    }
}
