using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger {
    public class MissionLog {
        private string fileName;

        public MissionLog() {
            fileName = "log.txt";
        }

        public MissionLog(string fileName) {
            this.fileName = fileName;
        }

        public void newMission() {
            string logString = "\nNEW MISSION STARTED    \n";
            addEntry(logString);
         }

        public void addEntry(string logEntry) {
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append))) {
                writer.WriteLine("{0}", logEntry);
            }
        }
    }
}