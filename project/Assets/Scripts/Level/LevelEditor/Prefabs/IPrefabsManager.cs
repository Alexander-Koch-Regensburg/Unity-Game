using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrefabsManager
{
    void AddPrefabs();
    Dictionary<string, IPlacedObject> GetPrefabTable();
    IPlacedObject GetInstantiatedPrefab(string prefabId, Transform parent);
}
