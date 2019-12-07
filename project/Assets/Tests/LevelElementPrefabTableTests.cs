using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestClass]
public class LevelElementPrefabTableTests {
    private GameObject emptyGameObject;
    private LevelElementPrefabTable prefabTable;
    // Position and size are dummy values, just needed to create a FloorData prefab. 
    private Vector2Int dummyPos;
    private Vector2Int dummySize; 

    [OneTimeSetUp]
    public void Init() {
        emptyGameObject = new GameObject();
        emptyGameObject.AddComponent<LevelElementPrefabTable>();
        prefabTable = emptyGameObject.GetComponent<LevelElementPrefabTable>();
        prefabTable.wallPrefab = new GameObject();
        dummyPos = new Vector2Int(2, 2);
        dummySize = new Vector2Int(2, 2);
    }

    [Test]
    public void GetPrefab_LevelElementIsWallData_ReturnWallPrefab() {
        // Arrange
        WallData wallData = Substitute.For<WallData>(dummyPos);

        // Act
        GameObject actualResult = prefabTable.GetPrefab(wallData);

        // Assert
        NUnit.Framework.Assert.IsNotNull(actualResult);
        NUnit.Framework.Assert.AreEqual(prefabTable.wallPrefab, actualResult);
    }

    [Test]
    public void GetPrefab_LevelElementIsInvalid_ReturnNull() {
        // Arrange
        RectLevelElementData rectLevelElementData = Substitute.For<RectLevelElementData>(dummyPos, dummySize, ElementRotation.UP);

        // Act
        GameObject actualResult = prefabTable.GetPrefab(rectLevelElementData);

        // Assert
        NUnit.Framework.Assert.IsNull(actualResult);
        NUnit.Framework.Assert.AreNotEqual(prefabTable.wallPrefab, actualResult);
    }
}
