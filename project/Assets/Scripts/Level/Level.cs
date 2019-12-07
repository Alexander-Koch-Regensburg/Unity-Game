using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {
	private ILevelTile[,] tiles;
    private Vector2 spawnPosition;

	//public Vector2 SpawnPosition {
	//	get;
	//}

    public Vector2 getSpawnPosition() {
        return spawnPosition;
    }

    public void SetSpawnPosition(Vector2 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

	public Level(Vector2Int size, Vector2 spawnPosition) {
		this.spawnPosition = spawnPosition;
		CreateTiles(size);
	}

	private void CreateTiles(Vector2Int size) {
        if(size.x <= 0 || size.y <= 0) {
            return; 
        }

		tiles = new ILevelTile[size.x, size.y];
		for (int i = 0; i < size.x; ++i) {
			for (int j = 0; j < size.y; ++j) {
                ILevelTile tile = new LevelTile(new Vector2Int(i, j));
                tiles[i, j] = tile;
			}
		}
	}

	public Vector2Int GetLevelSize() {
		return new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));
	}

	public ILevelTile GetTileAtPos(Vector2Int pos) {
		if (pos.x < 0) {
			return null;
		}
		if (pos.y < 0) {
			return null;
		}
		if (pos.x >= tiles.GetLength(0)) {
			return null;
		}
		if (pos.y >= tiles.GetLength(1)) {
			return null;
		}
        ILevelTile ret = tiles[pos.x, pos.y];
		return ret;
	}

	public ILevelTile GetTileAtPos(Vector2 pos) {
		int x = (int) pos.x;
		int y = (int) pos.y;
		ILevelTile ret = GetTileAtPos(new Vector2Int(x, y));
		return ret;
	}

	public ISet<ILevelTile> GetTilesInRectangle(Vector2Int pos, Vector2Int size) {
		if (pos.x < 0) {
			return null;
		}
		if (pos.y < 0) {
			return null;
		}
		if (size.x < 1 || size.y < 1) {
			return null;
		}
		if (pos.x + size.x > tiles.GetLength(0)) {
			return null;
		}
		if (pos.y + size.y > tiles.GetLength(1)) {
			return null;
		}

		ISet<ILevelTile> ret = new HashSet<ILevelTile>();
		for (int i = pos.x; i < (pos.x + size.x); ++i) {
			for (int j = pos.y; j < (pos.y + size.y); ++j) {
				ret.Add(tiles[i, j]);
			}
		}
		return ret;
	}

    public ILevelTile[,] GetTiles()
    {
        return tiles;
    }
}
