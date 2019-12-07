using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelElementData {
	public Vector2Int pos;
	public ElementRotation rotation;

	public LevelElementData(Vector2Int pos, ElementRotation rotation) {
		this.pos = pos;
		this.rotation = rotation;
	}
}


