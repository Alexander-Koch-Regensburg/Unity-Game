using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

[TestClass]
public class LevelTileTests
{
    private Vector2Int tilePosition;
    private LevelTile levelTile;

    [OneTimeSetUp]
    public void Init() {
        tilePosition = new Vector2Int(5, 5);
    }

    [Test]
    public void AddLevelElement_LevelElementsAreEmpty_CreateNewSetOfLevelElements() {
        // Arrange.
        levelTile = new LevelTile(tilePosition);
        ILevelElement levelElementToAdd = Substitute.For<ILevelElement>();

        // Act.
        levelTile.AddLevelElement(levelElementToAdd);

        // Assert. 
        NUnit.Framework.Assert.True(levelTile.LevelElements.Contains(levelElementToAdd));
        NUnit.Framework.Assert.AreEqual(1, levelTile.LevelElements.Count);
    }

    [Test]
    public void AddLevelElement_LevelElementsAreFilled_AddElementToSetOfLevelElements() {
        // Arrange.
        levelTile = new LevelTile(tilePosition);
        for (int i = 0; i < 5; ++i) {
            ILevelElement tmpLevelElement = Substitute.For<ILevelElement>();
            levelTile.AddLevelElement(tmpLevelElement);
        }

        ILevelElement levelElementToAdd = Substitute.For<ILevelElement>();

        // Act.
        levelTile.AddLevelElement(levelElementToAdd);

        // Assert. 
        NUnit.Framework.Assert.AreEqual(6, levelTile.LevelElements.Count);
        NUnit.Framework.Assert.True(levelTile.LevelElements.Contains(levelElementToAdd));
    }

    [Test]
    public void RemoveLevelElement_TileContainsLevelElement_RemoveLevelElementFromSet() {
        // Arrange.
        levelTile = new LevelTile(tilePosition);

        ILevelElement levelElementToRemove = Substitute.For<ILevelElement>();
        levelTile.AddLevelElement(levelElementToRemove);

        // Act.
        levelTile.RemoveLevelElement(levelElementToRemove);

        // Assert. 
        NUnit.Framework.Assert.AreEqual(0, levelTile.LevelElements.Count);
        NUnit.Framework.Assert.False(levelTile.LevelElements.Contains(levelElementToRemove));
    }

    [Test]
    public void RemoveLevelElement_TileNotContainsLevelElement_DoNothing() {
        // Arrange.
        levelTile = new LevelTile(tilePosition);

        ILevelElement levelElementToRemove = Substitute.For<ILevelElement>();

        // Act.
        levelTile.RemoveLevelElement(levelElementToRemove);

        // Assert. 
        NUnit.Framework.Assert.IsNull(levelTile.LevelElements);
    }

	[Test]
	public void IsTraversable_NoLevelElements_True() {
		levelTile = new LevelTile(tilePosition);

		NUnit.Framework.Assert.IsTrue(levelTile.IsTraversable());
	}

	[Test]
	public void IsTraversable_NonSolidLevelElements_True() {
		levelTile = new LevelTile(tilePosition);

		Floor floor = Substitute.For<Floor>();
		ILevelElement element1 = Substitute.For<ILevelElement>();
		element1.IsSolid().Returns(false);
		ILevelElement element2 = Substitute.For<ILevelElement>();
		element2.IsSolid().Returns(false);

		levelTile.AddLevelElement(element1);
		levelTile.AddLevelElement(element2);

		NUnit.Framework.Assert.IsTrue(levelTile.IsTraversable());
	}

	[Test]
	public void IsTraversable_SolidLevelElements_False() {
		levelTile = new LevelTile(tilePosition);

		Floor floor = Substitute.For<Floor>();
		ILevelElement element1 = Substitute.For<ILevelElement>();
		element1.IsSolid().Returns(false);
		ILevelElement element2 = Substitute.For<ILevelElement>();
		element2.IsSolid().Returns(true);

		levelTile.AddLevelElement(element1);
		levelTile.AddLevelElement(element2);

		NUnit.Framework.Assert.IsFalse(levelTile.IsTraversable());
	}
}
