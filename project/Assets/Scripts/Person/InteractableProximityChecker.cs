using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProximityChecker : MonoBehaviour {
	public float checkRadius = 2f;

	private List<IInteractable> nearbyInteractables = new List<IInteractable>();

	private void Start() {
	}

	private void Update() {
		DetectNearbyInteractables();
	}

	private void DetectNearbyInteractables() {
		IInteractable nearestInteractable = GetNearestInteractable();

		ResetHighlight();
		nearbyInteractables.Clear();

		Vector2 overlapPos = (Vector2) transform.position;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapPos, checkRadius);

		if (colliders == null) {
			return;
		}
		foreach (Collider2D collider in colliders) {
			IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
			if (interactable != null) {
				if (interactable.CanInteract(overlapPos)) {
					nearbyInteractables.Add(interactable);
				}
			}
		}

		nearbyInteractables.Sort((a, b) => CompareInteractableDistances(a, b));
		SetHighlightForNearestInteractable();
	}

	private void ResetHighlight() {
		IInteractable nearestInteractable = GetNearestInteractable();
		if (nearestInteractable != null) {
			nearestInteractable.SetHighlight(false);
		}
	}

	private void SetHighlightForNearestInteractable() {
		IInteractable nearestInteractable = GetNearestInteractable();
		if (nearestInteractable != null) {
			// Only highlight the IInteractable if none is directly overed over with the mouse
			if (InteractableHoverHandler.instance.currentHoverObject == null) {
				nearestInteractable.SetHighlight(true);
			}
		}
	}

	private int CompareInteractableDistances(IInteractable i1, IInteractable i2) {
		if ((i1 is MonoBehaviour == false) || (i2 is MonoBehaviour == false)) {
			return 0;
		}
		Transform t1 = ((MonoBehaviour) i1).transform;
		Transform t2 = ((MonoBehaviour) i2).transform;

		return VectorUtil.CompareDistanceToTarget2D(t1.position, t2.position, transform.position);
	}

	/// <summary>
	/// Returns the nearest <c>IInteracteble</c> to this <c>GameObject</c> or null if none is near
	/// </summary>
	/// <returns></returns>
	public IInteractable GetNearestInteractable() {
		if (nearbyInteractables.Count == 0) {
			return null;
		}
		return nearbyInteractables[0];
	}
}
