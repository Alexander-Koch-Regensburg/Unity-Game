using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour {

	public delegate void DestinationChanged(NavAgent agent, Vector2? destination);
	/// <summary>
	/// This event gets fired when the <code>NavAgent</code>s destination changes
	/// </summary>
	public event DestinationChanged OnDestinationChanged;

	/// <summary>
	/// The destination to which the <code>NavAgent</code> should move.
	/// A value of <code>null</code> indicates the agent should not move.
	/// </summary>
	public Vector2? Destination {
		get {
			return destination;
		}
		set {
			destination = value;
			OnDestinationChanged?.Invoke(this, destination);
		}
	}
	private Vector2? destination;

	/// <summary>
	/// The minimum distance of the <code>NavAgent</code> to the destination until the <code>NavAgent</code> considers the destination as reached.
	/// This is also the maximum distance the agent can deviate from its path.
	/// Consider using higher values for faster agents.
	/// </summary>
	public float destinationThreshold = 0.1f;

	public Rigidbody2D agentRigidbody;

	/// <summary>
	/// The speed at with which the <code>NavAgent</code> will traverse to given desinations
	/// </summary>
	public float velocity = 1f;

	public float acceleration = 100f;

	private NavMesh2D navMesh;

	private IList<Vector2> currentPath;

	private void Start() {
		navMesh = NavMesh2D.instance;
		Destination = null;
		if (navMesh == null) {
			Debug.LogError("The scene does not contain a NavMesh2D. No navigation possible!");
			return;
		}
		navMesh.RegisterAgent(this);
	}

	private void OnDestroy() {
		if (navMesh == null) {
			return;
		}
		navMesh.UnregisterAgent(this);
	}

	private void Update() {
		TraversePath();
		MoveToNextPathPoint();
	}

	private void OnDrawGizmosSelected() {
		if (currentPath == null) {
			return;
		}
		GizmoUtil.DrawPathGizmo(currentPath);
	}

	/// <summary>
	/// Updates the path to the destination with the new path
	/// </summary>
	/// <param name="path"></param>
	public void UpdatePath(IList<Vector2> path) {
		currentPath = path;
	}

	/// <summary>
	/// Lets the <code>NavAgent</code> traverse alongside the current path, if a destination and path is set
	/// </summary>
	private void TraversePath() {
		if (Destination == null) {
			return;
		}
		if ((currentPath == null) || (currentPath.Count == 0)) {
			return;
		}

		// Destination reached
		if (Vector2.Distance(Destination.Value, transform.position) < destinationThreshold) {
			Destination = null;
			return;
		}

		// Remove next path point if reached
		if (Vector2.Distance(currentPath[0], transform.position) < destinationThreshold) {
			currentPath.RemoveAt(0);
		}
	}

	/// <summary>
	/// Moves the <code>NavAgent</code> to the next point of the current path
	/// </summary>
	private void MoveToNextPathPoint() {
		if (agentRigidbody == null) {
			return;
		}

		Vector2 moveDir;
		if ((currentPath == null) || (currentPath.Count == 0)) {
			moveDir = Vector2.zero;
		} else {
			Vector2 nextPoint = currentPath[0];
			Vector2 agentPos = transform.position;
			moveDir = (nextPoint - agentPos).normalized;
		}

		Vector2 targetVelocity = moveDir * velocity;

		Vector2 forceToApply = (targetVelocity - agentRigidbody.velocity) * acceleration * Time.deltaTime;
		agentRigidbody.AddForce(forceToApply);
	}
}
