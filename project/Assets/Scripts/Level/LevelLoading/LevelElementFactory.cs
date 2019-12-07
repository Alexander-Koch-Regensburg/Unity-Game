using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElementFactory {

	public static ILevelElement InstantiateLevelElement(LevelElementData data, Transform parent) {
		if (data is FloorData) {
			return InstantiateFloor((FloorData)data, parent);
		} else if (data is WallData) {
			return InstantiateWall((WallData)data, parent);
		} else if (data is DoorData) {
			return InstantiateDoor((DoorData)data, parent);
		} else if (data is EndZoneData) {
			return InstantiateEndZone((EndZoneData) data, parent);
		}
		return null;
	}

	private static ILevelElement InstantiateFloor(FloorData data, Transform parent) {
		Floor floor = new Floor();
		floor.InitPos(data.pos);
		return floor;
	}

	private static ILevelElement InstantiateWall(WallData data, Transform parent) {
		GameObject prefab = LevelElementPrefabTable.instance.GetPrefab(data);
		if (prefab == null) {
			Debug.LogError("Could not create wall: Prefab not found!");
		}
		GameObject gameObject = Object.Instantiate(prefab, parent);
		Wall wall = gameObject.GetComponent<Wall>();
		if (wall == null) {
			Debug.LogError("Prefab for Wall is faulty: Has no Wall component!");
			return null;
		}

		wall.InitPos(data.pos);
		wall.AddToTilemap();
		
		return wall;
	}

	private static ILevelElement InstantiateDoor(DoorData data, Transform parent) {
		GameObject prefab = LevelElementPrefabTable.instance.GetPrefab(data);
		if (prefab == null) {
			Debug.LogError("Could not create door: Prefab not found!");
		}
		GameObject gameObject = Object.Instantiate(prefab, parent);
		Door door = gameObject.GetComponent<Door>();
		if (door == null) {
			Debug.LogError("Prefab for Door is faulty: Has no Door component!");
			return null;
		}
		if (data.rotation == ElementRotation.RIGHT || data.rotation == ElementRotation.LEFT) {
			data.size = VectorUtil.SwapXY(data.size);
		} 
		door.InitPos(data.pos, data.size.x, data.size.y);

		return door;
	}

	private static ILevelElement InstantiateEndZone(EndZoneData data, Transform parent) {
		GameObject prefab = LevelElementPrefabTable.instance.GetPrefab(data);
		if (prefab == null) {
			Debug.LogError("Could not create end-zone: Prefab not found!");
		}
		GameObject gameObject = Object.Instantiate(prefab, parent);
		EndZone endZone = gameObject.GetComponent<EndZone>();
		if (endZone == null) {
			Debug.LogError("Prefab for end-zone is faulty: Has no EndZone component!");
			return null;
		}

		endZone.InitPos(data.pos, data.size.x, data.size.y);

		return endZone;
	}
}
