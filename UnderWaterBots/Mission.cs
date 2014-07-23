using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Logger;

namespace UnderWaterBots {
    public class Mission {
        public MissionLog missionLog = new MissionLog();
        public bool DataIsValid { get; private set; }
        public GridSpace grid = new GridSpace();
        public uint botQty;
        public Robot[] bots;

        public Mission() {
            missionLog.newMission();
        }

        private void invalidData(Exception e, uint line) {
            this.DataIsValid = false;

            string logString = "Validation Error: Invalid input data at Line " + line + ". " + e + "\n";
            missionLog.addEntry(logString);

            Console.WriteLine("{0}", logString);
        }

        public void processInstructionSet(string input) {
            this.DataIsValid = true;

            uint lineCounter = 1;
            string curLine;
            uint curBot = 0;
            StreamReader data = new StreamReader(input);

            while ((curLine = data.ReadLine()) != null && DataIsValid) {
                curLine = curLine.Trim();

                switch (lineCounter) {
                    // First line must be the upper right grid coordinates
                    case 1:
                        try {
                            grid.setCoordinatesFromString(curLine);
                        }
                        catch (Exception e) {
                            this.invalidData(e, lineCounter);
                        }
                        break;
                    // Second line must be the quantity of robots in the mission
                    case 2:
                        try {
                            uint qty = Convert.ToUInt32(curLine);
                            // Prevent overflow
                            if (qty <= grid.maxPossibleBots()) {
                                botQty = qty;
                                this.initializeBotArray();
                            }
                            else {
                                throw new Exception("The specified number of bots is greater than allowed by the area of the grid.");
                            }
                        }
                        catch (Exception e) {
                            this.invalidData(e, lineCounter);
                        }
                        break;
                    // Lines of data subsequent to lines 1 and 2 of the input should be either a bot start position or bot directions
                    default:
                         try {
                            // Don't process line pairs in excess of botQty
                            if (lineCounter - 2 <= botQty * 2) {
                                // bot instructions are handled in order. two lines of instruction per bot.
                                // expect bots[].newInstructions() to return true if two lines have been processed for curBot 
                                bool botComplete = bots[curBot].newInstructions(curLine);
                                if (botComplete) {
                                    curBot++;
                                }
                            }
                            else {
                                throw new Exception("Too many lines of input data for the specified number of robots.\n");
                            }
                         }
                         catch (Exception e) {
                            this.invalidData(e, lineCounter);
                         }
                        break;
                }
                lineCounter++;
            }
            // The actual input was valid, but make sure there are enough instructions for the number of specified bots.
            try {
                if (lineCounter - 2 < botQty * 2) {
                    throw new Exception("There are too few lines of input data for the specified number of robots.\n");
                }
            }
            catch (Exception e) {
                this.invalidData(e, lineCounter);
            }
        }

        private void initializeBotArray() {
            bots = new Robot[botQty];
            for (uint i = 0; i < bots.Length; i++) {
                bots[i] = new Robot(i + 1);
            }
        }

        public void execute() {
            if (DataIsValid) {
                foreach (Robot curBot in bots) {
                    try {
                        this.activateRobot(curBot);
                    }
                    catch (Exception e) {
                        string logString = "Robot " + curBot.Id + " - " + e + ".\n";
                        missionLog.addEntry(logString);
                    }
                }
            }
        }

        private void activateRobot(Robot curBot) {
            if (curBot.Deployed == false) {
                this.deploy(curBot);
            }
            foreach (char step in curBot.directions) {
                this.followCurrentStepOfDirections(curBot, step);
            }
        }

        private void deploy(Robot curBot) {
            if (isValidMove(curBot.X, curBot.Y)) {
                curBot.Deployed = true;
            }
            else {
                string logString = "Robot " + curBot.Id + " not deployed: Coords are out of bounds or collide with a previously activated robot. Continuing to next bot, if applicable.\n";
                missionLog.addEntry(logString);
            }
        }

        private void followCurrentStepOfDirections(Robot curBot, char step) {
            if (step == 'M') {
                int[] proposedCoords = calcNewCoords(curBot.X, curBot.Y, curBot.Heading);

                if (isValidMove(proposedCoords[0], proposedCoords[1])) {
                    curBot.X = proposedCoords[0];
                    curBot.Y = proposedCoords[1];
                }
                else {
                    string logString = "Robot " + curBot.Id + " movement step ignored: Results in collision or go out of bounds. Continuing to next step, if applicable.\n";
                    missionLog.addEntry(logString);
                }
            }
            else {
                curBot.Heading = determineNewFacing(step, curBot.Heading);
            }
        }

        private bool isValidMove(int x, int y) {
            if (grid.isInBounds(x, y) && noCollisions(x, y)) {
                return true;
            }
            return false;
        }

        private int[] calcNewCoords(int x, int y, char heading) {
            int[] newCoords = new int[] { x, y };
            if (heading == 'N') {
                newCoords[1] = y + 1;
            }
            else if (heading == 'E') {
                newCoords[0] = x + 1;
            }
            else if (heading == 'S') {
                newCoords[1] = y - 1;
            }
            else if (heading == 'W') {
                newCoords[0] = x - 1;
            }
            return newCoords;
        }

        private char determineNewFacing(char turn, char curFacing) {
            switch (curFacing) {
                case 'N':
                    if (turn == 'L')
                        return 'W';
                    else
                        return 'E';
                case 'E':
                    if (turn == 'L')
                        return 'N';
                    else
                        return 'S';
                case 'S':
                    if (turn == 'L')
                        return 'E';
                    else
                        return 'W';
                case 'W':
                    if (turn == 'L')
                        return 'S';
                    else
                        return 'N';

                default:
                    return curFacing;
            }
        }

        private bool noCollisions(int x, int y) {
            foreach (Robot curBot in bots) {
                if (curBot.Deployed) {
                    if (curBot.X == x && curBot.Y == y) {
                        return false;
                    }
                }
            }
            return true;
        }

        public void reportPositions() {
            if (DataIsValid) {
                foreach (Robot curBot in bots) {
                    string logString = "Robot " + curBot.Id + " final position: " + curBot.X + " " + curBot.Y + " " + curBot.Heading + "\n";
                    missionLog.addEntry(logString);

                    Console.WriteLine("{0} {1} {2}", curBot.X, curBot.Y, curBot.Heading);
                }
            }
        }
    }
}
