using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDeletionModule : MonoBehaviour, IObjectDeletionModule
{
    private static ObjectDeletionModule instance;
    private ILevelDataManager levelDataManager;
    private Dictionary<int, IPlacedObject> inScenePlacedObjects;
    private Level level;
    private IObjectInformationModule objectInformationModule;

    public static ObjectDeletionModule GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Setup(IObjectInformationModule objectInformationModule, Dictionary<int, IPlacedObject> inScenePlacedObjects, ILevelDataManager levelDataManager)
    {
        this.objectInformationModule = objectInformationModule;
        this.levelDataManager = levelDataManager;
        this.inScenePlacedObjects = inScenePlacedObjects;
    }

    public void SetLevel(Level level)
    {
        this.level = level;
    }

    public void DeleteObject(IPlacedObject hoveredObject, bool ChecksEnabled = true, bool RemoveFromDict = true)
    {
        if (ChecksEnabled) {
            bool checksPassed = objectInformationModule.CheckObjectDeletion(inScenePlacedObjects, hoveredObject);
            if (!checksPassed)
                return;
        }
        if (hoveredObject != null)
            DeleteObjectIntern(hoveredObject, RemoveFromDict);
    }

    private void DeleteObjectIntern(IPlacedObject hoveredObject, bool RemoveFromDict)
    {
        if (RemoveFromDict)
            RemoveObjectFromDictionary(inScenePlacedObjects, hoveredObject);

        if (hoveredObject.Type == PrefabType.LEVELELEMENT)
        {
            RemoveWallFromTileMap(hoveredObject);
            RemoveLevelElementFromLevelTile(hoveredObject);
            levelDataManager.RemoveLevelElement(hoveredObject);
        }

        if (hoveredObject.Type is PrefabType.PLAYER)
            objectInformationModule.SetPlayerExists(false);

        DestroyPlacedObject(hoveredObject);
        hoveredObject = null;
    } 
    
    public void DestroyPlacedObject(IPlacedObject cloneObject)
    {
        if (cloneObject != null)
        {
            Destroy(((PlacedObject)cloneObject).gameObject);
            cloneObject = null;
        }
    }

    public void RemoveWallFromTileMap(IPlacedObject placedObject)
    {
        Wall wall = placedObject.Prefab.GetComponent<Wall>();
        if (wall != null)
        {
            placedObject.Prefab.transform.position = ((PlacedObject)placedObject).transform.position;
            wall.RemoveFromTilemap();
        }
    }

    private void RemoveObjectFromDictionary(Dictionary<int, IPlacedObject> inScenePlacedObjects, IPlacedObject hoveredObject)
    {
        inScenePlacedObjects.Remove(hoveredObject.ObjectId);
    }

    private void RemoveLevelElementFromLevelTile(IPlacedObject placedObject)
    {
        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            ILevelElement levelElement = placedObject.Prefab.GetComponent<ILevelElement>();
            ISet<ILevelTile> levelTiles = levelElement.GetTiles();
            foreach (ILevelTile levelTile in levelTiles)
            {
                levelTile.RemoveLevelElement(levelElement);
            }
        }
    }

    public void DeleteInScenePlacedObjects()
    {
        //Destroy each gameObject but don't modify the dictionary
        foreach (KeyValuePair<int, IPlacedObject> kvp in inScenePlacedObjects)
        {
            IPlacedObject plObject = kvp.Value;
            DeleteObject(plObject, false, false);
        }
        
        inScenePlacedObjects.Clear();
    }

    public void DeletePlacedObjects(IList<IPlacedObject> placedObjects, bool ChecksEnabled = true, bool RemoveFromDict = true)
    {
        for (int i = 0; i < placedObjects.Count; i++)
        {
            DeleteObject(placedObjects[i], ChecksEnabled, RemoveFromDict);
        }
    }
    /*
    public void RemovePlacedObjectsFromInScenePlacedObjects(IList<IPlacedObject> placedObjects)
    {
        foreach (IPlacedObject placedObject in placedObjects)
        {
            inScenePlacedObjects.Remove(placedObject.ObjectId);
        }
    }
    */
}
