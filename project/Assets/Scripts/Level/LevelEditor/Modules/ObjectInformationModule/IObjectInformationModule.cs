using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectInformationModule
{
    void Setup(Dictionary<int, IPlacedObject> inScenePlacedObjects);
    void SetLevel(Level level);

    bool CheckIfLevelTilesContainsLevelElements(IPlacedObject placedObject, Vector2 worldPosition);
    IList<IPlacedObject> GetInScenePlacedObjectsForLevelTiles(ISet<ILevelTile> levelTiles);
    ISet<ILevelTile> GetLevelTilesOfLevelElement(IPlacedObject placedObject, Vector2 worldPosition);
    bool CheckObjectDeletion(Dictionary<int, IPlacedObject> inScenePlacedObjects, IPlacedObject placedObject);
    bool CheckForOuterBoundWall(IPlacedObject plObject);
    Vector2 GetOffset(IPlacedObject placedObject);
    Vector2 GetPositionOfLevelTile(Vector2 position);
    bool CheckIfObjectCanBeMoved(IPlacedObject selectedObject, Vector2 position);
    bool CheckIfWeaponCanBeGivenToPerson(IPlacedObject selectedObject, IPlacedObject hoveredObject);
    bool CheckIfPlayerExists();
    bool CheckIfEndZoneOrEnemyExists(Dictionary<int, IPlacedObject> inScenePlacedObjects);
    void SetPlayerExists(bool exists);
    ILevelTile GetLevelTile(Vector2 position);
}
