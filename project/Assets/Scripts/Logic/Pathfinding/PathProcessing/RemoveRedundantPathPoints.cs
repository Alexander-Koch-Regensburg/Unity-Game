using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRedundantPathPoints : PathProcessorBase {

	private const float MAX_LINE_DEVIATION = 0.00001f;

    public RemoveRedundantPathPoints(IPathProcessor processor) : base(processor) {
	}

	protected override void ProcessPathConcrete(IList<Vector2> path) {
		if (path.Count < 3) {
			return;
		}
		ISet<Vector2> pointsToRemove = new HashSet<Vector2>();
		for (int i = 2; i < path.Count; ++i) {
			Vector2 p1 = path[i - 2];
			Vector2 p2 = path[i - 1];
			Vector2 p3 = path[i];
			if (ArePointsInLine(p1, p2, p3)) {
				pointsToRemove.Add(p2);
			}
		}
		foreach (Vector2 point in pointsToRemove) {
			path.Remove(point);
		}
	}

	private bool ArePointsInLine(Vector2 p1, Vector2 p2, Vector2 p3) {
		return Vector2.Angle(p2 - p1, p3 - p1) < MAX_LINE_DEVIATION;
	}
}
