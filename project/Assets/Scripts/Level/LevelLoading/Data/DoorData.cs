using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorData : RectLevelElementData {
	public DoorData(Vector2Int pos, ElementRotation rotation) : base(pos, new Vector2Int(2, 1), rotation) {
	}
}
