using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelDataManager
{
    void ResetLevelElements();
    void RemoveLevelElement(IPlacedObject plObject);
    void AddLevelElement(ILevelElement levelElement);
    void SavePlacedObjects(Dictionary<int, IPlacedObject> placedObjects, Level level);
    void InitializeLevelData();
    LevelDataCollection GetLevelDataCollection();
}
