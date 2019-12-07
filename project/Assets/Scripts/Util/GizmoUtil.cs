using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoUtil {
	public const int GIZMO_Z_POS = -5;

	private GizmoUtil() {
	}

	/// <summary>
	/// Draws a Gizmo for the given path consisting of 2D points.
	/// The endpoint of the path is shown red.
	/// </summary>
	public static void DrawPathGizmo(IList<Vector2> path) {
		if (path == null || path.Count == 0) {
			return;
		}

		Gizmos.color = Color.green;
		for (int i = 0; i < path.Count - 1; ++i) {
			Vector2 currPoint = path[i];
			Vector3 currPointPos = new Vector3(currPoint.x, currPoint.y, GIZMO_Z_POS);
			Gizmos.DrawWireSphere(currPointPos, 0.25f);
			Vector2 nextPoint = path[i + 1];
			Gizmos.DrawLine(currPointPos, new Vector3(nextPoint.x, nextPoint.y, GIZMO_Z_POS));
		}
		Gizmos.color = Color.red;
		Vector2 end = path[path.Count -1];
		Gizmos.DrawWireSphere(new Vector3(end.x, end.y, GIZMO_Z_POS), 0.25f);
	}
}
