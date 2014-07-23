using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnderWaterBots;

namespace Tests {
    [TestFixture]
    class GridSpaceTest {
        Mission testMission = new Mission();

        [Test]
        public void canSetupValidGrid() {
            string input = "1, 1";
            testMission.grid.setCoordinatesFromString(input);

            Assert.AreEqual(1, testMission.grid.Right);
            Assert.AreEqual(1, testMission.grid.Top);
            Assert.AreEqual(-1, testMission.grid.Left);
            Assert.AreEqual(-1, testMission.grid.Bottom);
        }

        [Test]
        [ExpectedException]
        public void rejectsInvalidGridSetupOnX() {
            string input = "-1, 1";

            testMission.grid.setCoordinatesFromString(input);
        }

        [Test]
        [ExpectedException]
        public void rejectsInvalidGridSetupOnY() {
            string input = "1, -1";

            testMission.grid.setCoordinatesFromString(input);
        }

        [Test]
        [ExpectedException]
        public void rejectsGridSetupWithZeroDimensions() {
            string input = "0, 0";

            testMission.grid.setCoordinatesFromString(input);
        }
        [Test]
        public void detectsBoundaryCheckInBounds() {
            string gridSize = "2, 2";
            testMission.grid.setCoordinatesFromString(gridSize);

            bool value = testMission.grid.isInBounds(2, 0);
            Assert.That(value, Is.True);

            value = testMission.grid.isInBounds(0, 2);
            Assert.That(value, Is.True);

            value = testMission.grid.isInBounds(-2, 0);
            Assert.That(value, Is.True);

            value = testMission.grid.isInBounds(0, -2);
            Assert.That(value, Is.True);
        }

        [Test]
        public void detectsBoundaryCheckOfBounds() {
            string gridSize = "2, 2";
            testMission.grid.setCoordinatesFromString(gridSize);

            bool value = testMission.grid.isInBounds(3, 0);
            Assert.That(value, Is.False);

            value = testMission.grid.isInBounds(0, 3);
            Assert.That(value, Is.False);

            value = testMission.grid.isInBounds(-3, 0);
            Assert.That(value, Is.False);

            value = testMission.grid.isInBounds(0, -3);
            Assert.That(value, Is.False);
        }

        [Test]
        public void detectsBoundaryCheckAtCenter() {
            string gridSize = "2, 2";
            testMission.grid.setCoordinatesFromString(gridSize);

            bool value = testMission.grid.isInBounds(0, 0);
            Assert.That(value, Is.False);
        }

        [Test]
        public void calcsMaxBots() {
            string gridSize = "1, 1";
            testMission.grid.setCoordinatesFromString(gridSize);

            uint value = testMission.grid.maxPossibleBots();
            Assert.AreEqual(8, value);

            gridSize = "2, 2";
            testMission.grid.setCoordinatesFromString(gridSize);

            value = testMission.grid.maxPossibleBots();
            Assert.AreEqual(24, value);
        }
    }
}
