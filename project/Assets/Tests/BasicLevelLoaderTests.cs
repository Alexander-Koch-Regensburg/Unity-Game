using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestClass]
public class BasicLevelLoaderTests
{
    [Test]
    public void LoadLevel_ReturnNewLevel() {
        // Arrange.
        ILevelDataProvider levelDataProvider = Substitute.For<ILevelDataProvider>();
        BasicLevelLoader basicLevelLoader = new BasicLevelLoader(levelDataProvider);

        LevelData expectedLevelData = new LevelData {
            size = new Vector2Int(100, 100)
        };
        levelDataProvider.GetLevelData().Returns(expectedLevelData);

        // Act.
        Level actualResult = basicLevelLoader.LoadLevel();

        // Assert.
        NUnit.Framework.Assert.AreEqual(expectedLevelData.size, actualResult.GetLevelSize());
    }
}
