using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacedObject
{
    GameObject Prefab { get; set; }
    PrefabType Type { get; }
    string Text { get; }
    int ObjectId { get; }
    string PrefabId { get; }
    bool IsProtectedObject { get; set;  }
    IList<ILevelTile> LevelTiles { get; set;  }

    void SetupPrefab(GameObject prefab, string prefabId, string name, PrefabType type, bool isOuterBoundLevelWall);
    void SetupInstantiatedObject(IList<ILevelTile> levelTiles, Transform parent);
    void SetupObjectToPlace();
    void SetAllCollidersStatus(bool active);
    void RemoveWeaponFromPerson();
    bool BelongsToParentObject();
    void SetProtectedObject(bool isProtectedObject);
}
