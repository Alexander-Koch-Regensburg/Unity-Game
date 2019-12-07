using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileMapLevelElement : SingleTileLevelElement {

	/// <summary>
	/// The <c>Tile</c> used for this <c>TileMapLevelElement</c>
	/// </summary>
	public TileBase tile;

	public void AddToTilemap() {
		Tilemap map = TilemapController.instance.GetTilemapForLevelElement(this);
		if (map == null) {
			Debug.LogError("Cannot add TileMapLevelElement to Tilemap: No Tilemap assigned for this type of LevelElement!");
			return;
		}
		Vector3Int tilemapPos = new Vector3Int(Pos.x, Pos.y, 0);
		map.SetTile(tilemapPos, tile);
    }

	public void RemoveFromTilemap() {
		Tilemap map = TilemapController.instance.GetTilemapForLevelElement(this);
		if (map == null) {
			return;
		}
		Vector3Int tilemapPos = new Vector3Int(Pos.x, Pos.y, 0);
		map.SetTile(tilemapPos, null);
    }

	public virtual void OnDestroy() {
		if (MainMenuPlayerPreferences.InLevelCreationMode == false) {
			RemoveFromTilemap();
		}
	}
}
