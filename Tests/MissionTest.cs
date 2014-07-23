using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using NUnit.Framework;
using UnderWaterBots;

namespace Tests {

    [TestFixture]
    public class MissionTest {

        [Test]
        public void canInitialize() {
            Mission testMission = new Mission();

            Assert.AreEqual(0, testMission.grid.Right);
            Assert.AreEqual(0, testMission.grid.Top);
            Assert.AreEqual(0, testMission.grid.Left);
            Assert.AreEqual(0, testMission.grid.Bottom);
            Assert.AreEqual(0, testMission.botQty);
            Assert.IsNull(testMission.bots);
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorAcceptsGoodInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/goodBasic.txt");

            Assert.AreEqual(2, testMission.grid.Right);
            Assert.AreEqual(2, testMission.grid.Top);
            Assert.AreEqual(-2, testMission.grid.Left);
            Assert.AreEqual(-2, testMission.grid.Bottom);
            Assert.AreEqual(4, testMission.botQty);
            Assert.AreEqual(4, testMission.bots.Length);
            Assert.AreEqual(3, testMission.bots[0].directions.Length);
            Assert.IsTrue(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorAcceptsGoodInputWithNoDirections1() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/goodNoDirections1.txt");

            Assert.AreEqual(2, testMission.botQty);
            Assert.AreEqual(2, testMission.bots.Length);
            Assert.IsTrue(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorAcceptsGoodInputWithNoDirections2() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/goodNoDirections2.txt");

            Assert.AreEqual(2, testMission.botQty);
            Assert.AreEqual(2, testMission.bots.Length);
            Assert.IsTrue(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotDirectionsInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotDirections.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotQuantityIsExcessive() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotQuantityIsExcessive.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotQuantityIsHighInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotQuantityIsHigh.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotQuantityIsLowInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotQuantityIsLow.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadNoBotQuantity() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badNoBotQuantity.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadNegativeBotQuantity() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badNegativeBotQuantity.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotQuantity() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotQuantity.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadBotStartPositionInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badBotStartPosition.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void inputProcessorRejectsBadGridInput() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/badGrid.txt");
            Assert.IsFalse(testMission.DataIsValid);
        }

        [Test]
        public void deploysBot() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/goodBasic.txt");
            testMission.execute();

            Assert.IsTrue(testMission.bots[0].Deployed);
        }

        [Test]
        public void detectsOutOfBoundsDeployment() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/outOfBoundsDeployment.txt");
            testMission.execute();

            Assert.IsFalse(testMission.bots[1].Deployed);
            Assert.IsFalse(testMission.bots[2].Deployed);
            Assert.IsFalse(testMission.bots[3].Deployed);
        }

        [Test]
        public void performsBasicMovement() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/goodBasic.txt");
            testMission.execute();

            Assert.AreEqual(0, testMission.bots[0].X);
            Assert.AreEqual(2, testMission.bots[0].Y);
            Assert.AreEqual('N', testMission.bots[0].Heading);
            Assert.AreEqual(2, testMission.bots[1].X);
            Assert.AreEqual(0, testMission.bots[1].Y);
            Assert.AreEqual('N', testMission.bots[1].Heading);
            Assert.AreEqual(0, testMission.bots[2].X);
            Assert.AreEqual(-2, testMission.bots[2].Y);
            Assert.AreEqual('W', testMission.bots[2].Heading);
            Assert.AreEqual(-2, testMission.bots[3].X);
            Assert.AreEqual(0, testMission.bots[3].Y);
            Assert.AreEqual('W', testMission.bots[3].Heading);
        }

        [Test]
        public void handlesBotCollision() {
            Mission testMission = new Mission();
            testMission.processInstructionSet("../../TestInputs/movementCollisions.txt");
            testMission.execute();

            Assert.AreEqual(1, testMission.bots[0].X);
            Assert.AreEqual(2, testMission.bots[1].X);
            Assert.AreEqual(-2, testMission.bots[2].X);
        }

        [Test]
        public void reportTest() {

        }
    }
}
