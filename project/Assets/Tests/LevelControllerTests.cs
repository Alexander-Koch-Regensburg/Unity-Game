using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestClass]
public class LevelControllerTests
{
    private GameObject emptyGameObject;
    private LevelController levelController;

    [OneTimeSetUp]
    public void Init() {
        emptyGameObject = new GameObject();
        emptyGameObject.AddComponent<LevelController>();
        levelController = emptyGameObject.GetComponent<LevelController>();
    }

    [Test]
    public void LoadLevel_LevelLoaderIsNull_ThrowNullReferenceException() {
        // Arrange.

        // Act.
        
        // No Act part needed here, because the call of Start() appears at the creation of the levelController instance. 

        // Assert.
        UnityEngine.TestTools.LogAssert.Expect(LogType.Error, "Cannot load level. No ILevelLoader assigned!");
        NUnit.Framework.Assert.Throws<NullReferenceException>(levelController.LoadLevel);
    }

    [Test]
    public void LoadLevel_LevelLoaderIsSet_LoadLevel() {
        // Arrange.

        PrivateObject internLevelController = new PrivateObject(levelController);
        ILevelLoader levelLoader = (ILevelLoader)internLevelController.GetField("levelLoader");

        levelLoader = Substitute.For<ILevelLoader>();
        internLevelController.SetField("levelLoader", levelLoader);

        // Act.

        // No Act part needed here, because the call of Start() appears at the creation of the levelController instance. 

        // Assert.
        NUnit.Framework.Assert.DoesNotThrow(levelController.LoadLevel);
        levelLoader.Received(1).LoadLevel();
        levelLoader.Received(1).LoadLevelElements(Arg.Any<Transform>());
    }

    [Test]
    [TestCase(-5.0f, 5.0f)]
    [TestCase(5.0f, -5.0f)]
    [TestCase(-5.0f, -5.0f)]
    public void GetTileAtWorldPos_FloatCoordinatesOutOfRange_ReturnNull(float xCoordinate, float yCoordinate) {
        // Arrange.
        Vector2 positionToFetch = new Vector2(xCoordinate, yCoordinate);

        // Act.
        ILevelTile actualResult = levelController.GetTileAtWorldPos(positionToFetch);

        // Assert.
        NUnit.Framework.Assert.IsNull(actualResult);
    }


    [Test]
    [TestCase(5.0f, 5.0f)]
    [TestCase(0.0f, 0.0f)]
    public void GetTileAtWorldPos_FloatCoordinatesAreValid_ReturnWorldPosition(float xCoordinate, float yCoordinate) {
        // Arrange.
        Vector2Int dummyLevelSize = new Vector2Int(100, 100);
		Vector2Int spawnPos = new Vector2Int(5, 5);
        Level dummyLevel = new Level(dummyLevelSize, spawnPos);

        PrivateObject internLevelController = new PrivateObject(levelController);
        internLevelController.SetField("level", dummyLevel);

        Vector2 positionToFetch = new Vector2(xCoordinate, yCoordinate);

        ILevelTile expectedResult = dummyLevel.GetTileAtPos(positionToFetch);

        // Act.
        ILevelTile actualResult = levelController.GetTileAtWorldPos(positionToFetch);

        // Assert.
        NUnit.Framework.Assert.IsNotNull(actualResult);
        NUnit.Framework.Assert.AreEqual(expectedResult, actualResult);
    }

    [Test]
    [TestCase(-5, 5)]
    [TestCase(5, -5)]
    [TestCase(-5, -5)]
    public void GetTileAtWorldPos_IntCoordinatesOutOfRange_ReturnNull(int xCoordinate, int yCoordinate) {
        // Arrange.
        Vector2Int positionToFetch = new Vector2Int(xCoordinate, yCoordinate);

        // Act.
        ILevelTile actualResult = levelController.GetTileAtWorldPos(positionToFetch);

        // Assert.
        NUnit.Framework.Assert.IsNull(actualResult);
    }

    [Test]
    [TestCase(5, 5)]
    [TestCase(0, 0)]
    public void GetTileAtWorldPos_IntCoordinatesAreValid_ReturnWorldPosition(int xCoordinate, int yCoordinate) {
        // Arrange.
        Vector2Int dummyLevelSize = new Vector2Int(100, 100);
		Vector2Int spawnPos = new Vector2Int(5, 5);
		Level dummyLevel = new Level(dummyLevelSize, spawnPos);

        PrivateObject internLevelController = new PrivateObject(levelController);
        internLevelController.SetField("level", dummyLevel);

        Vector2Int positionToFetch = new Vector2Int(xCoordinate, yCoordinate);

        ILevelTile expectedResult = dummyLevel.GetTileAtPos(positionToFetch);

        // Act.
        ILevelTile actualResult = levelController.GetTileAtWorldPos(positionToFetch);

        // Assert.
        NUnit.Framework.Assert.IsNotNull(actualResult);
        NUnit.Framework.Assert.AreEqual(expectedResult, actualResult);
    }

    [Test]
    [TestCase(-5, 5)]
    [TestCase(5, -5)]
    [TestCase(-5, -5)]
    public void GetTilesInRectangle_PositionCoordinatesOutOfRange_ReturnNull(int xCoordinate, int yCoordinate) {
        // Arrange.
        Vector2Int position = new Vector2Int(xCoordinate, yCoordinate);

        // Some dummy size of the rectangle. Not used here, just to call the target method that has to be tested.
        Vector2Int size = new Vector2Int(20, 20);

        // Act.
        ISet<ILevelTile> actualResult = levelController.GetTilesInRectangle(position, size);

        // Assert.
        NUnit.Framework.Assert.IsNull(actualResult);
    }


    [Test]
    [TestCase(-5, 5)]
    [TestCase(5, -5)]
    [TestCase(-5, -5)]
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    [TestCase(0, 0)]
    public void GetTilesInRectangle_SizeCoordinatesOutOfRange_ReturnNull(int xCoordinate, int yCoordinate) {
        // Arrange.

        // Some dummy position to start. 
        Vector2Int position = new Vector2Int(20, 10);
        Vector2Int size = new Vector2Int(xCoordinate, yCoordinate);

        // Act.
        ISet<ILevelTile> actualResult = levelController.GetTilesInRectangle(position, size);

        // Assert.
        NUnit.Framework.Assert.IsNull(actualResult);
    }

    [Test]
    [TestCase(5, 5, 1, 1)]
    [TestCase(0, 0, 1, 1)]
    public void GetTilesInRectangle_PosAndSizeArgumentsAreValid_ReturnTilesInRectangle(int xPosCoordinate, int yPosCoordinate, int xSizeCoordinate, int ySizeCoordinate) {
        // Arrange.
        Vector2Int position = new Vector2Int(xPosCoordinate, yPosCoordinate);
        Vector2Int size = new Vector2Int(xSizeCoordinate, ySizeCoordinate);

        Vector2Int dummyLevelSize = new Vector2Int(100, 100);
		Vector2Int spawnPos = new Vector2Int(5, 5);
		Level dummyLevel = new Level(dummyLevelSize, spawnPos);

        PrivateObject internLevelController = new PrivateObject(levelController);
        internLevelController.SetField("level", dummyLevel);

        ISet<ILevelTile> expectedResult = dummyLevel.GetTilesInRectangle(position, size);

        // Act.
        ISet<ILevelTile> actualResult = levelController.GetTilesInRectangle(position, size);

        // Assert.
        NUnit.Framework.Assert.IsNotNull(actualResult);
        NUnit.Framework.Assert.AreEqual(expectedResult, actualResult);
    }
}
