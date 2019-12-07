using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    [TestClass]
    public class WallTests {
        private GameObject emptyGameObject;
        private Wall wall;

        [OneTimeSetUp]
        public void Init() {
            emptyGameObject = new GameObject();
            emptyGameObject.AddComponent<Wall>();
            wall = emptyGameObject.GetComponent<Wall>();
        }

        [Test]
        public void IsSolid_ReturnTrue() {
            // Arrange.

            // Act.
            bool actualResult = wall.IsSolid();

            // Assert.
            NUnit.Framework.Assert.AreEqual(true, actualResult);
        }
    }
}