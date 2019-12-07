using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToDataConverter
{
    public static LevelElementData ObjectToLevelElement(IPlacedObject placedObject)
    {
        if (placedObject.Type != PrefabType.LEVELELEMENT)
            return null;

        Door door = placedObject.Prefab.GetComponent<Door>();
        if (door != null)
            return GetDoorData(door, ((PlacedObject)placedObject).transform.position);

        Wall wall = placedObject.Prefab.GetComponent<Wall>();
        if (wall != null)
            return GetWallData(wall, ((PlacedObject)placedObject).transform.position);

        EndZone endZone = placedObject.Prefab.GetComponent<EndZone>();
        if (endZone != null)
            return GetEndZoneData(endZone, ((PlacedObject)placedObject).transform.position);

        return null;
    }
    
    private static DoorData GetDoorData(Door door, Vector3 position)
    {
        ElementRotation elementRotation;
        if (door.transform.rotation.eulerAngles.z == 90)
            elementRotation = ElementRotation.RIGHT;
        else
            elementRotation = ElementRotation.UP;

        DoorData data = new DoorData(ConvertVector3ToVector2Int(position), elementRotation);
        return data;
    }

    private static WallData GetWallData(Wall wall, Vector3 position)
    {
        WallData data = new WallData(ConvertVector3ToVector2Int(position));
        return data;
    }

    private static EndZoneData GetEndZoneData(EndZone endZone, Vector3 position)
    {
        EndZoneData data = new EndZoneData(ConvertVector3ToVector2Int(position));
        return data;
    }

    private static Vector2Int ConvertVector3ToVector2Int(Vector3 worldPosition)
    {
        Vector2 position = worldPosition;
        return Vector2Int.CeilToInt(position);
    }

    public static EnemyData GetEnemyDataForEnemy(IPlacedObject enemyObject)
    {
        Enemy enemy = enemyObject.Prefab.GetComponent<Enemy>();
        // TODO NOTE: The enemyType is not set, so it is always BASIC... 
        // If it set correctly then enable this line and remove the code to determine the type
        //EnemyType enemyType = enemy.Type;
        EnemyType enemyType = DetermineEnemyType(enemyObject);
        Vector3 position = ((PlacedObject)enemyObject).transform.position;
        Weapon weapon = enemy.Weapon;
        WeaponType weaponType;
        int ammo;
        if (weapon != null) {
            ammo = weapon.GetAmmo();
            weaponType = DetermineWeaponType(weapon);
        }
        else {
            weaponType = WeaponType.INVALID;
            ammo = 0;
        }

        if (enemy.PatrolPoints == null)
        {
            enemy.PatrolPoints = new List<Vector2>();
            enemy.PatrolPoints.Add(position);
        }
        
        EnemyData data = new EnemyData(enemyType, enemy.PatrolPoints, weaponType, ammo, position);
        return data;
    }

    private static EnemyType DetermineEnemyType(IPlacedObject enemyObject)
    {
        if (enemyObject.PrefabId == "basicEnemyPrefab")
            return EnemyType.HARD;
        else if (enemyObject.PrefabId == "easyEnemyPrefab")
            return EnemyType.EASY;
        else if (enemyObject.PrefabId == "hardEnemyPrefab")
            return EnemyType.BASIC;
        else
            return EnemyType.BASIC;
    }

    private static WeaponType DetermineWeaponType(Weapon weapon)
    {
        if (weapon is Pistol)
            return WeaponType.PISTOL;
        else if (weapon is MachineGun)
            return WeaponType.MACHINEGUN;
        else if (weapon is Shotgun)
            return WeaponType.SHOTGUN;
        else if (weapon is Bat)
            return WeaponType.BAT;
        else
        {
            Debug.Log("Warning (ObjectToDataConverter): WeaponType could not determined");
            return WeaponType.BAT;
        }
    }

    public static WeaponData GetWeaponDataForWeapon(IPlacedObject weaponObject)
    {
        Weapon weapon = weaponObject.Prefab.GetComponent<Weapon>();
        WeaponType type = DetermineWeaponType(weapon);
        int ammo = weapon.GetAmmo();

        Vector3 position = ((PlacedObject)weaponObject).transform.position;
        float rotation = weapon.transform.rotation.eulerAngles.z;
        WeaponData data = new WeaponData(position, rotation, type, ammo);
        return data;
    }

    public static PlayerData GetPlayerDataForPlayer(IPlacedObject playerObject)
    {
        Player player = playerObject.Prefab.GetComponent<Player>();
        Weapon weapon = player.Weapon;
        WeaponType weaponType;
        int ammo;
        if (weapon != null)
        {
            ammo = weapon.GetAmmo();
            weaponType = DetermineWeaponType(weapon);
        }
        else
        {
            weaponType = WeaponType.INVALID;
            ammo = 0;
        }

        return new PlayerData(weaponType, ammo);
    }

}
