using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;

public class LevelSerializingTests {
    private const string MAIN_SCENE_NAME = "MainScene";

    [SetUp]
    public void Setup() {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    [UnityTest]
    public IEnumerator SerializeLevel_PlayerHasNoWeapon_CorrectSerializationOfDummyLevel() {
        yield return null;

        // Arrange.
        string actualLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasNoWeapon_SerializeLevel.json");
        string expectedLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasNoWeapon_SerializeLevel_ExpectedResult.json");

        ILevelSerializer jsonLevelSerializer = new JsonLevelSerializer();

        // Fake the level.
        const int levelSizeX = 64;
        const int levelSizeY = 64;
        Vector2Int spawnPos = new Vector2Int(3, 3);
        Vector2Int levelSize = new Vector2Int(levelSizeX, levelSizeY);
        Level dummyLevel = new Level(levelSize, spawnPos);

        // Fake the player.
        PlayerData playerData = new PlayerData(WeaponType.INVALID, 0);
        PersonController.instance.SpawnPlayer(playerData, spawnPos);

        // Fake the levelElements.
        IList<ILevelElement> levelElements = new List<ILevelElement>();
        GameObject emptyGameObject = new GameObject();

        // Create some dummy wall elements.
        List<LevelElementData> wallDataElements = new List<LevelElementData>();
        // Lower wall
        for (int x = 0; x < levelSizeX; ++x) {
            WallData data = new WallData(new Vector2Int(x, 0));
            wallDataElements.Add(data);
        }

        // Left wall
        for (int y = 1; y < levelSizeY; ++y) {
            WallData data = new WallData(new Vector2Int(0, y));
            wallDataElements.Add(data);
        }

        // Upper wall
        for (int x = 1; x < levelSizeX; ++x) {
            WallData data = new WallData(new Vector2Int(x, levelSizeY - 1));
            wallDataElements.Add(data);
        }

        // Right wall
        for (int y = 1; y < levelSizeY - 1; ++y) {
            WallData data = new WallData(new Vector2Int(levelSizeX - 1, y));
            wallDataElements.Add(data);
        }

        // Instantiate LevelElements from levelElementData objects. 
        foreach (LevelElementData wallData in wallDataElements) { 
            Wall wall = (Wall)LevelElementFactory.InstantiateLevelElement(wallData, emptyGameObject.transform);
            levelElements.Add(wall);
        }
        

        // Create some dummy door elements with horizontal and vertical rotation. 
        DoorData doorData = new DoorData(new Vector2Int(2, 2), ElementRotation.RIGHT);
        Door door = (Door)LevelElementFactory.InstantiateLevelElement(doorData, emptyGameObject.transform);
        levelElements.Add(door);

        doorData = new DoorData(new Vector2Int(6, 6), ElementRotation.UP);
        door = (Door)LevelElementFactory.InstantiateLevelElement(doorData, emptyGameObject.transform);
        levelElements.Add(door);

        // Create a dummy EndZone element.
        EndZoneData endZoneData = new EndZoneData(new Vector2Int(40, 50));
        EndZone endZone = (EndZone)LevelElementFactory.InstantiateLevelElement(endZoneData, emptyGameObject.transform);
        levelElements.Add(endZone);


        // Create some dummy level items.
        IList<ItemData> levelItemData = new List<ItemData> {
            new WeaponData(new Vector2(3.5f, 6f), 30f, WeaponType.PISTOL, -1),
            new WeaponData(new Vector2(6f, 21f), 30f, WeaponType.PISTOL, 10),
            new WeaponData(new Vector2(6.1f, 21.2f), 30f, WeaponType.MACHINEGUN, 300),
            new WeaponData(new Vector2(6.2f, 20.9f), 30f, WeaponType.PISTOL, 13)
        };

        List<Item> levelItems = new List<Item>();
        foreach (ItemData item in levelItemData) {
            if (item is WeaponData) {
                Weapon weaponObject = ItemFactory.InstantiateWeapon((WeaponData)item, emptyGameObject.transform);
                levelItems.Add(weaponObject);
            }
        }

        // Create some dummy enemies. 
        IList<EnemyData> enemyData = new List<EnemyData>();
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
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints1, WeaponType.MACHINEGUN, 20, patrolPoints1[0]));
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints2, WeaponType.PISTOL, 25, patrolPoints2[0]));
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints3, WeaponType.PISTOL, 20, patrolPoints3[0]));


        IList<Enemy> enemies = PersonController.instance.SpawnEnemies(enemyData);

        string expectedLevelJson = "{\"Size\":{\"x\":64,\"y\":64},\"Player\":{\"WeaponType\":null,\"Amount\":0,\"SpawnPosition\":{\"x\":3.5,\"y\":3.5}},\"LevelElements\":{\"Walls\":[{\"Position\":{\"x\":0,\"y\":0}},{\"Position\":{\"x\":1,\"y\":0}},{\"Position\":{\"x\":2,\"y\":0}},{\"Position\":{\"x\":3,\"y\":0}},{\"Position\":{\"x\":4,\"y\":0}},{\"Position\":{\"x\":5,\"y\":0}},{\"Position\":{\"x\":6,\"y\":0}},{\"Position\":{\"x\":7,\"y\":0}},{\"Position\":{\"x\":8,\"y\":0}},{\"Position\":{\"x\":9,\"y\":0}},{\"Position\":{\"x\":10,\"y\":0}},{\"Position\":{\"x\":11,\"y\":0}},{\"Position\":{\"x\":12,\"y\":0}},{\"Position\":{\"x\":13,\"y\":0}},{\"Position\":{\"x\":14,\"y\":0}},{\"Position\":{\"x\":15,\"y\":0}},{\"Position\":{\"x\":16,\"y\":0}},{\"Position\":{\"x\":17,\"y\":0}},{\"Position\":{\"x\":18,\"y\":0}},{\"Position\":{\"x\":19,\"y\":0}},{\"Position\":{\"x\":20,\"y\":0}},{\"Position\":{\"x\":21,\"y\":0}},{\"Position\":{\"x\":22,\"y\":0}},{\"Position\":{\"x\":23,\"y\":0}},{\"Position\":{\"x\":24,\"y\":0}},{\"Position\":{\"x\":25,\"y\":0}},{\"Position\":{\"x\":26,\"y\":0}},{\"Position\":{\"x\":27,\"y\":0}},{\"Position\":{\"x\":28,\"y\":0}},{\"Position\":{\"x\":29,\"y\":0}},{\"Position\":{\"x\":30,\"y\":0}},{\"Position\":{\"x\":31,\"y\":0}},{\"Position\":{\"x\":32,\"y\":0}},{\"Position\":{\"x\":33,\"y\":0}},{\"Position\":{\"x\":34,\"y\":0}},{\"Position\":{\"x\":35,\"y\":0}},{\"Position\":{\"x\":36,\"y\":0}},{\"Position\":{\"x\":37,\"y\":0}},{\"Position\":{\"x\":38,\"y\":0}},{\"Position\":{\"x\":39,\"y\":0}},{\"Position\":{\"x\":40,\"y\":0}},{\"Position\":{\"x\":41,\"y\":0}},{\"Position\":{\"x\":42,\"y\":0}},{\"Position\":{\"x\":43,\"y\":0}},{\"Position\":{\"x\":44,\"y\":0}},{\"Position\":{\"x\":45,\"y\":0}},{\"Position\":{\"x\":46,\"y\":0}},{\"Position\":{\"x\":47,\"y\":0}},{\"Position\":{\"x\":48,\"y\":0}},{\"Position\":{\"x\":49,\"y\":0}},{\"Position\":{\"x\":50,\"y\":0}},{\"Position\":{\"x\":51,\"y\":0}},{\"Position\":{\"x\":52,\"y\":0}},{\"Position\":{\"x\":53,\"y\":0}},{\"Position\":{\"x\":54,\"y\":0}},{\"Position\":{\"x\":55,\"y\":0}},{\"Position\":{\"x\":56,\"y\":0}},{\"Position\":{\"x\":57,\"y\":0}},{\"Position\":{\"x\":58,\"y\":0}},{\"Position\":{\"x\":59,\"y\":0}},{\"Position\":{\"x\":60,\"y\":0}},{\"Position\":{\"x\":61,\"y\":0}},{\"Position\":{\"x\":62,\"y\":0}},{\"Position\":{\"x\":63,\"y\":0}},{\"Position\":{\"x\":0,\"y\":1}},{\"Position\":{\"x\":0,\"y\":2}},{\"Position\":{\"x\":0,\"y\":3}},{\"Position\":{\"x\":0,\"y\":4}},{\"Position\":{\"x\":0,\"y\":5}},{\"Position\":{\"x\":0,\"y\":6}},{\"Position\":{\"x\":0,\"y\":7}},{\"Position\":{\"x\":0,\"y\":8}},{\"Position\":{\"x\":0,\"y\":9}},{\"Position\":{\"x\":0,\"y\":10}},{\"Position\":{\"x\":0,\"y\":11}},{\"Position\":{\"x\":0,\"y\":12}},{\"Position\":{\"x\":0,\"y\":13}},{\"Position\":{\"x\":0,\"y\":14}},{\"Position\":{\"x\":0,\"y\":15}},{\"Position\":{\"x\":0,\"y\":16}},{\"Position\":{\"x\":0,\"y\":17}},{\"Position\":{\"x\":0,\"y\":18}},{\"Position\":{\"x\":0,\"y\":19}},{\"Position\":{\"x\":0,\"y\":20}},{\"Position\":{\"x\":0,\"y\":21}},{\"Position\":{\"x\":0,\"y\":22}},{\"Position\":{\"x\":0,\"y\":23}},{\"Position\":{\"x\":0,\"y\":24}},{\"Position\":{\"x\":0,\"y\":25}},{\"Position\":{\"x\":0,\"y\":26}},{\"Position\":{\"x\":0,\"y\":27}},{\"Position\":{\"x\":0,\"y\":28}},{\"Position\":{\"x\":0,\"y\":29}},{\"Position\":{\"x\":0,\"y\":30}},{\"Position\":{\"x\":0,\"y\":31}},{\"Position\":{\"x\":0,\"y\":32}},{\"Position\":{\"x\":0,\"y\":33}},{\"Position\":{\"x\":0,\"y\":34}},{\"Position\":{\"x\":0,\"y\":35}},{\"Position\":{\"x\":0,\"y\":36}},{\"Position\":{\"x\":0,\"y\":37}},{\"Position\":{\"x\":0,\"y\":38}},{\"Position\":{\"x\":0,\"y\":39}},{\"Position\":{\"x\":0,\"y\":40}},{\"Position\":{\"x\":0,\"y\":41}},{\"Position\":{\"x\":0,\"y\":42}},{\"Position\":{\"x\":0,\"y\":43}},{\"Position\":{\"x\":0,\"y\":44}},{\"Position\":{\"x\":0,\"y\":45}},{\"Position\":{\"x\":0,\"y\":46}},{\"Position\":{\"x\":0,\"y\":47}},{\"Position\":{\"x\":0,\"y\":48}},{\"Position\":{\"x\":0,\"y\":49}},{\"Position\":{\"x\":0,\"y\":50}},{\"Position\":{\"x\":0,\"y\":51}},{\"Position\":{\"x\":0,\"y\":52}},{\"Position\":{\"x\":0,\"y\":53}},{\"Position\":{\"x\":0,\"y\":54}},{\"Position\":{\"x\":0,\"y\":55}},{\"Position\":{\"x\":0,\"y\":56}},{\"Position\":{\"x\":0,\"y\":57}},{\"Position\":{\"x\":0,\"y\":58}},{\"Position\":{\"x\":0,\"y\":59}},{\"Position\":{\"x\":0,\"y\":60}},{\"Position\":{\"x\":0,\"y\":61}},{\"Position\":{\"x\":0,\"y\":62}},{\"Position\":{\"x\":0,\"y\":63}},{\"Position\":{\"x\":1,\"y\":63}},{\"Position\":{\"x\":2,\"y\":63}},{\"Position\":{\"x\":3,\"y\":63}},{\"Position\":{\"x\":4,\"y\":63}},{\"Position\":{\"x\":5,\"y\":63}},{\"Position\":{\"x\":6,\"y\":63}},{\"Position\":{\"x\":7,\"y\":63}},{\"Position\":{\"x\":8,\"y\":63}},{\"Position\":{\"x\":9,\"y\":63}},{\"Position\":{\"x\":10,\"y\":63}},{\"Position\":{\"x\":11,\"y\":63}},{\"Position\":{\"x\":12,\"y\":63}},{\"Position\":{\"x\":13,\"y\":63}},{\"Position\":{\"x\":14,\"y\":63}},{\"Position\":{\"x\":15,\"y\":63}},{\"Position\":{\"x\":16,\"y\":63}},{\"Position\":{\"x\":17,\"y\":63}},{\"Position\":{\"x\":18,\"y\":63}},{\"Position\":{\"x\":19,\"y\":63}},{\"Position\":{\"x\":20,\"y\":63}},{\"Position\":{\"x\":21,\"y\":63}},{\"Position\":{\"x\":22,\"y\":63}},{\"Position\":{\"x\":23,\"y\":63}},{\"Position\":{\"x\":24,\"y\":63}},{\"Position\":{\"x\":25,\"y\":63}},{\"Position\":{\"x\":26,\"y\":63}},{\"Position\":{\"x\":27,\"y\":63}},{\"Position\":{\"x\":28,\"y\":63}},{\"Position\":{\"x\":29,\"y\":63}},{\"Position\":{\"x\":30,\"y\":63}},{\"Position\":{\"x\":31,\"y\":63}},{\"Position\":{\"x\":32,\"y\":63}},{\"Position\":{\"x\":33,\"y\":63}},{\"Position\":{\"x\":34,\"y\":63}},{\"Position\":{\"x\":35,\"y\":63}},{\"Position\":{\"x\":36,\"y\":63}},{\"Position\":{\"x\":37,\"y\":63}},{\"Position\":{\"x\":38,\"y\":63}},{\"Position\":{\"x\":39,\"y\":63}},{\"Position\":{\"x\":40,\"y\":63}},{\"Position\":{\"x\":41,\"y\":63}},{\"Position\":{\"x\":42,\"y\":63}},{\"Position\":{\"x\":43,\"y\":63}},{\"Position\":{\"x\":44,\"y\":63}},{\"Position\":{\"x\":45,\"y\":63}},{\"Position\":{\"x\":46,\"y\":63}},{\"Position\":{\"x\":47,\"y\":63}},{\"Position\":{\"x\":48,\"y\":63}},{\"Position\":{\"x\":49,\"y\":63}},{\"Position\":{\"x\":50,\"y\":63}},{\"Position\":{\"x\":51,\"y\":63}},{\"Position\":{\"x\":52,\"y\":63}},{\"Position\":{\"x\":53,\"y\":63}},{\"Position\":{\"x\":54,\"y\":63}},{\"Position\":{\"x\":55,\"y\":63}},{\"Position\":{\"x\":56,\"y\":63}},{\"Position\":{\"x\":57,\"y\":63}},{\"Position\":{\"x\":58,\"y\":63}},{\"Position\":{\"x\":59,\"y\":63}},{\"Position\":{\"x\":60,\"y\":63}},{\"Position\":{\"x\":61,\"y\":63}},{\"Position\":{\"x\":62,\"y\":63}},{\"Position\":{\"x\":63,\"y\":63}},{\"Position\":{\"x\":63,\"y\":1}},{\"Position\":{\"x\":63,\"y\":2}},{\"Position\":{\"x\":63,\"y\":3}},{\"Position\":{\"x\":63,\"y\":4}},{\"Position\":{\"x\":63,\"y\":5}},{\"Position\":{\"x\":63,\"y\":6}},{\"Position\":{\"x\":63,\"y\":7}},{\"Position\":{\"x\":63,\"y\":8}},{\"Position\":{\"x\":63,\"y\":9}},{\"Position\":{\"x\":63,\"y\":10}},{\"Position\":{\"x\":63,\"y\":11}},{\"Position\":{\"x\":63,\"y\":12}},{\"Position\":{\"x\":63,\"y\":13}},{\"Position\":{\"x\":63,\"y\":14}},{\"Position\":{\"x\":63,\"y\":15}},{\"Position\":{\"x\":63,\"y\":16}},{\"Position\":{\"x\":63,\"y\":17}},{\"Position\":{\"x\":63,\"y\":18}},{\"Position\":{\"x\":63,\"y\":19}},{\"Position\":{\"x\":63,\"y\":20}},{\"Position\":{\"x\":63,\"y\":21}},{\"Position\":{\"x\":63,\"y\":22}},{\"Position\":{\"x\":63,\"y\":23}},{\"Position\":{\"x\":63,\"y\":24}},{\"Position\":{\"x\":63,\"y\":25}},{\"Position\":{\"x\":63,\"y\":26}},{\"Position\":{\"x\":63,\"y\":27}},{\"Position\":{\"x\":63,\"y\":28}},{\"Position\":{\"x\":63,\"y\":29}},{\"Position\":{\"x\":63,\"y\":30}},{\"Position\":{\"x\":63,\"y\":31}},{\"Position\":{\"x\":63,\"y\":32}},{\"Position\":{\"x\":63,\"y\":33}},{\"Position\":{\"x\":63,\"y\":34}},{\"Position\":{\"x\":63,\"y\":35}},{\"Position\":{\"x\":63,\"y\":36}},{\"Position\":{\"x\":63,\"y\":37}},{\"Position\":{\"x\":63,\"y\":38}},{\"Position\":{\"x\":63,\"y\":39}},{\"Position\":{\"x\":63,\"y\":40}},{\"Position\":{\"x\":63,\"y\":41}},{\"Position\":{\"x\":63,\"y\":42}},{\"Position\":{\"x\":63,\"y\":43}},{\"Position\":{\"x\":63,\"y\":44}},{\"Position\":{\"x\":63,\"y\":45}},{\"Position\":{\"x\":63,\"y\":46}},{\"Position\":{\"x\":63,\"y\":47}},{\"Position\":{\"x\":63,\"y\":48}},{\"Position\":{\"x\":63,\"y\":49}},{\"Position\":{\"x\":63,\"y\":50}},{\"Position\":{\"x\":63,\"y\":51}},{\"Position\":{\"x\":63,\"y\":52}},{\"Position\":{\"x\":63,\"y\":53}},{\"Position\":{\"x\":63,\"y\":54}},{\"Position\":{\"x\":63,\"y\":55}},{\"Position\":{\"x\":63,\"y\":56}},{\"Position\":{\"x\":63,\"y\":57}},{\"Position\":{\"x\":63,\"y\":58}},{\"Position\":{\"x\":63,\"y\":59}},{\"Position\":{\"x\":63,\"y\":60}},{\"Position\":{\"x\":63,\"y\":61}},{\"Position\":{\"x\":63,\"y\":62}}],\"Doors\":[{\"Position\":{\"x\":2,\"y\":2},\"Rotation\":\"HORIZONTAL\"},{\"Position\":{\"x\":6,\"y\":6},\"Rotation\":\"VERTICAL\"}],\"EndZones\":[{\"Position\":{\"x\":40,\"y\":50}}]},\"LevelItems\":{\"Weapons\":[{\"Position\":{\"x\":3.5,\"y\":6.0},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":-1},{\"Position\":{\"x\":6.0,\"y\":21.0},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":10},{\"Position\":{\"x\":6.1,\"y\":21.2},\"Rotation\":0.0,\"Type\":\"MACHINEGUN\",\"Ammount\":300},{\"Position\":{\"x\":6.2,\"y\":20.9},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":13}]},\"Enemies\":[{\"Position\":{\"x\":14.5,\"y\":61.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":14.0,\"y\":61.0},{\"x\":2.0,\"y\":61.0}],\"WeaponType\":\"MACHINEGUN\",\"Amount\":20},{\"Position\":{\"x\":61.5,\"y\":61.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":61.0,\"y\":61.0},{\"x\":61.0,\"y\":3.0}],\"WeaponType\":\"PISTOL\",\"Amount\":25},{\"Position\":{\"x\":2.5,\"y\":18.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":2.0,\"y\":18.0},{\"x\":21.0,\"y\":18.0},{\"x\":21.0,\"y\":38.0},{\"x\":2.0,\"y\":38.0}],\"WeaponType\":\"PISTOL\",\"Amount\":20}]}";
        File.WriteAllText(expectedLevelPath, expectedLevelJson);

        // Act.
        jsonLevelSerializer.SerializeLevel(actualLevelPath, dummyLevel, levelElements, levelItems, enemies);

        // Assert. 
        Assert.IsTrue(File.Exists(actualLevelPath));
        FileAssert.AreEqual(expectedLevelPath, actualLevelPath);
        Assert.AreEqual(255, levelElements.Count);
        Assert.AreEqual(4, levelItems.Count);
        Assert.AreEqual(3, enemies.Count);
        Assert.NotNull(Player.instance);
    }

    [UnityTest]
    public IEnumerator SerializeLevel_PlayerHasSomeWeapon_CorrectSerializationOfDummyLevel() {
        yield return null;

        // Arrange.
        string actualLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasSomeWeapon_SerializeLevel.json");
        string expectedLevelPath = string.Concat(Environment.CurrentDirectory, "\\Test_PlayerHasSomeWeapon_SerializeLevel_ExpectedResult.json");

        ILevelSerializer jsonLevelSerializer = new JsonLevelSerializer();

        // Fake the level.
        const int levelSizeX = 64;
        const int levelSizeY = 64;
        Vector2Int spawnPos = new Vector2Int(3, 3);
        Vector2Int levelSize = new Vector2Int(levelSizeX, levelSizeY);
        Level dummyLevel = new Level(levelSize, spawnPos);

        // Fake the player.
        PlayerData playerData = new PlayerData(WeaponType.INVALID, 0);
        PersonController.instance.SpawnPlayer(playerData, spawnPos);

        // Fake the levelElements.
        IList<ILevelElement> levelElements = new List<ILevelElement>();
        GameObject emptyGameObject = new GameObject();

        // Create some dummy wall elements.
        List<LevelElementData> wallDataElements = new List<LevelElementData>();
        // Lower wall
        for (int x = 0; x < levelSizeX; ++x) {
            WallData data = new WallData(new Vector2Int(x, 0));
            wallDataElements.Add(data);
        }

        // Left wall
        for (int y = 1; y < levelSizeY; ++y) {
            WallData data = new WallData(new Vector2Int(0, y));
            wallDataElements.Add(data);
        }

        // Upper wall
        for (int x = 1; x < levelSizeX; ++x) {
            WallData data = new WallData(new Vector2Int(x, levelSizeY - 1));
            wallDataElements.Add(data);
        }

        // Right wall
        for (int y = 1; y < levelSizeY - 1; ++y) {
            WallData data = new WallData(new Vector2Int(levelSizeX - 1, y));
            wallDataElements.Add(data);
        }

        // Instantiate LevelElements from levelElementData objects. 
        foreach (LevelElementData wallData in wallDataElements) {
            Wall wall = (Wall)LevelElementFactory.InstantiateLevelElement(wallData, emptyGameObject.transform);
            levelElements.Add(wall);
        }


        // Create some dummy door elements with horizontal and vertical rotation. 
        DoorData doorData = new DoorData(new Vector2Int(2, 2), ElementRotation.RIGHT);
        Door door = (Door)LevelElementFactory.InstantiateLevelElement(doorData, emptyGameObject.transform);
        levelElements.Add(door);

        doorData = new DoorData(new Vector2Int(6, 6), ElementRotation.UP);
        door = (Door)LevelElementFactory.InstantiateLevelElement(doorData, emptyGameObject.transform);
        levelElements.Add(door);

        // Create a dummy EndZone element.
        EndZoneData endZoneData = new EndZoneData(new Vector2Int(40, 50));
        EndZone endZone = (EndZone)LevelElementFactory.InstantiateLevelElement(endZoneData, emptyGameObject.transform);
        levelElements.Add(endZone);


        // Create some dummy level items.
        IList<ItemData> levelItemData = new List<ItemData> {
            new WeaponData(new Vector2(3.5f, 6f), 30f, WeaponType.PISTOL, -1),
            new WeaponData(new Vector2(6f, 21f), 30f, WeaponType.PISTOL, 10),
            new WeaponData(new Vector2(6.1f, 21.2f), 30f, WeaponType.MACHINEGUN, 300),
            new WeaponData(new Vector2(6.2f, 20.9f), 30f, WeaponType.PISTOL, 13)
        };

        List<Item> levelItems = new List<Item>();
        foreach (ItemData item in levelItemData) {
            if (item is WeaponData) {
                Weapon weaponObject = ItemFactory.InstantiateWeapon((WeaponData)item, emptyGameObject.transform);
                levelItems.Add(weaponObject);
            }
        }

        // Assign a weapon to the player.
        Weapon someWeapon = (Weapon)levelItems[1];
        someWeapon.SetPerson(Player.instance);


        // Create some dummy enemies. 
        IList<EnemyData> enemyData = new List<EnemyData>();
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
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints1, WeaponType.MACHINEGUN, 20, patrolPoints1[0]));
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints2, WeaponType.PISTOL, 25, patrolPoints2[0]));
        enemyData.Add(new EnemyData(EnemyType.BASIC, patrolPoints3, WeaponType.PISTOL, 20, patrolPoints3[0]));


        IList<Enemy> enemies = PersonController.instance.SpawnEnemies(enemyData);

        string expectedLevelJson = "{\"Size\":{\"x\":64,\"y\":64},\"Player\":{\"WeaponType\":\"PISTOL\",\"Amount\":10,\"SpawnPosition\":{\"x\":3.5,\"y\":3.5}},\"LevelElements\":{\"Walls\":[{\"Position\":{\"x\":0,\"y\":0}},{\"Position\":{\"x\":1,\"y\":0}},{\"Position\":{\"x\":2,\"y\":0}},{\"Position\":{\"x\":3,\"y\":0}},{\"Position\":{\"x\":4,\"y\":0}},{\"Position\":{\"x\":5,\"y\":0}},{\"Position\":{\"x\":6,\"y\":0}},{\"Position\":{\"x\":7,\"y\":0}},{\"Position\":{\"x\":8,\"y\":0}},{\"Position\":{\"x\":9,\"y\":0}},{\"Position\":{\"x\":10,\"y\":0}},{\"Position\":{\"x\":11,\"y\":0}},{\"Position\":{\"x\":12,\"y\":0}},{\"Position\":{\"x\":13,\"y\":0}},{\"Position\":{\"x\":14,\"y\":0}},{\"Position\":{\"x\":15,\"y\":0}},{\"Position\":{\"x\":16,\"y\":0}},{\"Position\":{\"x\":17,\"y\":0}},{\"Position\":{\"x\":18,\"y\":0}},{\"Position\":{\"x\":19,\"y\":0}},{\"Position\":{\"x\":20,\"y\":0}},{\"Position\":{\"x\":21,\"y\":0}},{\"Position\":{\"x\":22,\"y\":0}},{\"Position\":{\"x\":23,\"y\":0}},{\"Position\":{\"x\":24,\"y\":0}},{\"Position\":{\"x\":25,\"y\":0}},{\"Position\":{\"x\":26,\"y\":0}},{\"Position\":{\"x\":27,\"y\":0}},{\"Position\":{\"x\":28,\"y\":0}},{\"Position\":{\"x\":29,\"y\":0}},{\"Position\":{\"x\":30,\"y\":0}},{\"Position\":{\"x\":31,\"y\":0}},{\"Position\":{\"x\":32,\"y\":0}},{\"Position\":{\"x\":33,\"y\":0}},{\"Position\":{\"x\":34,\"y\":0}},{\"Position\":{\"x\":35,\"y\":0}},{\"Position\":{\"x\":36,\"y\":0}},{\"Position\":{\"x\":37,\"y\":0}},{\"Position\":{\"x\":38,\"y\":0}},{\"Position\":{\"x\":39,\"y\":0}},{\"Position\":{\"x\":40,\"y\":0}},{\"Position\":{\"x\":41,\"y\":0}},{\"Position\":{\"x\":42,\"y\":0}},{\"Position\":{\"x\":43,\"y\":0}},{\"Position\":{\"x\":44,\"y\":0}},{\"Position\":{\"x\":45,\"y\":0}},{\"Position\":{\"x\":46,\"y\":0}},{\"Position\":{\"x\":47,\"y\":0}},{\"Position\":{\"x\":48,\"y\":0}},{\"Position\":{\"x\":49,\"y\":0}},{\"Position\":{\"x\":50,\"y\":0}},{\"Position\":{\"x\":51,\"y\":0}},{\"Position\":{\"x\":52,\"y\":0}},{\"Position\":{\"x\":53,\"y\":0}},{\"Position\":{\"x\":54,\"y\":0}},{\"Position\":{\"x\":55,\"y\":0}},{\"Position\":{\"x\":56,\"y\":0}},{\"Position\":{\"x\":57,\"y\":0}},{\"Position\":{\"x\":58,\"y\":0}},{\"Position\":{\"x\":59,\"y\":0}},{\"Position\":{\"x\":60,\"y\":0}},{\"Position\":{\"x\":61,\"y\":0}},{\"Position\":{\"x\":62,\"y\":0}},{\"Position\":{\"x\":63,\"y\":0}},{\"Position\":{\"x\":0,\"y\":1}},{\"Position\":{\"x\":0,\"y\":2}},{\"Position\":{\"x\":0,\"y\":3}},{\"Position\":{\"x\":0,\"y\":4}},{\"Position\":{\"x\":0,\"y\":5}},{\"Position\":{\"x\":0,\"y\":6}},{\"Position\":{\"x\":0,\"y\":7}},{\"Position\":{\"x\":0,\"y\":8}},{\"Position\":{\"x\":0,\"y\":9}},{\"Position\":{\"x\":0,\"y\":10}},{\"Position\":{\"x\":0,\"y\":11}},{\"Position\":{\"x\":0,\"y\":12}},{\"Position\":{\"x\":0,\"y\":13}},{\"Position\":{\"x\":0,\"y\":14}},{\"Position\":{\"x\":0,\"y\":15}},{\"Position\":{\"x\":0,\"y\":16}},{\"Position\":{\"x\":0,\"y\":17}},{\"Position\":{\"x\":0,\"y\":18}},{\"Position\":{\"x\":0,\"y\":19}},{\"Position\":{\"x\":0,\"y\":20}},{\"Position\":{\"x\":0,\"y\":21}},{\"Position\":{\"x\":0,\"y\":22}},{\"Position\":{\"x\":0,\"y\":23}},{\"Position\":{\"x\":0,\"y\":24}},{\"Position\":{\"x\":0,\"y\":25}},{\"Position\":{\"x\":0,\"y\":26}},{\"Position\":{\"x\":0,\"y\":27}},{\"Position\":{\"x\":0,\"y\":28}},{\"Position\":{\"x\":0,\"y\":29}},{\"Position\":{\"x\":0,\"y\":30}},{\"Position\":{\"x\":0,\"y\":31}},{\"Position\":{\"x\":0,\"y\":32}},{\"Position\":{\"x\":0,\"y\":33}},{\"Position\":{\"x\":0,\"y\":34}},{\"Position\":{\"x\":0,\"y\":35}},{\"Position\":{\"x\":0,\"y\":36}},{\"Position\":{\"x\":0,\"y\":37}},{\"Position\":{\"x\":0,\"y\":38}},{\"Position\":{\"x\":0,\"y\":39}},{\"Position\":{\"x\":0,\"y\":40}},{\"Position\":{\"x\":0,\"y\":41}},{\"Position\":{\"x\":0,\"y\":42}},{\"Position\":{\"x\":0,\"y\":43}},{\"Position\":{\"x\":0,\"y\":44}},{\"Position\":{\"x\":0,\"y\":45}},{\"Position\":{\"x\":0,\"y\":46}},{\"Position\":{\"x\":0,\"y\":47}},{\"Position\":{\"x\":0,\"y\":48}},{\"Position\":{\"x\":0,\"y\":49}},{\"Position\":{\"x\":0,\"y\":50}},{\"Position\":{\"x\":0,\"y\":51}},{\"Position\":{\"x\":0,\"y\":52}},{\"Position\":{\"x\":0,\"y\":53}},{\"Position\":{\"x\":0,\"y\":54}},{\"Position\":{\"x\":0,\"y\":55}},{\"Position\":{\"x\":0,\"y\":56}},{\"Position\":{\"x\":0,\"y\":57}},{\"Position\":{\"x\":0,\"y\":58}},{\"Position\":{\"x\":0,\"y\":59}},{\"Position\":{\"x\":0,\"y\":60}},{\"Position\":{\"x\":0,\"y\":61}},{\"Position\":{\"x\":0,\"y\":62}},{\"Position\":{\"x\":0,\"y\":63}},{\"Position\":{\"x\":1,\"y\":63}},{\"Position\":{\"x\":2,\"y\":63}},{\"Position\":{\"x\":3,\"y\":63}},{\"Position\":{\"x\":4,\"y\":63}},{\"Position\":{\"x\":5,\"y\":63}},{\"Position\":{\"x\":6,\"y\":63}},{\"Position\":{\"x\":7,\"y\":63}},{\"Position\":{\"x\":8,\"y\":63}},{\"Position\":{\"x\":9,\"y\":63}},{\"Position\":{\"x\":10,\"y\":63}},{\"Position\":{\"x\":11,\"y\":63}},{\"Position\":{\"x\":12,\"y\":63}},{\"Position\":{\"x\":13,\"y\":63}},{\"Position\":{\"x\":14,\"y\":63}},{\"Position\":{\"x\":15,\"y\":63}},{\"Position\":{\"x\":16,\"y\":63}},{\"Position\":{\"x\":17,\"y\":63}},{\"Position\":{\"x\":18,\"y\":63}},{\"Position\":{\"x\":19,\"y\":63}},{\"Position\":{\"x\":20,\"y\":63}},{\"Position\":{\"x\":21,\"y\":63}},{\"Position\":{\"x\":22,\"y\":63}},{\"Position\":{\"x\":23,\"y\":63}},{\"Position\":{\"x\":24,\"y\":63}},{\"Position\":{\"x\":25,\"y\":63}},{\"Position\":{\"x\":26,\"y\":63}},{\"Position\":{\"x\":27,\"y\":63}},{\"Position\":{\"x\":28,\"y\":63}},{\"Position\":{\"x\":29,\"y\":63}},{\"Position\":{\"x\":30,\"y\":63}},{\"Position\":{\"x\":31,\"y\":63}},{\"Position\":{\"x\":32,\"y\":63}},{\"Position\":{\"x\":33,\"y\":63}},{\"Position\":{\"x\":34,\"y\":63}},{\"Position\":{\"x\":35,\"y\":63}},{\"Position\":{\"x\":36,\"y\":63}},{\"Position\":{\"x\":37,\"y\":63}},{\"Position\":{\"x\":38,\"y\":63}},{\"Position\":{\"x\":39,\"y\":63}},{\"Position\":{\"x\":40,\"y\":63}},{\"Position\":{\"x\":41,\"y\":63}},{\"Position\":{\"x\":42,\"y\":63}},{\"Position\":{\"x\":43,\"y\":63}},{\"Position\":{\"x\":44,\"y\":63}},{\"Position\":{\"x\":45,\"y\":63}},{\"Position\":{\"x\":46,\"y\":63}},{\"Position\":{\"x\":47,\"y\":63}},{\"Position\":{\"x\":48,\"y\":63}},{\"Position\":{\"x\":49,\"y\":63}},{\"Position\":{\"x\":50,\"y\":63}},{\"Position\":{\"x\":51,\"y\":63}},{\"Position\":{\"x\":52,\"y\":63}},{\"Position\":{\"x\":53,\"y\":63}},{\"Position\":{\"x\":54,\"y\":63}},{\"Position\":{\"x\":55,\"y\":63}},{\"Position\":{\"x\":56,\"y\":63}},{\"Position\":{\"x\":57,\"y\":63}},{\"Position\":{\"x\":58,\"y\":63}},{\"Position\":{\"x\":59,\"y\":63}},{\"Position\":{\"x\":60,\"y\":63}},{\"Position\":{\"x\":61,\"y\":63}},{\"Position\":{\"x\":62,\"y\":63}},{\"Position\":{\"x\":63,\"y\":63}},{\"Position\":{\"x\":63,\"y\":1}},{\"Position\":{\"x\":63,\"y\":2}},{\"Position\":{\"x\":63,\"y\":3}},{\"Position\":{\"x\":63,\"y\":4}},{\"Position\":{\"x\":63,\"y\":5}},{\"Position\":{\"x\":63,\"y\":6}},{\"Position\":{\"x\":63,\"y\":7}},{\"Position\":{\"x\":63,\"y\":8}},{\"Position\":{\"x\":63,\"y\":9}},{\"Position\":{\"x\":63,\"y\":10}},{\"Position\":{\"x\":63,\"y\":11}},{\"Position\":{\"x\":63,\"y\":12}},{\"Position\":{\"x\":63,\"y\":13}},{\"Position\":{\"x\":63,\"y\":14}},{\"Position\":{\"x\":63,\"y\":15}},{\"Position\":{\"x\":63,\"y\":16}},{\"Position\":{\"x\":63,\"y\":17}},{\"Position\":{\"x\":63,\"y\":18}},{\"Position\":{\"x\":63,\"y\":19}},{\"Position\":{\"x\":63,\"y\":20}},{\"Position\":{\"x\":63,\"y\":21}},{\"Position\":{\"x\":63,\"y\":22}},{\"Position\":{\"x\":63,\"y\":23}},{\"Position\":{\"x\":63,\"y\":24}},{\"Position\":{\"x\":63,\"y\":25}},{\"Position\":{\"x\":63,\"y\":26}},{\"Position\":{\"x\":63,\"y\":27}},{\"Position\":{\"x\":63,\"y\":28}},{\"Position\":{\"x\":63,\"y\":29}},{\"Position\":{\"x\":63,\"y\":30}},{\"Position\":{\"x\":63,\"y\":31}},{\"Position\":{\"x\":63,\"y\":32}},{\"Position\":{\"x\":63,\"y\":33}},{\"Position\":{\"x\":63,\"y\":34}},{\"Position\":{\"x\":63,\"y\":35}},{\"Position\":{\"x\":63,\"y\":36}},{\"Position\":{\"x\":63,\"y\":37}},{\"Position\":{\"x\":63,\"y\":38}},{\"Position\":{\"x\":63,\"y\":39}},{\"Position\":{\"x\":63,\"y\":40}},{\"Position\":{\"x\":63,\"y\":41}},{\"Position\":{\"x\":63,\"y\":42}},{\"Position\":{\"x\":63,\"y\":43}},{\"Position\":{\"x\":63,\"y\":44}},{\"Position\":{\"x\":63,\"y\":45}},{\"Position\":{\"x\":63,\"y\":46}},{\"Position\":{\"x\":63,\"y\":47}},{\"Position\":{\"x\":63,\"y\":48}},{\"Position\":{\"x\":63,\"y\":49}},{\"Position\":{\"x\":63,\"y\":50}},{\"Position\":{\"x\":63,\"y\":51}},{\"Position\":{\"x\":63,\"y\":52}},{\"Position\":{\"x\":63,\"y\":53}},{\"Position\":{\"x\":63,\"y\":54}},{\"Position\":{\"x\":63,\"y\":55}},{\"Position\":{\"x\":63,\"y\":56}},{\"Position\":{\"x\":63,\"y\":57}},{\"Position\":{\"x\":63,\"y\":58}},{\"Position\":{\"x\":63,\"y\":59}},{\"Position\":{\"x\":63,\"y\":60}},{\"Position\":{\"x\":63,\"y\":61}},{\"Position\":{\"x\":63,\"y\":62}}],\"Doors\":[{\"Position\":{\"x\":2,\"y\":2},\"Rotation\":\"HORIZONTAL\"},{\"Position\":{\"x\":6,\"y\":6},\"Rotation\":\"VERTICAL\"}],\"EndZones\":[{\"Position\":{\"x\":40,\"y\":50}}]},\"LevelItems\":{\"Weapons\":[{\"Position\":{\"x\":3.5,\"y\":6.0},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":-1},{\"Position\":{\"x\":3.6,\"y\":3.3},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":10},{\"Position\":{\"x\":6.1,\"y\":21.2},\"Rotation\":0.0,\"Type\":\"MACHINEGUN\",\"Ammount\":300},{\"Position\":{\"x\":6.2,\"y\":20.9},\"Rotation\":0.0,\"Type\":\"PISTOL\",\"Ammount\":13}]},\"Enemies\":[{\"Position\":{\"x\":14.5,\"y\":61.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":14.0,\"y\":61.0},{\"x\":2.0,\"y\":61.0}],\"WeaponType\":\"MACHINEGUN\",\"Amount\":20},{\"Position\":{\"x\":61.5,\"y\":61.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":61.0,\"y\":61.0},{\"x\":61.0,\"y\":3.0}],\"WeaponType\":\"PISTOL\",\"Amount\":25},{\"Position\":{\"x\":2.5,\"y\":18.5},\"Type\":\"BASIC\",\"PatrolPoints\":[{\"x\":2.0,\"y\":18.0},{\"x\":21.0,\"y\":18.0},{\"x\":21.0,\"y\":38.0},{\"x\":2.0,\"y\":38.0}],\"WeaponType\":\"PISTOL\",\"Amount\":20}]}";
        File.WriteAllText(expectedLevelPath, expectedLevelJson);

        // Act.
        jsonLevelSerializer.SerializeLevel(actualLevelPath, dummyLevel, levelElements, levelItems, enemies);

        // Assert. 
        Assert.IsTrue(File.Exists(actualLevelPath));
        FileAssert.AreEqual(expectedLevelPath, actualLevelPath);
        Assert.AreEqual(255, levelElements.Count);
        Assert.AreEqual(4, levelItems.Count);
        Assert.AreEqual(3, enemies.Count);
        Assert.NotNull(Player.instance);
    }
}