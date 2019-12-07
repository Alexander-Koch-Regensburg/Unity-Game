using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : RectLevelElement {

    public Door()
    {
        this.size = new Vector2Int(2, 1);
    }

	/// <summary>
	/// The physical collider of the door
	/// </summary>
	public Collider2D doorCollider;

	public Animator animator;
	private const string isOpenAnimationParam = "isOpen";

	/// <summary>
	/// The time in s that needs to pass until the door gets closed again
	/// </summary>
	public float closeTimeout;

	private bool isOpen;
	private float closeTimeLeft;

	private bool isLocked = false;
	/// <summary>
	/// Determines whether the door is locked
	/// </summary>
	public bool IsLocked {
		get {
			return isLocked;
		}
		set {
			isLocked = value;
			if (value == true) {
				Close();
			}
		}
	}

	public override bool IsSolid() {
		return false;
	}

	private void Update() {
		if (isOpen) {
			UpdateCloseTime();
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		Person person = other.GetComponent<Person>();
		HandlePersonTrigger(person);
	}

	/// <summary>
	/// Handles a person entering the doors trigger area
	/// </summary>
	/// <param name="person"></param>
	private void HandlePersonTrigger(Person person) {
		if (person == null) {
			return;
		}

		if (isLocked) {
			return;
		}

		if (isOpen == false) {
			Open();
		}
		ResetCloseTimer();
	}

	/// <summary>
	/// Opens the door
	/// </summary>
	private void Open() {
		isOpen = true;
		doorCollider.enabled = false;
		animator.SetBool(isOpenAnimationParam, true);
	}

	/// <summary>
	/// Closes the door
	/// </summary>
	private void Close() {
		isOpen = false;
		doorCollider.enabled = true;
		animator.SetBool(isOpenAnimationParam, false);
	}

	/// <summary>
	/// Updates the time left until the door is closed again
	/// </summary>
	private void UpdateCloseTime() {
		closeTimeLeft -= Time.deltaTime;
		if (closeTimeLeft <= 0f) {
			Close();
		}
	}

	/// <summary>
	/// Resets the timer until the door is closed again
	/// </summary>
	private void ResetCloseTimer() {
		closeTimeLeft = closeTimeout;
	}
}
