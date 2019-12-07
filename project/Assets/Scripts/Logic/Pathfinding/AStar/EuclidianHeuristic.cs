using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclidianHeuristic : IAStarHeuristic {
	public float GetDistance(Vector2 from, Vector2 to) {
		return Vector2.Distance(from, to);
	}
}
