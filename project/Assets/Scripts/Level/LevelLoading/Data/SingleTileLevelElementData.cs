using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleTileLevelElementData : LevelElementData {
	public SingleTileLevelElementData(Vector2Int pos, ElementRotation rotation) : base(pos, rotation) {
	}
}
