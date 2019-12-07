using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RectLevelElement : LevelElement {

	protected Vector2Int size;
	public Vector2Int Size {
		get {
			return this.size;
		}
	}

	public override ISet<ILevelTile> GetTiles() {
		return LevelController.instance.GetTilesInRectangle(Pos, Size);
	}

	public override void InitPos(Vector2Int pos, int sizeX = 1, int sizeY = 1) {
		this.size = new Vector2Int(sizeX, sizeY);
		base.InitPos(pos, size.x, size.y);
	}
}
