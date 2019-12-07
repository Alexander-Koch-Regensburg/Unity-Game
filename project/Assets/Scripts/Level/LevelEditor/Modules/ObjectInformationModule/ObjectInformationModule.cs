using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInformationModule : MonoBehaviour, IObjectInformationModule
{
    private static ObjectInformationModule instance;
    private Level level;
    private bool playerExists = false;

    public static ObjectInformationModule GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void SetLevel(Level level)
    {
        this.level = level;
    }

    private Dictionary<int, IPlacedObject> inScenePlacedObjects;

    public void Setup(Dictionary<int, IPlacedObject> inScenePlacedObjects)
    {
        this.inScenePlacedObjects = inScenePlacedObjects;
    }

    public IList<IPlacedObject> GetInScenePlacedObjectsForLevelTiles(ISet<ILevelTile> levelTiles)
    {
        IList<IPlacedObject> placedObjects = new List<IPlacedObject>();
        // TODO: Use of another data structure to improve this 
        foreach (IPlacedObject inScenePlacedObject in inScenePlacedObjects.Values)
        {
            foreach (ILevelTile objectLevelTile in inScenePlacedObject.LevelTiles)
            {
                foreach (ILevelTile levelTile in levelTiles)
                {
                    if (objectLevelTile == levelTile)
                    {
                        placedObjects.Add(inScenePlacedObject);
                        break;
                    }
                }
            }
        }
        return placedObjects;
    }

    public bool CheckObjectDeletion(Dictionary<int, IPlacedObject> inScenePlacedObjects, IPlacedObject placedObject)
    {
        if (placedObject != null && inScenePlacedObjects != null && !placedObject.IsProtectedObject)
        {
            return true;
        }
        return false;
    }

    public bool CheckForOuterBoundWall(IPlacedObject plObject)
    {
        if (plObject.Type == PrefabType.LEVELELEMENT)
        {
            Wall wall = plObject.Prefab.GetComponent<Wall>();
            if (wall != null)
            {
                Vector2Int levelSize = level.GetLevelSize();
                Vector3 position = ((PlacedObject)plObject).transform.position;
                if (position.x == 0 || position.y == 0 || position.y == levelSize.y - 1 || position.x == levelSize.x - 1)
                    return true;
            }
        }
        return false;
    }


    private ILevelElement GetLevelElement (IPlacedObject placedObject)
    {
        if (placedObject.Type != PrefabType.LEVELELEMENT)
            return null;

        Door door = placedObject.Prefab.GetComponent<Door>();
        if (door != null)
            return (ILevelElement)door;

        Wall wall = placedObject.Prefab.GetComponent<Wall>();
        if (wall != null)
            return (ILevelElement)wall;

        EndZone endZone = placedObject.Prefab.GetComponent<EndZone>();
        if (endZone != null)
            return (ILevelElement) endZone;

        return null;
    }

    public Vector2 GetOffset(IPlacedObject placedObject)
    {
        Vector2 size = GetSizeOfLevelElement(placedObject);
        return new Vector2(size.x / 2f, size.y / 2f);
    }

    private Vector2 GetSizeOfLevelElement(IPlacedObject placedObject)
    {
        ILevelElement levelElement = GetLevelElement(placedObject);

        if (levelElement is RectLevelElement)
        {
            RectLevelElement rectLevelElement = (RectLevelElement) levelElement;
            if (placedObject.Prefab.transform.rotation.eulerAngles.z == 90)
                return new Vector2(rectLevelElement.Size.y, rectLevelElement.Size.x);

            return rectLevelElement.Size;
        }
        else
            return new Vector2(1, 1);
    }

    public ILevelTile GetLevelTile(Vector2 position)
    {
        return level.GetTileAtPos(position);
    }

    public Vector2 GetPositionOfLevelTile(Vector2 position)
    {
        ILevelTile levelTile = GetLevelTile(position);
        return levelTile.Pos;
    }

    public ISet<ILevelTile> GetLevelTilesOfLevelElement(IPlacedObject placedObject, Vector2 worldPosition)
    {
        Vector2 size = GetSizeOfLevelElement(placedObject);
        Vector2 position = GetPositionOfLevelTile(worldPosition);

        ISet<ILevelTile> levelTiles = new HashSet<ILevelTile>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                // position is the position of the lower left corner of the levelElement
                Vector2 levelTilePos = new Vector2(position.x + x, position.y + y);
                ILevelTile levelTile = GetLevelTile(levelTilePos);
                if (levelTile == null)
                    continue;
                levelTiles.Add(levelTile);
            }
        }
        return levelTiles;
    }

    public bool CheckIfLevelTilesContainsLevelElements(IPlacedObject placedObject, Vector2 worldPosition)
    {
        ISet<ILevelTile> levelTiles = GetLevelTilesOfLevelElement(placedObject, worldPosition);
        IList<IPlacedObject> inScenePlacedObjects = GetInScenePlacedObjectsForLevelTiles(levelTiles);

        foreach (IPlacedObject inScenePlacedObject in inScenePlacedObjects)
        {
            if (inScenePlacedObject.Type == PrefabType.LEVELELEMENT)
                return true;
        }
        return false;
    }
    
    public bool CheckIfObjectCanBeMoved(IPlacedObject selectedObject, Vector2 position)
    {
        ILevelTile levelTile = GetLevelTile(position);
        if (levelTile.IsTraversable())
        {
            return true;
        }
        return false;
    }

    public bool CheckIfWeaponCanBeGivenToPerson(IPlacedObject selectedObject, IPlacedObject hoveredObject)
    {
        if (selectedObject == null)
            return false;

        Weapon weapon = selectedObject.Prefab.GetComponent<Weapon>();
        if (weapon == null)
            return false;

        if (hoveredObject == null)
            return false;

        Person person = hoveredObject.Prefab.GetComponent<Person>();
        if (person == null)
            return false;

        return true;
    }
    
    public bool CheckIfPlayerExists()
    {
        return playerExists;
    }

    public bool CheckIfEndZoneOrEnemyExists(Dictionary<int, IPlacedObject> inScenePlacedObjects)
    {
        foreach (IPlacedObject placedObject in inScenePlacedObjects.Values)
        {
            // Check for endZone
            if (placedObject.PrefabId == "endZonePrefab")
                return true;
            // Check for enemy
            else if (placedObject.Type is PrefabType.ENEMY)
                return true;
        }
        return false;
    }

    public void SetPlayerExists(bool exists)
    {
        this.playerExists = exists;
    }
}
