using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathProcessor {
	void ProcessPath(IList<Vector2> path);
}
