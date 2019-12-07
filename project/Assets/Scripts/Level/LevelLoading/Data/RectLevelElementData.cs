using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RectLevelElementData : LevelElementData
{
	public Vector2Int size;

	public RectLevelElementData(Vector2Int pos, Vector2Int size, ElementRotation rotation) : base(pos, rotation) {
		this.size = size;
	}
}
