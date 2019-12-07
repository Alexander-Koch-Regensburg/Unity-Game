using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAStarHeuristic {
	float GetDistance(Vector2 from, Vector2 to);
}
