using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ILevelElement {

	public Vector2Int Pos {
		get;
	}

    public Floor() {

	}

	public ISet<ILevelTile> GetTiles() {
		ILevelTile tile = LevelController.instance.GetTileAtWorldPos(Pos);
		ISet<ILevelTile> ret = new HashSet<ILevelTile> {
			tile
		};
		return ret;
	}

	public void InitPos(Vector2Int pos, int sizeX = 1, int sizeY = 1) {
		if (pos.x < 0 || pos.y < 0) {
			return;
		}
		if (sizeX != 1 || sizeY != 1) {
			Debug.LogWarning("Floor has to be of size 1x1");
			return;
		}
		ISet<ILevelTile> tiles = GetTiles();
		foreach (ILevelTile tile in tiles) {
			tile.AddLevelElement(this);
		}

		FloorTilemap.instance.AddFloor(pos);
	}

	public bool IsSolid() {
        return false; 
    }
}
