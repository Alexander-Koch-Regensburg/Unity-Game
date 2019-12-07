using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows for obtaining the prefabs for given <c>LevelElement</c>s
/// </summary>
public class LevelElementPrefabTable : MonoBehaviour {

	public static LevelElementPrefabTable instance;

	public GameObject wallPrefab;
	public GameObject door2x1HorizontalPrefab;
	public GameObject door2x1VerticalPrefab;
	public GameObject endZonePrefab;

    void Awake() {
		instance = this;
    }

	public GameObject GetPrefab(LevelElementData data) {
		if (data is WallData) {
			return wallPrefab;
		} else if (data is DoorData) {
			return GetDoor((DoorData)data);
		} else if (data is EndZoneData) {
			return endZonePrefab;
		}
		return null;
	}

	private GameObject GetDoor(DoorData data) {
		if (data.rotation == ElementRotation.UP || data.rotation == ElementRotation.DOWN) {
			return door2x1HorizontalPrefab;
		} else {
			return door2x1VerticalPrefab;
		}
	}
}
