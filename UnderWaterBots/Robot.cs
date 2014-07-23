using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnderWaterBots {
    public class Robot {
        public uint Id { get; private set; }
        // defaults to false upon construction
        public bool Deployed { get; set; }
        // While deployed, current coords.
        // While not deployed, intended deployment coords.
        public int X { get; set; }
        public int Y { get; set; }
        // Corresponds to a cardinal compass point
        public char Heading { get; set; }
        public Char[] directions;

        public Robot(uint arg) {
            this.Id = arg;
            this.Deployed = false;
        }

        // Validates and initilizes either start position or movement directions
        // The excepted order of the directions is start position followed by movement
        public bool newInstructions(string input) {
            // A valid start position is of format: # # A where A must be a letter corresponding to one of the cardinal points.
            if (Regex.IsMatch(input, @"^-?[0-9]+\s-?[0-9]+\s[NWSEnwse]$")) {
                setStartPosition(input);
                return false;
            }
            // Valid directions include any combination of the following letters: are L for left, R for right, and M for move forward.
            else if (Regex.IsMatch(input, @"^[LRMlrm]*$")) {
                storeDirections(input);
                return true;
            }
            else {
                throw new Exception("Robot start position or movement instructions are not formatted correctly.\n");
            }
        }

        private void setStartPosition(string input) {
            string[] split = input.Split(new Char[] { ' ' });
            this.X = Convert.ToInt32(split[0]);
            this.Y = Convert.ToInt32(split[1]);
            this.Heading = Convert.ToChar(split[2]);
        }

        private void storeDirections(string input) {
            this.directions = input.ToCharArray();
        }
    }
}
