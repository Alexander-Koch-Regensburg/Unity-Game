using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FloorTilemap : MonoBehaviour {

	public static FloorTilemap instance;
	public TileBase floorTile;

	private Tilemap map;
    
    private void Awake() {
		instance = this;

		map = GetComponent<Tilemap>();
		if (map == null) {
			Debug.LogError("FloorTilemap has to have a Tilemap component!");
		}
    }

	public void AddFloor(Vector2Int pos) {
		map.SetTile(new Vector3Int(pos.x, pos.y, 0), floorTile);
	}

	public void RemoveFloor(Vector2Int pos) {
		map.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
	}
}
