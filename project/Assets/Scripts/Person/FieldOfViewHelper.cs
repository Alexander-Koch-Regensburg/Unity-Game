using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewHelper : MonoBehaviour {

	private Enemy enemy;
	private float range;
	private float angle;
	private LineRenderer lineRenderer;

	private void Awake() {
		enemy = GetComponent<Enemy>();
		if (enemy == null) {
			Debug.LogError("A FieldOfViewChecker has to be assigned to GameObject with a Person component!");
			return;
		}
		range = enemy.visionRange;
		angle = enemy.visionAngle;
		lineRenderer = GetComponent<LineRenderer>();
	}
	public IList<GameObject> GetObjectsInFieldOfView() {
		IList<GameObject> objects = new List<GameObject>();

		Vector2 overlapPos = (Vector2) transform.position;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapPos, range);

		foreach (Collider2D collider in colliders) {
			if (ObjectIsInSight(collider.gameObject)) {
				objects.Add(collider.gameObject);
			}
		}

		return objects;
	}

	public IInteractable GetInteractableById(string id) {
		Vector2 overlapPos = (Vector2) transform.position;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapPos, range);

		foreach (Collider2D collider in colliders) {
			IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
			if (interactable != null) {
				if (interactable.CanInteract(enemy.transform.position) && interactable.GetID() == id) {
					return interactable;
				}
			}
		}
		return null;
	}

	public void DrawFieldOfView() {

		if (lineRenderer == null) {
			return;
		}

		List<Vector3> points = new List<Vector3>();

		points.Add(transform.position);
		for (int currentAngle = (int) -angle/2; currentAngle <= (int) angle/2; currentAngle++) {
			points.Add(GetFieldOfViewPointForAngle(currentAngle));
		}
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());
	}

	private Vector3 GetFieldOfViewPointForAngle(int angle) {
		int layerMask = LayerMask.GetMask("Obstacle");
		Vector3 forward = transform.right;
		Vector3 direction = RotateVectorByAngle(forward, angle);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, layerMask);
		if (hit) {
			return (hit.point);
		}
		return transform.position + RotateVectorByAngle(forward, angle) * range;
	}

	private Vector3 RotateVectorByAngle(Vector3 vector, float angle) {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
		float newX = (cos * vector.x) - (sin * vector.y);
		float newY = (cos * vector.y) + (sin * vector.x);
		vector.x = newX;
		vector.y = newY;
		return vector;
     }

	private bool ObjectIsInSight(GameObject gameObject) {
		Transform transform = gameObject.transform;
		Vector2? objectDirection = gameObject.transform.position - enemy.transform.position;
		if (objectDirection == null) {
			return false;
		}
		float? angle = Vector2.Angle(objectDirection.Value, enemy.transform.right);
		return angle <= angle / 2;
	}
}
