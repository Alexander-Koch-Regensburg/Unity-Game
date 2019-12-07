using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelDataProvider : ILevelDataProvider {
	private const int sizeX = 64;
	private const int sizeY = 64;

	private const int spawnPosX = 3;
	private const int spawnPosY = 3;

	public LevelData GetLevelData() {
		LevelData levelData = new LevelData {
			size = new Vector2Int(sizeX, sizeY),
			spawnPosition = new Vector2Int(spawnPosX, spawnPosY)
		};
		return levelData;
	}

	public IList<LevelElementData> GetLevelElementsData() {
		List<LevelElementData> elementData = new List<LevelElementData>();

		elementData.AddRange(GetOuterBoundsWallData());
		elementData.AddRange(GetAdditionalWallData());
		elementData.Add(new EndZoneData(new Vector2Int(40, 50)));

		return elementData;
	}

	private List<LevelElementData> GetOuterBoundsWallData() {
		List<LevelElementData> outerWalls = new List<LevelElementData>();

		// Lower wall
		for (int x = 0; x < sizeX; ++x) {
			WallData data = new WallData(new Vector2Int(x, 0));
			outerWalls.Add(data);
		}

		// Left wall
		for (int y = 1; y < sizeY; ++y) {
			WallData data = new WallData(new Vector2Int(0, y));
			outerWalls.Add(data);
		}

		// Upper wall
		for (int x = 1; x < sizeX; ++x) {
			WallData data = new WallData(new Vector2Int(x, sizeY - 1));
			outerWalls.Add(data);
		}

		// right wall
		for (int y = 1; y < sizeY - 1; ++y) {
			WallData data = new WallData(new Vector2Int(sizeX - 1, y));
			outerWalls.Add(data);
		}

		return outerWalls;
	}

	private List<LevelElementData> GetAdditionalWallData() {
		List<LevelElementData> walls = new List<LevelElementData>();

		for (int x = 16; x < 19; ++x) {
			for (int y = sizeY - 2; y > 40; --y) {
				WallData data = new WallData(new Vector2Int(x, y));
				walls.Add(data);
			}
		}

		for (int x = 19; x < 36; ++x) {
			WallData data = new WallData(new Vector2Int(x, 41));
			walls.Add(data);
		}

		for (int y = sizeY - 16; y > 41; --y) {
			WallData data = new WallData(new Vector2Int(30, y));
			walls.Add(data);
		}

		for (int y = 40; y > 28; --y) {
			WallData data = new WallData(new Vector2Int(30, y));
			walls.Add(data);
		}

		for (int x = 31; x < 48; ++x) {
			WallData data = new WallData(new Vector2Int(x, 29));
			walls.Add(data);
		}

		for (int y = 28; y > 12; --y) {
			WallData data = new WallData(new Vector2Int(38, y));
			walls.Add(data);
		}

		DoorData door1 = new DoorData(new Vector2Int(38, 11), ElementRotation.RIGHT);
		walls.Add(door1);

		for (int x = 37; x < 40; ++x) {
			WallData data = new WallData(new Vector2Int(x, 10));
			walls.Add(data);
		}

		// Room bottom left
		for (int x = 1; x < 16; ++x) {
			WallData data = new WallData(new Vector2Int(x, 16));
			walls.Add(data);
		}

		for (int x = 14; x < 16; ++x) {
			WallData data = new WallData(new Vector2Int(x, 15));
			walls.Add(data);
		}

		DoorData door2 = new DoorData(new Vector2Int(16, 16), ElementRotation.UP);
		walls.Add(door2);

		for (int x = 18; x < 25; ++x) {
			WallData data = new WallData(new Vector2Int(x, 16));
			walls.Add(data);
		}

		for (int x = 23; x < 27; ++x) {
			WallData data = new WallData(new Vector2Int(x, 17));
			walls.Add(data);
		}

		for (int y = 17; y < 20; ++y) {
			WallData data = new WallData(new Vector2Int(24, y));
			walls.Add(data);
		}

		for (int y = 1; y < 16; ++y) {
			WallData data = new WallData(new Vector2Int(24, y));
			walls.Add(data);
		}

		for (int x = 32; x < 44; x += 2) {
			WallData data = new WallData(new Vector2Int(x, 8));
			walls.Add(data);
		}

		for (int x = 48; x < 53; ++x) {
			WallData data = new WallData(new Vector2Int(x, 8));
			walls.Add(data);
		}

		for (int x = 48; x < 52; x += 2) {
			WallData data = new WallData(new Vector2Int(x, 7));
			walls.Add(data);
		}

		for (int x = 48; x < 53; ++x) {
			WallData data = new WallData(new Vector2Int(x, 6));
			walls.Add(data);
		}

		return walls;
	}

	public IList<ItemData> GetItemData() {
		IList<ItemData> ret = new List<ItemData>();
		ret.Add(new WeaponData(new Vector2(3.5f, 6f), 30f, WeaponType.PISTOL, 12));
		ret.Add(new WeaponData(new Vector2(6f, 21f), 30f, WeaponType.SHOTGUN, 10));
		ret.Add(new WeaponData(new Vector2(6.1f, 21.2f), 30f, WeaponType.MACHINEGUN, 300));
		ret.Add(new WeaponData(new Vector2(6.2f, 20.9f), 30f, WeaponType.PISTOL, 13));
		ret.Add(new WeaponData(new Vector2(6f, 3.5f), 70f, WeaponType.BAT, -1));
		return ret;
	}

	public IList<EnemyData> GetEnemyData() {
		IList<EnemyData> data = new List<EnemyData>();
		IList<Vector2> patrolPoints1 = new List<Vector2>();
		patrolPoints1.Add(new Vector2(14f, 61f));
		patrolPoints1.Add(new Vector2(2f, 61f));
		IList<Vector2> patrolPoints2 = new List<Vector2>();
		patrolPoints2.Add(new Vector2(61f, 61f));
		patrolPoints2.Add(new Vector2(61f, 3f));
		IList<Vector2> patrolPoints3 = new List<Vector2>();
		patrolPoints3.Add(new Vector2(2f, 18f));
		//patrolPoints3.Add(new Vector2(21f, 18f));
		//patrolPoints3.Add(new Vector2(21f, 38f));
		patrolPoints3.Add(new Vector2(2f, 38f));
		IList<Vector2> patrolPoints4 = new List<Vector2>();
		patrolPoints4.Add(new Vector2(60f, 3f));
		patrolPoints4.Add(new Vector2(60f, 61f));
		data.Add(new EnemyData(EnemyType.BASIC, patrolPoints1, WeaponType.MACHINEGUN, 20, patrolPoints1[0]));
        data.Add(new EnemyData(EnemyType.BASIC, patrolPoints2, WeaponType.PISTOL, 25, patrolPoints2[0]));
        data.Add(new EnemyData(EnemyType.EASY, patrolPoints3, WeaponType.BAT, -1, patrolPoints3[0]));
		data.Add(new EnemyData(EnemyType.HARD, patrolPoints4, WeaponType.MACHINEGUN, 20, patrolPoints4[0]));
		return data;
	}
}
