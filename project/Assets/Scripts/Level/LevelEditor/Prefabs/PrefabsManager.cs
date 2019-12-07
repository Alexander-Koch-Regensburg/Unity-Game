using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabsManager : MonoBehaviour, IPrefabsManager
{
    private static PrefabsManager instance;

    public LevelElementPrefabTable levelElementPrefabTable;
    public PersonPrefabTable personPrefabTable;
    public WeaponPrefabTable weaponPrefabTable;
    
    private static Dictionary<string, IPlacedObject> prefabTable = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        weaponPrefabTable = WeaponPrefabTable.instance;
    }

    public void AddPrefabs()
    {
        prefabTable = new Dictionary<string, IPlacedObject>();
        
        AddPrefab(levelElementPrefabTable.wallPrefab, "wallPrefab", "Wall", PrefabType.LEVELELEMENT);
        AddPrefab(levelElementPrefabTable.door2x1HorizontalPrefab, "door2x1HorizontalPrefab", "Door2x1Horizontal", PrefabType.LEVELELEMENT);
        AddPrefab(levelElementPrefabTable.door2x1VerticalPrefab, "door2x1VerticalPrefab", "Door2x1Vertical", PrefabType.LEVELELEMENT);
        AddPrefab(levelElementPrefabTable.endZonePrefab, "endZonePrefab", "EndZone", PrefabType.LEVELELEMENT);

        AddPrefab(personPrefabTable.playerPrefab.gameObject, "playerPrefab", "Player", PrefabType.PLAYER);

        AddPrefab(personPrefabTable.basicEnemyPrefab.gameObject, "basicEnemyPrefab", "BasicEnemy", PrefabType.ENEMY);
        AddPrefab(personPrefabTable.easyEnemyPrefab.gameObject, "easyEnemyPrefab", "EasyEnemy", PrefabType.ENEMY);
        AddPrefab(personPrefabTable.hardEnemyPrefab.gameObject, "hardEnemyPrefab", "HardEnemy", PrefabType.ENEMY);

        AddPrefab(weaponPrefabTable.pistolPrefab.gameObject, "pistolPrefab", "Pistol", PrefabType.WEAPON);
        AddPrefab(weaponPrefabTable.machineGunPrefab.gameObject, "machineGunPrefab", "MachineGun", PrefabType.WEAPON);
        AddPrefab(weaponPrefabTable.shotgunPrefab.gameObject, "shotgunPrefab", "Shotgun", PrefabType.WEAPON);
        AddPrefab(weaponPrefabTable.batPrefab.gameObject, "batPrefab", "Bat", PrefabType.WEAPON);
    }

    private void AddPrefab(GameObject prefab, string prefabId, string name, PrefabType type)
    {
        GameObject gObject = new GameObject();
        gObject.name = name;
        gObject.transform.SetParent(this.transform);
        IPlacedObject placedObject = (IPlacedObject) gObject.AddComponent<PlacedObject>();
        placedObject.SetupPrefab(prefab, prefabId, name, type, true);
        prefabTable.Add(prefabId, placedObject);
    }

    public static PrefabsManager GetInstance()
    {
        return instance;
    }

    public IPlacedObject GetInstantiatedPrefab(string prefabId, Transform parent)
    {
        IPlacedObject placedObject = (IPlacedObject)Instantiate((PlacedObject)prefabTable[prefabId], new Vector2(-10, -10), ((PlacedObject)prefabTable[prefabId]).gameObject.transform.rotation, parent);
        placedObject.Prefab = Instantiate(prefabTable[prefabId].Prefab, new Vector2(-10, -10), placedObject.Prefab.transform.rotation, ((PlacedObject)placedObject).transform);
        placedObject.SetupObjectToPlace();
        placedObject.IsProtectedObject = false;
        return placedObject;
    }

    public Dictionary<string, IPlacedObject> GetPrefabTable()
    {
        return new Dictionary<string, IPlacedObject>(prefabTable);
    }
}
