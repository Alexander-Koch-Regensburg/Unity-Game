using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectDeletionModule
{
    void Setup(IObjectInformationModule objectInformationModule, Dictionary<int, IPlacedObject> inScenePlacedObjects, ILevelDataManager levelDataManager);
    void DeleteObject(IPlacedObject hoveredObject, bool ChecksEnabled = true, bool RemoveFromDict = true);
    void DeleteInScenePlacedObjects();
    void DestroyPlacedObject(IPlacedObject cloneObject);
    void DeletePlacedObjects(IList<IPlacedObject> placedObjects, bool ChecksEnabled = true, bool RemoveFromDict = true);
    //void RemovePlacedObjectsFromInScenePlacedObjects(IList<IPlacedObject> placedObjects);
    void SetLevel(Level level);
    void RemoveWallFromTileMap(IPlacedObject placedObject);
}
