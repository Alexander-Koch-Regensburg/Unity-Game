using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests {
    [TestClass]
    public class LevelTests {

        private Vector2Int spawnPosition;

        [OneTimeSetUp]
        public void Init() {
            // Some dummy spawn position. Just needed for creation of level instances.
            spawnPosition = new Vector2Int(20, 20);
        }
        
        [Test]
        [TestCase(5, -5)]
        [TestCase(-5, 5)]
        [TestCase(-5, -5)]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(0, 0)]
        public void Constructor_CallCreateTilesWithArgumentsOutOfRange_InstantiateTileObjects(int xSizeCoordinate, int ySizeCoordinate) {
            // Arrange

            Vector2Int levelSize = new Vector2Int(xSizeCoordinate, ySizeCoordinate);
            Level level = new Level(levelSize, spawnPosition);

            // Act 
            PrivateObject internLevel = new PrivateObject(level);
            ILevelTile[,] internTilesArr = (ILevelTile[,])internLevel.GetField("tiles");

            // Assert.
            NUnit.Framework.Assert.IsNull(internTilesArr);
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(1, 20)]
        public void Constructor_CallCreateTilesWithValidArguments_InstantiateTileObjects(int xSizeCoordinate, int ySizeCoordinate) {
            // Arrange

            Vector2Int levelSize = new Vector2Int(xSizeCoordinate, ySizeCoordinate);
            Level level = new Level(levelSize, spawnPosition);

            // Act 
            PrivateObject internLevel = new PrivateObject(level);
            ILevelTile[,] internTilesArr = (ILevelTile[,])internLevel.GetField("tiles");

            // Assert.
            NUnit.Framework.Assert.NotNull(internTilesArr);
            NUnit.Framework.Assert.AreEqual(levelSize.x * levelSize.y, internTilesArr.Length);

            // For each created element check if it is instance of ILevelTile and if it is located at the correct position. 
            for (int i = 0; i < levelSize.x; ++i) {
                for (int j = 0; j < levelSize.y; ++j) {
                    NUnit.Framework.Assert.IsInstanceOf<ILevelTile>(internTilesArr[i, j]);
                    NUnit.Framework.Assert.AreEqual(internTilesArr[i, j].Pos.x, i);
                    NUnit.Framework.Assert.AreEqual(internTilesArr[i, j].Pos.y, j);
                }
            }

        }

        [Test]
        public void GetLevelSize_ReturnLevelSize() {
            // Arrange
            Vector2Int levelSize = new Vector2Int(100, 100);
            Level level = new Level(levelSize, spawnPosition);

            // Act 
            Vector2Int actualLevelSize = level.GetLevelSize();

            // Assert.
            NUnit.Framework.Assert.AreEqual(actualLevelSize, levelSize);
        }

        [Test]
        // Position coordinates could be negative.
        [TestCase(5, -5, 100, 100)]
        [TestCase(-5, 5, 100, 100)]
        [TestCase(-5, -5, 100, 100)]
        // Position coordinates could be greater than the level size.
        [TestCase(130, 20, 100, 100)]
        [TestCase(20, 130, 100, 100)]
        [TestCase(130, 130, 100, 100)]
        [TestCase(100, 100, 100, 100)]
        public void GetTileAtPos_PosCoordinatesOutOfRange_ReturnNull(int xCoordinate, int yCoordinate, int levelLength, int levelHeight) {
            // Arrange.
            Vector2Int levelSize = new Vector2Int(levelLength, levelHeight);
			Vector2Int spawnPos = new Vector2Int(5, 5);
			Level level = new Level(levelSize, spawnPos);

            Vector2Int posToFetch = new Vector2Int(xCoordinate, yCoordinate);

            // Act.
            ILevelTile levelTile = level.GetTileAtPos(posToFetch);

            // Assert.
            NUnit.Framework.Assert.IsNull(levelTile);
        }

        [Test]
        [TestCase(10, 20, 100, 100)]
        [TestCase(0, 0, 100, 100)]
        public void GetTileAtPos_PosCoordinatesAreValid_ReturnLevelTileAtPos(int xCoordinate, int yCoordinate, int levelLength, int levelHeight) {
            // Arrange.
            Vector2Int levelSize = new Vector2Int(levelLength, levelHeight);
			Vector2Int spawnPos = new Vector2Int(5, 5);
			Level level = new Level(levelSize, spawnPos);

            Vector2Int posToFetch = new Vector2Int(xCoordinate, yCoordinate);

            // Act.
            ILevelTile actualILevelTile = level.GetTileAtPos(posToFetch);

            // Assert.
            NUnit.Framework.Assert.IsNotNull(actualILevelTile);
            NUnit.Framework.Assert.IsInstanceOf<ILevelTile>(actualILevelTile);
            NUnit.Framework.Assert.AreEqual(posToFetch.x, actualILevelTile.Pos.x);
            NUnit.Framework.Assert.AreEqual(posToFetch.y, actualILevelTile.Pos.y);
        }

        [Test]
        [TestCase(10.0f, 20.0f, 100, 100)]
        [TestCase(10.2f, 20.9f, 100, 100)]
        [TestCase(10.9f, 20.2f, 100, 100)]
        public void GetTileAtPos_UseFloatArgument_ReturnLevelTileAtCuttedPos(float xCoordinate, float yCoordinate, int levelLength, int levelHeight) {
            // Arrange.
            Vector2Int levelSize = new Vector2Int(100, 100);
            Level level = new Level(levelSize, spawnPosition);

            Vector2 FloatPosToFetch = new Vector2(xCoordinate, yCoordinate);
            Vector2Int IntPosToFetch = new Vector2Int((int)FloatPosToFetch.x, (int)FloatPosToFetch.y);

            // Act.
            ILevelTile actualILevelTile = level.GetTileAtPos(FloatPosToFetch);
            ILevelTile expectedILevelTile = level.GetTileAtPos(IntPosToFetch);


            // Assert. 
            NUnit.Framework.Assert.IsNotNull(actualILevelTile);
            NUnit.Framework.Assert.IsInstanceOf<ILevelTile>(actualILevelTile);
            NUnit.Framework.Assert.AreEqual(expectedILevelTile, actualILevelTile);
            NUnit.Framework.Assert.AreEqual(IntPosToFetch.x, actualILevelTile.Pos.x);
            NUnit.Framework.Assert.AreEqual(IntPosToFetch.y, actualILevelTile.Pos.y);
        }

        [Test]
        // Position coordinates could be less than zero.
        [TestCase(5, -5, 10, 10, 100, 100)]
        [TestCase(-5, 5, 10, 10, 100, 100)]
        [TestCase(-5, -5, 10, 10, 100, 100)]
        // Size coordinates could be less than one.
        [TestCase(0, 0, 10, -10, 100, 100)]
        [TestCase(0, 0, -10, 10, 100, 100)]
        [TestCase(0, 0, -10, -10, 100, 100)]
        [TestCase(0, 0, 0, 10, 100, 100)]
        [TestCase(0, 0, 10, 0, 100, 100)]
        [TestCase(0, 0, 0, 0, 100, 100)]
        // The rectangle size could be greater than the level size.
        [TestCase(0, 0, 130, 10, 100, 100)]
        [TestCase(0, 0, 10, 130, 100, 100)]
        [TestCase(0, 0, 130, 130, 100, 100)]
        public void GetTilesInRectangle_ArgumentsOutOfRange_ReturnNull(int xPosCoordinate, int yPosCoordinate, int xSizeCoordinate, int ySizeCoordinate, int levelLength, int levelHeight) {
            // Arrange.
            Vector2Int levelSize = new Vector2Int(levelLength, levelHeight);
			Vector2Int spawnPos = new Vector2Int(5, 5);
			Level level = new Level(levelSize, spawnPos);

            Vector2Int pos = new Vector2Int(xPosCoordinate, yPosCoordinate);
            Vector2Int size = new Vector2Int(xSizeCoordinate, ySizeCoordinate);

            // Act.
            ISet<ILevelTile> levelTilesInRect = level.GetTilesInRectangle(pos, size);

            // Assert. 
            NUnit.Framework.Assert.IsNull(levelTilesInRect);
        }
        
        [Test]
        [TestCase(5, 8, 20, 30, 100, 100)]
        [TestCase(0, 0, 1, 1, 100, 100)]
        [TestCase(0, 0, 100, 100, 100, 100)]
        public void GetTilesInRectangle_ArgumentsAreValid_ReturnILevelTilesInRect(int xPosCoordinate, int yPosCoordinate, int xSizeCoordinate, int ySizeCoordinate, int levelLength, int levelHeight) {
            // Arrange.
            Vector2Int levelSize = new Vector2Int(levelLength, levelHeight);
			Vector2Int spawnPos = new Vector2Int(5, 5);
			Level level = new Level(levelSize, spawnPos);

            Vector2Int pos = new Vector2Int(xPosCoordinate, yPosCoordinate);
            Vector2Int size = new Vector2Int(xSizeCoordinate, ySizeCoordinate);

            PrivateObject internLevel = new PrivateObject(level);
            ILevelTile[,] internTilesArr = (ILevelTile[,])internLevel.GetField("tiles");

            // Act.
            ISet<ILevelTile> levelTilesInRect = level.GetTilesInRectangle(pos, size);

            // Assert. 
            NUnit.Framework.Assert.IsNotNull(levelTilesInRect);
            NUnit.Framework.Assert.AreEqual(size.x * size.y, levelTilesInRect.Count);
            for (int i = pos.x; i < (pos.x + size.x); ++i) {
                for (int j = pos.y; j < (pos.y + size.y); ++j) {
                    NUnit.Framework.Assert.True(levelTilesInRect.Contains(internTilesArr[i, j]));
                }
            }
        }



    }
}