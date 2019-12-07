using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveStartBacktracking : PathProcessorBase {

	public RemoveStartBacktracking(IPathProcessor processor) : base(processor) {
	}

	protected override void ProcessPathConcrete(IList<Vector2> path) {
		if (path.Count < 3) {
			return;
		}
		if (Vector2.Distance(path[0], path[2]) < Vector2.Distance(path[1], path[2])) {
			path.RemoveAt(1);
		}
	}
}
