using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour, ILevelDataManager
{
    private static LevelDataCollection levelData;
    private static LevelDataManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    public LevelDataCollection GetLevelDataCollection()
    {
        return levelData;
    }

    public void InitializeLevelData()
    {
        levelData = new LevelDataCollection();
    }

    public void ResetLevelElements()
    {
        levelData.levelElements = new List<ILevelElement>();
    }

    public void SavePlacedObjects(Dictionary<int, IPlacedObject> placedObjects, Level level)
    {
        PrepareForSaving();
        foreach (IPlacedObject plObject in placedObjects.Values)
        {
            if (plObject.BelongsToParentObject())
                continue;

            switch (plObject.Type)
            {
                case PrefabType.LEVELELEMENT:
                    // Do nothing. Data is already added
                    break;
                case PrefabType.PLAYER:
                    AddPlayerData(plObject, level);
                    break;
                case PrefabType.ENEMY:
                    AddEnemyData(plObject);
                    break;
                case PrefabType.WEAPON:
                    AddWeaponData(plObject);
                    break;
                default:
                    Debug.Log("Warning(LevelEditor-LevelDataManager): Prefab of type " + plObject.Type + " unknown");
                    break;
            }
        }
    }

    private void PrepareForSaving()
    {
        ResetEnemyData();
        ResetEnemies();
        ResetWeaponData();
        ResetWeapons();
    }

    private void ResetEnemyData()
    {
        levelData.enemyData = new List<EnemyData>();
    }

    private void ResetEnemies()
    {
        levelData.enemies = new List<Enemy>();
    }

    private void ResetWeaponData()
    {
        levelData.levelItemsData = new List<ItemData>();
    }

    private void ResetWeapons()
    {
        levelData.levelItems = new List<Item>();
    }

    private void AddPlayerData(IPlacedObject plObject, Level level)
    {
        levelData.playerData = ObjectToDataConverter.GetPlayerDataForPlayer(plObject);
        level.SetSpawnPosition(((PlacedObject)plObject).transform.position);
    }

    private void AddEnemyData(IPlacedObject plObject)
    {
        Enemy enemy = plObject.Prefab.GetComponent<Enemy>();
        EnemyData enemyData = ObjectToDataConverter.GetEnemyDataForEnemy(plObject);
        enemy.Type = enemyData.type;
        levelData.enemyData.Add(enemyData);
        levelData.enemies.Add(enemy);
    }

    private void AddWeaponData(IPlacedObject plObject)
    {
        Weapon weapon = plObject.Prefab.GetComponent<Weapon>();
        WeaponData weaponData = ObjectToDataConverter.GetWeaponDataForWeapon(plObject);
        levelData.levelItemsData.Add((ItemData)weaponData);
        levelData.levelItems.Add((Item)weapon);
    }

    public void RemoveLevelElement(IPlacedObject plObject)
    {
        ILevelElement levelElement = plObject.Prefab.GetComponent<ILevelElement>();
        levelData.levelElements.Remove(levelElement);

    }

    public void AddLevelElement(ILevelElement levelElement)
    {
        levelData.levelElements.Add(levelElement);
    }

    public static LevelDataManager GetInstance()
    {
        return instance;
    }
}
