using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStartupModule
{
    Level InitializeLevel(int width, int height);
    void SetupLevel(IPlacedObject wallPrefabObject);
    void Setup(IObjectDeletionModule objectDeletionModule, IObjectPlacementModule objectPlacementModule, IObjectInformationModule objectInformationModule, IObjectEditingModule objectEditingModule, ILevelDataManager levelDataManager, ILevelController levelController, IPrefabsManager prefabsManager);
    void ConvertGameObjects(LevelDataCollection levelData);
    void SetLevel(Level level);
}
