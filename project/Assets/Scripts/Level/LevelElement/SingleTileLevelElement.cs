using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleTileLevelElement : LevelElement {
	public override ISet<ILevelTile> GetTiles() {
		ILevelTile tile = LevelController.instance.GetTileAtWorldPos(Pos);
		ISet<ILevelTile> ret = new HashSet<ILevelTile> {
			tile
		};
		return ret;
	}
}
