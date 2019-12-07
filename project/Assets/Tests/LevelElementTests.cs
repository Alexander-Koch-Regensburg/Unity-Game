using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests {
    [TestClass]
    public class LevelElementTests {
        [Test]
        // Check if position coordinates are negative.
        [TestCase (-5, 5, 1, 1)]
        [TestCase (5, -5, 1, 1)]
        [TestCase (-5, -5, 1, 1)]
        // Check if element size is negative.
        [TestCase(5, 5, -1, 1)]
        [TestCase(5, 5, 1, -1)]
        [TestCase(5, 5, -1, -1)]
        // Check if element size is zero. 
        [TestCase(5, 5, 0, 5)]
        [TestCase(5, 5, 5, 0)]
        [TestCase(5, 5, 0, 0)]
        public void InitPos_ArgumentsOutOfRange_DoNothing(int xCoordinate, int yCoordinate, int sizeX, int sizeY) {
            // Arrange.
            // Use substitute here instead of instantiation because the class is abstract.
            ILevelElement levelElement = Substitute.For<ILevelElement>();

            Vector2Int position = new Vector2Int(xCoordinate, yCoordinate);

            // Act.
            levelElement.InitPos(position, sizeX, sizeY);

            // Assert.
        }
    }
}