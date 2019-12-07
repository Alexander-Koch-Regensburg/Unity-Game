using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupModule : MonoBehaviour, IStartupModule
{
    private static StartupModule instance;

    private IObjectDeletionModule objectDeletionModule;
    private IObjectPlacementModule objectPlacementModule;
    private IObjectInformationModule objectInformationModule;
    private IObjectEditingModule objectEditingModule;
    private ILevelDataManager levelDataManager;
    private Level level;
    private ILevelController levelController;
    private IPrefabsManager prefabsManager;

    public static StartupModule GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Setup(IObjectDeletionModule objectDeletionModule, IObjectPlacementModule objectPlacementModule, IObjectInformationModule objectInformationModule, IObjectEditingModule objectEditingModule, ILevelDataManager levelDataManager, ILevelController levelController, IPrefabsManager prefabsManager)
    {
        this.objectInformationModule = objectInformationModule;
        this.objectDeletionModule = objectDeletionModule;
        this.levelDataManager = levelDataManager;
        this.objectPlacementModule = objectPlacementModule;
        this.objectEditingModule = objectEditingModule;
        this.levelController = levelController;
        this.prefabsManager = prefabsManager;
    }

    public Level InitializeLevel(int width, int height)
    {
        return CreateLevel(width, height);
    }

    public Level CreateLevel(int width, int height)
    {
        Vector2Int worldSize = new Vector2Int(width, height);
        Vector2Int spawnPosition = new Vector2Int(10, 10);

        level = new Level(worldSize, spawnPosition);
        return level;
    }

    public void SetLevel(Level level)
    {
        this.level = level;
    }
    
    public void SetupLevel(IPlacedObject wallPrefabObject)
    {
        TilemapController.instance.ClearTilemaps();
        FillTilesWithFloor();
        CreateMouseCollision();
        CreateOuterBoundWalls(wallPrefabObject);
    }

    private void CreateMouseCollision()
    {
        Vector2Int levelSize = level.GetLevelSize();

        FloorTilemap tilemap = FloorTilemap.instance;
        BoxCollider collider = tilemap.gameObject.GetComponent<BoxCollider>();
        if (collider != null)
            Destroy(tilemap.gameObject.GetComponent<BoxCollider>());

        BoxCollider newCollider = tilemap.gameObject.AddComponent<BoxCollider>();
        newCollider.size = new Vector3(levelSize.x - 2, levelSize.y - 2, 0);
        newCollider.size = new Vector3(levelSize.x, levelSize.y, 0);
    }

    private void FillTilesWithFloor()
    {
        Vector2Int levelSize = level.GetLevelSize();

        for (int x = 0; x < levelSize.x; ++x)
        {
            for (int y = 0; y < levelSize.y; ++y)
            {
                Vector2Int pos = new Vector2Int(x, y);
                ILevelTile tile = level.GetTileAtPos(pos);

                FloorData floorData = new FloorData(pos);

                ILevelElement floor = LevelElementFactory.InstantiateLevelElement(floorData, transform);
                levelDataManager.AddLevelElement(floor);
            }
        }
    }

    private void CreateOuterBoundWalls(IPlacedObject wallPrefabObject)
    {
        Vector2Int levelSize = level.GetLevelSize();
        Vector3 position;
        // Lower wall
        for (int x = 0; x < levelSize.x; ++x)
        {
            position = new Vector3(x, 0);
            objectPlacementModule.PlaceObject(wallPrefabObject, position);
        }

        // Left wall
        for (int y = 1; y < levelSize.y; ++y)
        {
            position = new Vector3(0, y);
            objectPlacementModule.PlaceObject(wallPrefabObject, position);
        }

        // Upper wall
        for (int x = 1; x < levelSize.x; ++x)
        {
            position = new Vector3(x, levelSize.y - 1);
            objectPlacementModule.PlaceObject(wallPrefabObject, position);
        }

        // right wall
        for (int y = 1; y < levelSize.y - 1; ++y)
        {
            position = new Vector3(levelSize.x - 1, y);
            objectPlacementModule.PlaceObject(wallPrefabObject, position);
        }
    }

    public void ConvertGameObjects(LevelDataCollection levelData) { 
        List<Enemy> enemies = new List<Enemy>(levelData.enemies);
        List<ILevelElement> levelElements = new List<ILevelElement>(levelData.levelElements);
        List<Item> items = new List<Item>(levelData.levelItems);
        PlayerData playerData = levelData.playerData;

        ConvertEnemies(enemies);
        ConvertLevelElements(levelElements);
        ConvertWeapons(items);
        SpawnPlayer(playerData, level);
    }

    private void ConvertEnemies(IList<Enemy> enemies)
    {
        IPlacedObject placedObject;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Type is EnemyType.HARD)
            {
                placedObject = prefabsManager.GetInstantiatedPrefab("hardEnemyPrefab", transform);
            }
            else if (enemy.Type is EnemyType.EASY)
            {
                placedObject = prefabsManager.GetInstantiatedPrefab("easyEnemyPrefab", transform);
            }
            else if (enemy.Type is EnemyType.BASIC)
            {
                placedObject = prefabsManager.GetInstantiatedPrefab("basicEnemyPrefab", transform);
            }
            else
            {
                Debug.Log("Enemy of Type " + enemy.GetType() + "could not be determined");
                Destroy(enemy.gameObject);
                continue;
            }
            Destroy(placedObject.Prefab);
            placedObject.Prefab = enemy.gameObject;

            Weapon weapon = enemy.Weapon;
            ConvertWeapon(weapon);
            enemy.Weapon = null;

            objectPlacementModule.PlaceObject(placedObject, placedObject.Prefab.transform.position, false, null, false);

            if (weapon != null)
            {
                IPlacedObject weaponObject = (IPlacedObject)weapon.gameObject.transform.parent.GetComponent<PlacedObject>();
                objectEditingModule.GiveWeaponToPerson(weaponObject, placedObject);
            }
        }
    }

    private void ConvertLevelElements(IList<ILevelElement> levelElements)
    {
        IPlacedObject placedObject;
        GameObject levelElementObject;
        foreach (ILevelElement levelElement in levelElements)
        {
            if (levelElement is Door)
            {
                levelElementObject = ((Door)levelElement).gameObject;
                if (levelElementObject.transform.rotation.eulerAngles.z == 90)
                {
                    placedObject = prefabsManager.GetInstantiatedPrefab("door2x1VerticalPrefab", transform);
                }
                else
                {
                    placedObject = prefabsManager.GetInstantiatedPrefab("door2x1HorizontalPrefab", transform);
                }

            }
            else if (levelElement is EndZone)
            {
                placedObject = prefabsManager.GetInstantiatedPrefab("endZonePrefab", transform);
                levelElementObject = ((EndZone)levelElement).gameObject;
            }
            else if (levelElement is Wall)
            {
                placedObject = prefabsManager.GetInstantiatedPrefab("wallPrefab", transform);
                levelElementObject = ((Wall)levelElement).gameObject;
            }
            else if (levelElement is Floor)
            {
                continue;
            }
            else
            {
                Debug.Log("LevelElement of Type " + levelElement.GetType() + "could not be determined");
                continue;
            }

            Vector2 offset = objectInformationModule.GetOffset(placedObject);
            Vector2 leftLowerCornerPos = new Vector2(levelElementObject.transform.position.x - offset.x, levelElementObject.transform.position.y - offset.y);
            objectPlacementModule.PlaceObject(placedObject, leftLowerCornerPos, false, levelElement, false);
        }
    }

    private void ConvertWeapons(IList<Item> levelItems)
    {
        foreach (Item item in levelItems)
        {
            if (item is Weapon)
            {
                ConvertWeapon((Weapon)item);
            }
        }
    }

    private void ConvertWeapon(Weapon weapon)
    {
        IPlacedObject placedObject;
        if (weapon == null)
            return;

        if (weapon is MachineGun)
        {
            placedObject = prefabsManager.GetInstantiatedPrefab("machineGunPrefab", transform);
        }
        else if (weapon is Pistol)
        {
            placedObject = prefabsManager.GetInstantiatedPrefab("pistolPrefab", transform);
        }
        else if (weapon is Shotgun)
        {
            placedObject = prefabsManager.GetInstantiatedPrefab("shotgunPrefab", transform);
        }
        else if (weapon is Bat)
        {
            placedObject = prefabsManager.GetInstantiatedPrefab("batPrefab", transform);
        }
        else
        {
            Debug.Log("Weapon of Type " + weapon.GetType() + "could not be determined");
            Destroy(weapon.gameObject);
            return;
        }
        GameObject weaponObject = weapon.gameObject;
        Destroy(placedObject.Prefab);
        placedObject.Prefab = weaponObject;
        objectPlacementModule.PlaceObject(placedObject, weaponObject.transform.position, false, null, false);
    }

    private void SpawnPlayer(PlayerData playerData, Level level)
    {
        Vector2 position = level.getSpawnPosition();
        Player player = PersonFactory.InstantiatePlayer(playerData, position, transform);

        Weapon weapon = player.Weapon;
        ConvertWeapon(weapon);
        player.Weapon = null;

        IPlacedObject placedObject = prefabsManager.GetInstantiatedPrefab("playerPrefab", transform);
        Destroy(placedObject.Prefab);
        placedObject.Prefab = player.gameObject;
        objectPlacementModule.PlaceObject(placedObject, player.gameObject.transform.position, false, null, false);

        if (weapon != null)
        {
            IPlacedObject weaponObject = (IPlacedObject)weapon.gameObject.transform.parent.GetComponent<PlacedObject>();
            objectEditingModule.GiveWeaponToPerson(weaponObject, placedObject);
        }
        Player.instance = player;
    }
}
