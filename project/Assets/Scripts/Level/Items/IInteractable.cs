using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

	/// <summary>
	/// Sets whether this <c>IInteractable</c> should be highlighted
	/// </summary>
	/// <param name="highlighted"></param>
	void SetHighlight(bool highlighted);

	/// <summary>
	/// Returns whether the GameObject can currently interact with this <c>IInteractable</c>
	/// </summary>
	/// <returns></returns>
	bool CanInteract(Vector3 position);

	/// <summary>
	/// Notifies this <c>IInteractable</c> that it was interacted with by a <c>Person</c>
	/// </summary>
	/// <param name="player"></param>
	void Interact(Person person);

	/// <summary>
	/// The maximum distance at which the <c>Person</c> can interact
	/// </summary>
	/// <returns></returns>
	float GetMaxInteractionDistance();

	string GetID();
}
