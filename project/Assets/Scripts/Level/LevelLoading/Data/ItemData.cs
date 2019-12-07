using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData {
	public Vector2 pos;
	public float rotation;

	public ItemData(Vector2 pos, float rotation) {
		this.pos = pos;
		this.rotation = rotation;
	}
}
