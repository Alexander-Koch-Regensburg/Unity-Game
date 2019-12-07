using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEndBacktracking : PathProcessorBase {

	public RemoveEndBacktracking(IPathProcessor processor) : base(processor) {
	}

	protected override void ProcessPathConcrete(IList<Vector2> path) {
		if (path.Count < 3) {
			return;
		}
		int length = path.Count;
		if (Vector2.Distance(path[length - 3], path[length - 1]) < Vector2.Distance(path[length - 3], path[length - 2])) {
			path.RemoveAt(length - 2);
		}
	}
}
