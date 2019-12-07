using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour {
	public static TilemapController instance;

	public Tilemap floorTilemap;
	public Tilemap wallTilemap;

	private void Awake() {
		instance = this;
	}

	public Tilemap GetTilemapForLevelElement(TileMapLevelElement element) {
		if (element is Wall) {
			return wallTilemap;
		}
		return null;
	}

    public void ClearTilemaps()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
