using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class LevelDeserializingTests {
    private const string MAIN_SCENE_NAME = "MainScene";

    [SetUp]
    public void Setup() {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    [UnityTest]
    public IEnumerator DeserializeLevel_PlayerHasNoWeapon_CorrectlyDeserializeLevel() {
        yield return null;

        // Arrange.
        string actualLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasNoWeapon_DeserializeLevel.json");
        JsonLevelDeserializer jsonLevelDeserializer = new JsonLevelDeserializer(actualLevelPath);

        Vector2Int expectedLevelSize = new Vector2Int(64, 64);
        Vector2 expectedSpawnPosition = new Vector2((float)2.5, (float)2.5);
        Level dummyLevel = new Level(expectedLevelSize, expectedSpawnPosition);
        GameObject emptyGameObject = new GameObject();

        // 252 Elements for the outer bounds walls, 5 additional wall datas.
        int expectedNumberOfWallElements = 257;
        int expectedNumberOfDoorElements = 5;
        int expectedNumberOfEndZoneElements = 1;
        int expectedNumberOfWeaponElements = 4;
        int expectedNumberOfEnemyElements = 3;

        // Define the coordinates that should be available for the wall, door and EndZone data. 
        IList<Vector2Int> expectedWallDataCoordinates = new List<Vector2Int> {
            new Vector2Int(5, 0),
            new Vector2Int(0, 5),
            new Vector2Int(2, 2),
            new Vector2Int(8, 1),
            new Vector2Int(4, 4)
        };

        IList<Vector2Int> expectedDoorDataCoordinates = new List<Vector2Int> {
            new Vector2Int(7, 7),
            new Vector2Int(6, 6),
            new Vector2Int(1, 3),
            new Vector2Int(5, 2),
            new Vector2Int(1, 2)
        };

        IList<ElementRotation> expectedDoorRotations = new List<ElementRotation> {
            ElementRotation.UP,
            ElementRotation.UP,
            ElementRotation.UP,
            ElementRotation.RIGHT,
            ElementRotation.UP
        };

        IList<Vector2> expectedWeaponDataCoordinates = new List<Vector2> {
            new Vector2(3.5f, 6f),
            new Vector2(6f, 21f),
            new Vector2(6.1f, 21.2f),
            new Vector2(6.2f, 20.9f)
        };

        IList<float> expectedWeaponDataRotations = new List<float> {
            30f,
            30f,
            30f,
            30f
        };

        IList<WeaponType> expectedWeaponDataTypes = new List<WeaponType> {
            WeaponType.PISTOL,
            WeaponType.PISTOL,
            WeaponType.MACHINEGUN,
            WeaponType.PISTOL
        };

        IList<int> expectedWeaponDataAmounts = new List<int> {
            -1,
            10,
            300,
            13
        };

        IList<Vector2Int> expectedEndZoneDataCoordinates = new List<Vector2Int> {
            new Vector2Int(40, 50)
        };

        // Create some dummy enemy data objects. 
        IList<EnemyData> expectedEnemyDataObjects = new List<EnemyData>();
        IList<Vector2> patrolPoints1 = new List<Vector2> {
            new Vector2(14f, 61f),
            new Vector2(2f, 61f)
        };
        IList<Vector2> patrolPoints2 = new List<Vector2> {
            new Vector2(61f, 61f),
            new Vector2(61f, 3f)
        };
        IList<Vector2> patrolPoints3 = new List<Vector2> {
            new Vector2(2f, 18f),
            new Vector2(21f, 18f),
            new Vector2(21f, 38f),
            new Vector2(2f, 38f)
        };
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints1, WeaponType.MACHINEGUN, 20, patrolPoints1[0]));
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints2, WeaponType.PISTOL, 25, patrolPoints2[0]));
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints3, WeaponType.PISTOL, 20, patrolPoints3[0]));

        // Define expected player weapon type and amount.
        PlayerData expectedPlayerData = new PlayerData(WeaponType.INVALID, 0);


        // Act.
        JsonDataProvider levelDataProvider = (JsonDataProvider)jsonLevelDeserializer.DeserializeLevel();

        // Get levelData and levelElementData from deserialized levelDataProvider.
        LevelData levelData = levelDataProvider.GetLevelData();
        IList<LevelElementData> levelElementData = levelDataProvider.GetLevelElementsData();
        IList<ItemData> levelItemData = levelDataProvider.GetItemData();
        IList<EnemyData> actualEnemyData = levelDataProvider.GetEnemyData();
        PlayerData actualPlayerData = levelDataProvider.GetPlayerData();

        IList<ILevelElement> walls = new List<ILevelElement>();
        IList<ILevelElement> doors = new List<ILevelElement>();
        IList<ILevelElement> endZones = new List<ILevelElement>();
        IList<IWeapon> weapons = new List<IWeapon>();
        IList<Enemy> enemies = new List<Enemy>();
        IList<Vector2Int> actualWallDataCoordinates = new List<Vector2Int>();
        IList<Vector2Int> actualDoorDataCoordinates = new List<Vector2Int>();
        IList<ElementRotation> actualDoorDataRotations = new List<ElementRotation>();
        IList<Vector2Int> actualEndZoneDataCoordinates = new List<Vector2Int>();
        IList<Vector2> actualWeaponDataCoordinates = new List<Vector2>();
        IList<float> actualWeaponDataRotations = new List<float>();
        IList<WeaponType> actualWeaponDataTypes = new List<WeaponType>();
        IList<int> actualWeaponDataAmounts = new List<int>();

        //Instantiate levelElements from levelElementData objects.
        foreach (LevelElementData elementData in levelElementData) {
            if (elementData is WallData) {
                Wall wallLevelElem = (Wall)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                walls.Add(wallLevelElem);
                actualWallDataCoordinates.Add(elementData.pos);
            }
            else if (elementData is DoorData) {
                Door doorLevelElem = (Door)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                doors.Add(doorLevelElem);
                actualDoorDataCoordinates.Add(elementData.pos);
                actualDoorDataRotations.Add(elementData.rotation);
            }
            else if (elementData is EndZoneData) {
                EndZone endZone = (EndZone)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                endZones.Add(endZone);
                actualEndZoneDataCoordinates.Add(elementData.pos);
            }
        }

        // Add the actual coordinates of the weapon data items to the corresponding list. 
        foreach(ItemData itemData in levelItemData) {
            if (itemData is WeaponData weaponData) {
                Weapon weaponLevelItem = ItemFactory.InstantiateWeapon(weaponData, emptyGameObject.transform);
                weapons.Add(weaponLevelItem);

                actualWeaponDataCoordinates.Add(weaponData.pos);
                actualWeaponDataRotations.Add(weaponData.rotation);
                actualWeaponDataTypes.Add(weaponData.type);
                actualWeaponDataAmounts.Add(weaponData.ammo);
            }
        }

        enemies = PersonController.instance.SpawnEnemies(actualEnemyData);

        // Assert. 
        Assert.AreEqual(expectedLevelSize, levelData.size);
        Assert.AreEqual(expectedSpawnPosition, levelData.spawnPosition);
        Assert.AreEqual(expectedNumberOfWallElements, walls.Count);
        Assert.AreEqual(expectedNumberOfDoorElements, doors.Count);
        Assert.AreEqual(expectedNumberOfEndZoneElements, endZones.Count);
        Assert.AreEqual(expectedNumberOfWeaponElements, weapons.Count);
        Assert.AreEqual(expectedNumberOfEnemyElements, enemies.Count);
        Assert.NotNull(Player.instance);
        Assert.AreEqual(expectedPlayerData.weapon, actualPlayerData.weapon);
        Assert.AreEqual(expectedPlayerData.ammo, actualPlayerData.ammo);

        // Check if the actual wall data elements contain the expected coordinates. 
        foreach (Vector2Int expectedContainedCoordinate in expectedWallDataCoordinates) {
            Assert.IsTrue(actualWallDataCoordinates.Contains(expectedContainedCoordinate));
        }

        // Check if the actual door data elements contain the expected coordinates.
        foreach (Vector2Int expectedContainedCoordinate in expectedDoorDataCoordinates) {
            Assert.IsTrue(actualDoorDataCoordinates.Contains(expectedContainedCoordinate));
        }

        // Check if the actual door data elements contain the expected rotations.
        for (int i = 0; i < expectedDoorRotations.Count; i++) {
            Assert.AreEqual(expectedDoorRotations[i], actualDoorDataRotations[i]);
        }

        // Check if the bounding walls are available. 
        // Lower wall
        for (int x = 0; x < expectedLevelSize.x; ++x) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(x, 0)));
        }
        // Left wall
        for (int y = 1; y < expectedLevelSize.y; ++y) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(0, y)));
        }
        // Upper wall
        for (int x = 1; x < expectedLevelSize.x; ++x) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(x, expectedLevelSize.y - 1)));
        }
        // right wall
        for (int y = 1; y < expectedLevelSize.y - 1; ++y) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(expectedLevelSize.x - 1, y)));
        }

        // Assure that all expected end zones are loaded at the correct coordinates. 
        for (int i = 0; i < expectedEndZoneDataCoordinates.Count; i++) {
            Assert.AreEqual(expectedEndZoneDataCoordinates[i], actualEndZoneDataCoordinates[i]);
        }

        // Check if all weapon data are loaded correctly. 
        for (int i = 0; i < expectedWeaponDataCoordinates.Count; i++) {
            Assert.AreEqual(expectedWeaponDataCoordinates[i], actualWeaponDataCoordinates[i]);
        }
        for (int i = 0; i < expectedWeaponDataRotations.Count; i++) {
            Assert.AreEqual(expectedWeaponDataRotations[i], actualWeaponDataRotations[i]);
        }
        for (int i = 0; i < expectedWeaponDataTypes.Count; i++) {
            Assert.AreEqual(expectedWeaponDataTypes[i], actualWeaponDataTypes[i]);
        }
        for (int i = 0; i < expectedWeaponDataAmounts.Count; i++) {
            Assert.AreEqual(expectedWeaponDataAmounts[i], actualWeaponDataAmounts[i]);
        }

        // Check if all enemy data objects contain the correct data. 
        for (int i = 0; i < expectedEnemyDataObjects.Count; i++) {
            Assert.AreEqual(expectedEnemyDataObjects[i].type, actualEnemyData[i].type);
            Assert.AreEqual(expectedEnemyDataObjects[i].patrolPoints, actualEnemyData[i].patrolPoints);
            Assert.AreEqual(expectedEnemyDataObjects[i].weapon, actualEnemyData[i].weapon);
            Assert.AreEqual(expectedEnemyDataObjects[i].ammo, actualEnemyData[i].ammo);
        }
    }

    [UnityTest]
    public IEnumerator DeserializeLevel_PlayerHasSomeWeapon_CorrectlyDeserializeLevel() {
        yield return null;

        // Arrange.
        string actualLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasSomeWeapon_DeserializeLevel.json");
        JsonLevelDeserializer jsonLevelDeserializer = new JsonLevelDeserializer(actualLevelPath);

        Vector2Int expectedLevelSize = new Vector2Int(64, 64);
        Vector2 expectedSpawnPosition = new Vector2((float)2.5, (float)2.5);
        Level dummyLevel = new Level(expectedLevelSize, expectedSpawnPosition);
        GameObject emptyGameObject = new GameObject();

        // 252 Elements for the outer bounds walls, 5 additional wall datas.
        int expectedNumberOfWallElements = 257;
        int expectedNumberOfDoorElements = 5;
        int expectedNumberOfEndZoneElements = 1;
        int expectedNumberOfWeaponElements = 4;
        int expectedNumberOfEnemyElements = 3;

        // Define the coordinates that should be available for the wall, door and EndZone data. 
        IList<Vector2Int> expectedWallDataCoordinates = new List<Vector2Int> {
            new Vector2Int(5, 0),
            new Vector2Int(0, 5),
            new Vector2Int(2, 2),
            new Vector2Int(8, 1),
            new Vector2Int(4, 4)
        };

        IList<Vector2Int> expectedDoorDataCoordinates = new List<Vector2Int> {
            new Vector2Int(7, 7),
            new Vector2Int(6, 6),
            new Vector2Int(1, 3),
            new Vector2Int(5, 2),
            new Vector2Int(1, 2)
        };

        IList<ElementRotation> expectedDoorRotations = new List<ElementRotation> {
            ElementRotation.UP,
            ElementRotation.UP,
            ElementRotation.UP,
            ElementRotation.RIGHT,
            ElementRotation.UP
        };

        IList<Vector2> expectedWeaponDataCoordinates = new List<Vector2> {
            new Vector2(3.5f, 6f),
            new Vector2(6f, 21f),
            new Vector2(6.1f, 21.2f),
            new Vector2(6.2f, 20.9f)
        };

        IList<float> expectedWeaponDataRotations = new List<float> {
            30f,
            30f,
            30f,
            30f
        };

        IList<WeaponType> expectedWeaponDataTypes = new List<WeaponType> {
            WeaponType.PISTOL,
            WeaponType.PISTOL,
            WeaponType.MACHINEGUN,
            WeaponType.PISTOL
        };

        IList<int> expectedWeaponDataAmounts = new List<int> {
            -1,
            10,
            300,
            13
        };

        IList<Vector2Int> expectedEndZoneDataCoordinates = new List<Vector2Int> {
            new Vector2Int(40, 50)
        };

        // Create some dummy enemy data objects. 
        IList<EnemyData> expectedEnemyDataObjects = new List<EnemyData>();
        IList<Vector2> patrolPoints1 = new List<Vector2> {
            new Vector2(14f, 61f),
            new Vector2(2f, 61f)
        };
        IList<Vector2> patrolPoints2 = new List<Vector2> {
            new Vector2(61f, 61f),
            new Vector2(61f, 3f)
        };
        IList<Vector2> patrolPoints3 = new List<Vector2> {
            new Vector2(2f, 18f),
            new Vector2(21f, 18f),
            new Vector2(21f, 38f),
            new Vector2(2f, 38f)
        };
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints1, WeaponType.MACHINEGUN, 20, patrolPoints1[0]));
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints2, WeaponType.PISTOL, 25, patrolPoints2[0]));
        expectedEnemyDataObjects.Add(new EnemyData(EnemyType.BASIC, patrolPoints3, WeaponType.PISTOL, 20, patrolPoints3[0]));

        // Define expected player weapon type and amount.
        PlayerData expectedPlayerData = new PlayerData(WeaponType.PISTOL, 10);


        // Act.
        JsonDataProvider levelDataProvider = (JsonDataProvider)jsonLevelDeserializer.DeserializeLevel();

        // Get levelData and levelElementData from deserialized levelDataProvider.
        LevelData levelData = levelDataProvider.GetLevelData();
        IList<LevelElementData> levelElementData = levelDataProvider.GetLevelElementsData();
        IList<ItemData> levelItemData = levelDataProvider.GetItemData();
        IList<EnemyData> actualEnemyData = levelDataProvider.GetEnemyData();
        PlayerData actualPlayerData = levelDataProvider.GetPlayerData();

        IList<ILevelElement> walls = new List<ILevelElement>();
        IList<ILevelElement> doors = new List<ILevelElement>();
        IList<ILevelElement> endZones = new List<ILevelElement>();
        IList<IWeapon> weapons = new List<IWeapon>();
        IList<Enemy> enemies = new List<Enemy>();
        IList<Vector2Int> actualWallDataCoordinates = new List<Vector2Int>();
        IList<Vector2Int> actualDoorDataCoordinates = new List<Vector2Int>();
        IList<ElementRotation> actualDoorDataRotations = new List<ElementRotation>();
        IList<Vector2Int> actualEndZoneDataCoordinates = new List<Vector2Int>();
        IList<Vector2> actualWeaponDataCoordinates = new List<Vector2>();
        IList<float> actualWeaponDataRotations = new List<float>();
        IList<WeaponType> actualWeaponDataTypes = new List<WeaponType>();
        IList<int> actualWeaponDataAmounts = new List<int>();

        //Instantiate levelElements from levelElementData objects.
        foreach (LevelElementData elementData in levelElementData) {
            if (elementData is WallData) {
                Wall wallLevelElem = (Wall)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                walls.Add(wallLevelElem);
                actualWallDataCoordinates.Add(elementData.pos);
            }
            else if (elementData is DoorData) {
                Door doorLevelElem = (Door)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                doors.Add(doorLevelElem);
                actualDoorDataCoordinates.Add(elementData.pos);
                actualDoorDataRotations.Add(elementData.rotation);
            }
            else if (elementData is EndZoneData) {
                EndZone endZone = (EndZone)LevelElementFactory.InstantiateLevelElement(elementData, emptyGameObject.transform);
                endZones.Add(endZone);
                actualEndZoneDataCoordinates.Add(elementData.pos);
            }
        }

        // Add the actual coordinates of the weapon data items to the corresponding list. 
        foreach (ItemData itemData in levelItemData) {
            if (itemData is WeaponData weaponData) {
                Weapon weaponLevelItem = ItemFactory.InstantiateWeapon(weaponData, emptyGameObject.transform);
                weapons.Add(weaponLevelItem);

                actualWeaponDataCoordinates.Add(weaponData.pos);
                actualWeaponDataRotations.Add(weaponData.rotation);
                actualWeaponDataTypes.Add(weaponData.type);
                actualWeaponDataAmounts.Add(weaponData.ammo);
            }
        }

        enemies = PersonController.instance.SpawnEnemies(actualEnemyData);

        // Assert. 
        Assert.AreEqual(expectedLevelSize, levelData.size);
        Assert.AreEqual(expectedSpawnPosition, levelData.spawnPosition);
        Assert.AreEqual(expectedNumberOfWallElements, walls.Count);
        Assert.AreEqual(expectedNumberOfDoorElements, doors.Count);
        Assert.AreEqual(expectedNumberOfEndZoneElements, endZones.Count);
        Assert.AreEqual(expectedNumberOfWeaponElements, weapons.Count);
        Assert.AreEqual(expectedNumberOfEnemyElements, enemies.Count);
        Assert.NotNull(Player.instance);
        Assert.AreEqual(expectedPlayerData.weapon, actualPlayerData.weapon);
        Assert.AreEqual(expectedPlayerData.ammo, actualPlayerData.ammo);

        // Check if the actual wall data elements contain the expected coordinates. 
        foreach (Vector2Int expectedContainedCoordinate in expectedWallDataCoordinates) {
            Assert.IsTrue(actualWallDataCoordinates.Contains(expectedContainedCoordinate));
        }

        // Check if the actual door data elements contain the expected coordinates.
        foreach (Vector2Int expectedContainedCoordinate in expectedDoorDataCoordinates) {
            Assert.IsTrue(actualDoorDataCoordinates.Contains(expectedContainedCoordinate));
        }

        // Check if the actual door data elements contain the expected rotations.
        for (int i = 0; i < expectedDoorRotations.Count; i++) {
            Assert.AreEqual(expectedDoorRotations[i], actualDoorDataRotations[i]);
        }

        // Check if the bounding walls are available. 
        // Lower wall
        for (int x = 0; x < expectedLevelSize.x; ++x) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(x, 0)));
        }
        // Left wall
        for (int y = 1; y < expectedLevelSize.y; ++y) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(0, y)));
        }
        // Upper wall
        for (int x = 1; x < expectedLevelSize.x; ++x) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(x, expectedLevelSize.y - 1)));
        }
        // right wall
        for (int y = 1; y < expectedLevelSize.y - 1; ++y) {
            Assert.True(actualWallDataCoordinates.Contains(new Vector2Int(expectedLevelSize.x - 1, y)));
        }

        // Assure that all expected end zones are loaded at the correct coordinates. 
        for (int i = 0; i < expectedEndZoneDataCoordinates.Count; i++) {
            Assert.AreEqual(expectedEndZoneDataCoordinates[i], actualEndZoneDataCoordinates[i]);
        }

        // Check if all weapon data are loaded correctly. 
        for (int i = 0; i < expectedWeaponDataCoordinates.Count; i++) {
            Assert.AreEqual(expectedWeaponDataCoordinates[i], actualWeaponDataCoordinates[i]);
        }
        for (int i = 0; i < expectedWeaponDataRotations.Count; i++) {
            Assert.AreEqual(expectedWeaponDataRotations[i], actualWeaponDataRotations[i]);
        }
        for (int i = 0; i < expectedWeaponDataTypes.Count; i++) {
            Assert.AreEqual(expectedWeaponDataTypes[i], actualWeaponDataTypes[i]);
        }
        for (int i = 0; i < expectedWeaponDataAmounts.Count; i++) {
            Assert.AreEqual(expectedWeaponDataAmounts[i], actualWeaponDataAmounts[i]);
        }

        // Check if all enemy data objects contain the correct data. 
        for (int i = 0; i < expectedEnemyDataObjects.Count; i++) {
            Assert.AreEqual(expectedEnemyDataObjects[i].type, actualEnemyData[i].type);
            Assert.AreEqual(expectedEnemyDataObjects[i].patrolPoints, actualEnemyData[i].patrolPoints);
            Assert.AreEqual(expectedEnemyDataObjects[i].weapon, actualEnemyData[i].weapon);
            Assert.AreEqual(expectedEnemyDataObjects[i].ammo, actualEnemyData[i].ammo);
        }
    }
}