using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPlacementModule
{
    void Setup(Dictionary<int, IPlacedObject> inScenePlacedObjects, IObjectDeletionModule objectDeletionModule, IObjectInformationModule objectInformationModule, Sprite wallSprite, ILevelDataManager levelDataManager);
    void PlaceObject(IPlacedObject objectToPlace, Vector3 mousePosition, bool checksEnabled = true, ILevelElement levelElement = null, bool cloneObject = true);
    void SetWallSprite(IPlacedObject placedObject);
    void SetLevel(Level level);
}
