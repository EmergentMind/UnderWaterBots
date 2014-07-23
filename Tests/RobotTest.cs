using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnderWaterBots;

namespace Tests {
    [TestFixture]
    class RobotTest {
        Robot robot = new Robot(0);

        [Test]
        public void canInitialize() {
            Assert.AreEqual(0, robot.Id);
            Assert.IsFalse(robot.Deployed);
        }

        [Test]
        public void acceptsGoodStartPosition1() {
            string input = "1 2 N";

            robot.newInstructions(input);

            Assert.AreEqual(1, robot.X);
            Assert.AreEqual(2, robot.Y);
            Assert.AreEqual('N', robot.Heading);
        }

        [Test]
        public void acceptsGoodStartPosition2() {
            string input = "-1 2 N";

            robot.newInstructions(input);

            Assert.AreEqual(-1, robot.X);
            Assert.AreEqual(2, robot.Y);
            Assert.AreEqual('N', robot.Heading);
        }

        [Test]
        public void acceptsGoodStartPosition3() {
            string input = "1 -2 N";

            robot.newInstructions(input);

            Assert.AreEqual(1, robot.X);
            Assert.AreEqual(-2, robot.Y);
            Assert.AreEqual('N', robot.Heading);
        }

        [Test]
        public void acceptsGoodStartPosition4() {
            string input = "1 2 E";

            robot.newInstructions(input);

            Assert.AreEqual(1, robot.X);
            Assert.AreEqual(2, robot.Y);
            Assert.AreEqual('E', robot.Heading);
        }

        [Test]
        public void acceptsGoodStartPosition5() {
            string input = "1 2 S";

            robot.newInstructions(input);

            Assert.AreEqual(1, robot.X);
            Assert.AreEqual(2, robot.Y);
            Assert.AreEqual('S', robot.Heading);
        }

        [Test]
        public void acceptsGoodStartPosition6() {
            string input = "1 2 W";

            robot.newInstructions(input);

            Assert.AreEqual(1, robot.X);
            Assert.AreEqual(2, robot.Y);
            Assert.AreEqual('W', robot.Heading);
        }


        [Test]
        [ExpectedException]
        public void rejectsBadStartPosition1() {
            string input = "3 7 Q";
            robot.newInstructions(input);
        }

        [Test]
        [ExpectedException]
        public void rejectsBadStartPosition2() {
            string input = "3 Q1";
            robot.newInstructions(input);
        }

        [Test]
        [ExpectedException]
        public void rejectsBadStartPosition3() {
            string input = "3, 7 N";
            robot.newInstructions(input);
        }

        [Test]
        public void acceptsGoodDirections() {
            string input = "MLR";

            robot.newInstructions(input);

            Assert.AreEqual(3, robot.directions.Length);
        }

        [Test]
        public void acceptsNoDirections() {
            string input = "";

            robot.newInstructions(input);

            Assert.AreEqual(0, robot.directions.Length);
        }

        [Test]
        [ExpectedException]
        public void rejectsBadDirections() {
            string input = "MLRG";
            robot.newInstructions(input);
        }
    }
}
