using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHoverHandler : MonoBehaviour {
	public static InteractableHoverHandler instance;

	public IInteractable currentHoverObject;

	private void Awake() {
		instance = this;
	}

	private void Update() {
		CheckMouseHoverOverInteractable();
    }

	private void CheckMouseHoverOverInteractable() {
		Vector3 mousePos = Input.mousePosition;
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider == null)
        {
            ResetCurrentHoverObject();
            return;
        }
        GameObject hitObject = hit.collider.gameObject;

        IInteractable interactable = hitObject.GetComponent<IInteractable>();
		if (interactable == null) {
            ResetCurrentHoverObject();
			return;
		}
        if (currentHoverObject == interactable) {
			return;
		}
        ResetCurrentHoverObject();
		interactable.SetHighlight(true);
		currentHoverObject = interactable;
	}

	private void ResetCurrentHoverObject() {
		if (currentHoverObject != null) {
			currentHoverObject.SetHighlight(false);
		}
		currentHoverObject = null;
	}
}
