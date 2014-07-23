using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Logger;

namespace UnderWaterBots {

    public class GridSpace {
        public MissionLog missionLog = new MissionLog();

        public int Top { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }
        public int Left { get; private set; }

        public void setCoordinatesFromString(string input) {
            // The string of input must follow the coordinate format: #, #
            // The values of the supplied digits must be greater than zero.
            if (Regex.IsMatch(input, @"^[1-9]\d*,\s[1-9]\d*$")) {
                MatchCollection matches = Regex.Matches(input, @"[1-9]\d*");
                this.Right = Convert.ToInt32(matches[0].Value);
                this.Top = Convert.ToInt32(matches[1].Value);
                this.Left = this.Right * -1;
                this.Bottom = this.Top * -1;
            }
            else {
                throw new Exception("Failed to setCoordinatesFromString(): Upper right coordinates must be positive values in the format: #, # (e.g. \"3, 7\").\n");
            }
        }

        public bool isInBounds(int x, int y) {
            if (isInsideOuterBoundary(x, y) && isOutsideInnerBoundary(x, y)) {
                return true;
            }
            else {
                return false;
            }
        }

        public uint maxPossibleBots() {
            // calc grid dimensions accouting for 0 as a valid x or y position
            int width = this.Right * 2 + 1;
            int height = this.Top * 2 + 1;

            // Grid area minus 1 to account for center being off limits
            uint max = Convert.ToUInt32(width * height - 1);

            return max;
        }

        private bool isInsideOuterBoundary(int x, int y) {
            if (x <= this.Right &&
                y <= this.Top &&
                x >= this.Left &&
                y >= this.Bottom) {

                return true;
            }
            else {
                return false;	         
	        }
        }

        private bool isOutsideInnerBoundary(int x, int y) {
            if (x == 0 && y == 0) {
                return false;
            }
            else {
                return true;
            }
       }
    };
}
