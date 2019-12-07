using UnityEngine;
using System.Collections.Generic;

public class JsonDataProvider : ILevelDataProvider {

    private readonly Vector2Int levelSize;

    private readonly Vector2 spawnPos;

    private readonly IList<JsonWallStructure> wallElements;
    private readonly IList<JsonDoorStructure> doorElements;
	private readonly IList<JsonEndZoneStructure> endZoneElements;
    private readonly IList<JsonWeaponStructure> weaponElements;
    private readonly IList<JsonEnemyStructure> enemyElements;
    private readonly JsonPlayerStructure playerStructure;

    public JsonDataProvider(JsonLevelStructure jsonLevelStructure, JsonPlayerStructure playerStructure ,IList<JsonWallStructure> wallElements, IList<JsonDoorStructure> doorElements, IList<JsonEndZoneStructure> endZoneElements, IList<JsonWeaponStructure> weaponElements, IList<JsonEnemyStructure> enemyElements) {
        levelSize = new Vector2Int(jsonLevelStructure.Size.x, jsonLevelStructure.Size.y);
        spawnPos = new Vector2(playerStructure.SpawnPosition.x - (float)0.5, playerStructure.SpawnPosition.y - (float)0.5);
        this.wallElements = wallElements;
        this.doorElements = doorElements;
		this.endZoneElements = endZoneElements;
        this.weaponElements = weaponElements;
        this.enemyElements = enemyElements;
        this.playerStructure = playerStructure;
    }

    public LevelData GetLevelData() {
        LevelData levelData = new LevelData {
            size = new Vector2Int(levelSize.x, levelSize.y),
            spawnPosition = new Vector2(spawnPos.x, spawnPos.y)
        };
        return levelData;
    }

    public PlayerData GetPlayerData() {
        if(playerStructure == null) {
            Debug.LogWarning("null");
        }
        WeaponType weaponType = GetWeaponTypeByString(playerStructure.WeaponType);
        return new PlayerData(weaponType, playerStructure.Amount);

    }

    public IList<LevelElementData> GetLevelElementsData() {
        List<LevelElementData> elementData = new List<LevelElementData>();

        elementData.AddRange(GetOuterBoundsWallData());
        elementData.AddRange(GetAdditionalWallData());
        elementData.AddRange(GetDoorData());
		elementData.AddRange(GetEndZoneData());

        return elementData;
    }


    private List<LevelElementData> GetOuterBoundsWallData() {
        List<LevelElementData> outerWalls = new List<LevelElementData>();

        // Lower wall
        for (int x = 0; x < levelSize.x; ++x) {
            WallData data = new WallData(new Vector2Int(x, 0));
            outerWalls.Add(data);
        }

        // Left wall
        for (int y = 1; y < levelSize.x; ++y) {
            WallData data = new WallData(new Vector2Int(0, y));
            outerWalls.Add(data);
        }

        // Upper wall
        for (int x = 1; x < levelSize.x; ++x) {
            WallData data = new WallData(new Vector2Int(x, levelSize.y - 1));
            outerWalls.Add(data);
        }

        // right wall
        for (int y = 1; y < levelSize.y - 1; ++y) {
            WallData data = new WallData(new Vector2Int(levelSize.x - 1, y));
            outerWalls.Add(data);
        }

        return outerWalls;
    }

    /// <summary>
    /// Create wall data from list.
    /// </summary>
    /// <returns>List of LevelElementData.</returns>
    private List<LevelElementData> GetAdditionalWallData() {
        List<LevelElementData> walls = new List<LevelElementData>();

        foreach (JsonWallStructure wallStructure in wallElements) {
            WallData data = new WallData(new Vector2Int(wallStructure.Position.x, wallStructure.Position.y));
            walls.Add(data);
        }

        return walls;
    }

    /// <summary>
    /// Create door data from list.
    /// </summary>
    /// <returns>List of LevelElementData.</returns>
    private List<LevelElementData> GetDoorData() {
        List<LevelElementData> doors = new List<LevelElementData>();

        foreach (JsonDoorStructure doorStructure in doorElements) {
            ElementRotation doorRotation;
            // A door can only have two states: horizontal or vertical.
            if (doorStructure.Rotation == "HORIZONTAL") {
                doorRotation = ElementRotation.RIGHT;
            } 
            else {
                doorRotation = ElementRotation.UP;
            }

            DoorData data = new DoorData(new Vector2Int(doorStructure.Position.x, doorStructure.Position.y), doorRotation);
            doors.Add(data);
        }

        return doors;
    }

	private List<LevelElementData> GetEndZoneData() {
		List<LevelElementData> endZones = new List<LevelElementData>();

		foreach (JsonEndZoneStructure endZoneStructure in endZoneElements) {
			EndZoneData data = new EndZoneData(new Vector2Int(endZoneStructure.Position.x, endZoneStructure.Position.y));
			endZones.Add(data);
		}

		return endZones;
	}

	public IList<ItemData> GetItemData() {
        IList<ItemData> itemData = new List<ItemData>();

        // Loop through all json weapon structures and create WeaponData objects from them. 
        foreach (JsonWeaponStructure weaponStructure in weaponElements) {
            Vector2 position = new Vector2(weaponStructure.Position.x, weaponStructure.Position.y);
            float rotation = weaponStructure.Rotation;

            WeaponType weaponType = GetWeaponTypeByString(weaponStructure.Type);

            int amount = weaponStructure.Ammount;

            WeaponData weaponData = new WeaponData(position, rotation, weaponType, amount);

            // Add the created WeaponData object to the more general itemData list. 
            itemData.Add(weaponData);
        }

        return itemData;
    }

    public IList<EnemyData> GetEnemyData() {
        IList<EnemyData> enemyData = new List<EnemyData>();

        // Loop through all enemy json structures and create EnemyData objects from them. 
        foreach(JsonEnemyStructure enemyStructure in enemyElements) {

            // Get the enemy type from the json enemy structure. Can be expanded in the future. 
            EnemyType enemyType;
            switch (enemyStructure.Type) {
                case "EASY":
                    enemyType = EnemyType.EASY;
                    break;
                case "HARD":
                    enemyType = EnemyType.HARD;
                    break;
                default:
                    enemyType = EnemyType.BASIC;
                    break;
            }

            IList<Vector2> patrolPoints = new List<Vector2>();
            foreach (JsonFloatPositionStructure point in enemyStructure.PatrolPoints) {
                Vector2 floatPatrolPoint = new Vector2(point.x, point.y);
                patrolPoints.Add(floatPatrolPoint);
            }

            // Get the weapon type from the json enemy structure.
            WeaponType weaponType = GetWeaponTypeByString(enemyStructure.WeaponType);

            Vector2 actualPosition = new Vector2(enemyStructure.Position.x - (float)0.5, enemyStructure.Position.y - (float)0.5);

            EnemyData tmpEnemyData = new EnemyData(enemyType, patrolPoints, weaponType, enemyStructure.Amount, actualPosition);
            enemyData.Add(tmpEnemyData);
        }

        return enemyData;
    }


    private WeaponType GetWeaponTypeByString(string stringWeaponType) {
        // Get the weapon type that matches the given string.
        WeaponType weaponType;
        switch (stringWeaponType) {
            case "PISTOL":
                weaponType = WeaponType.PISTOL;
                break;
            case "MACHINEGUN":
                weaponType = WeaponType.MACHINEGUN;
                break;
            case "SHOTGUN":
                weaponType = WeaponType.SHOTGUN;
                break;
            case "BAT":
                weaponType = WeaponType.BAT;
                break;
            default:
                weaponType = WeaponType.INVALID;
                break;
        }
        return weaponType;
    }
}