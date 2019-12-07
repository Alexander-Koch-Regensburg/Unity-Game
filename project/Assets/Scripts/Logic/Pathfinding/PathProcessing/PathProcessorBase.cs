using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathProcessorBase : IPathProcessor {

	private IPathProcessor decoratedProcessor;

	public PathProcessorBase(IPathProcessor decoratedProcessor) {
		this.decoratedProcessor = decoratedProcessor;
	}

	public void ProcessPath(IList<Vector2> path) {
		if (path == null) {
			return;
		}
		if (decoratedProcessor != null) {
			decoratedProcessor.ProcessPath(path);
		}
		ProcessPathConcrete(path);
	}

	protected abstract void ProcessPathConcrete(IList<Vector2> path);
}