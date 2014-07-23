using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace UnderWaterBots {
    class MainApp {
        static void Main(string[] args) {
            try {
                if (args.Any()) {
                    if (File.Exists(args[0])) {
                        Mission mission = new Mission();

                        mission.processInstructionSet(args[0]);
                        mission.execute();
                        mission.reportPositions();
                    }
                    else {
                        throw new Exception("Input Error: File path not found.\n");
                    }
                }
                else {
                    throw new Exception("Input Error: You must provide the path of your instructions file.\n Usage: UnderWaterBots.exe <file>\n");
                }
            }
            catch (Exception e) {
                Console.WriteLine("{0}", e);
            }
        }
    }
 }
