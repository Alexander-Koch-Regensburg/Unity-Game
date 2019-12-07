using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    [TestClass]
    public class FloorTests {
        private Floor floor;
        
        [OneTimeSetUp]
        public void Init() {
            floor = new Floor();
        }

        [Test]
        public void IsSolid_ReturnFalse() {
            // Arrange.

            // Act.
            bool actualResult = floor.IsSolid();

            // Assert.
            NUnit.Framework.Assert.AreEqual(false, actualResult);
        }
    }
}
