using System.IO;

namespace AdventOfCode2021
{
    class Program
    {
        static int Day1_1()
        {
            string[] depthMeasurements;
            int previuosDepthMeasurement, currentDepthMeasurement, increaseCount = 0;

            depthMeasurements = File.ReadAllLines(@".\inputs\day1.txt");
            
            if (depthMeasurements.Length == 0)
                return 0;
            
            previuosDepthMeasurement = Int32.Parse(depthMeasurements[0]);
            for (int cur = 1; cur < depthMeasurements.Length; cur++)
            {
                currentDepthMeasurement = Int32.Parse(depthMeasurements[cur]);
                if (currentDepthMeasurement > previuosDepthMeasurement)
                    increaseCount++;
                previuosDepthMeasurement = currentDepthMeasurement;
            }

            return increaseCount;
        }

        static int Day1_2()
        {
            string[] depthMeasurements;
            int currentMeasurementWindow, previuosMeasurementWindow, increaseCount = 0;

            depthMeasurements = File.ReadAllLines(@".\inputs\day1.txt");
            
            if (depthMeasurements.Length < 3)
                return 0;
            
            previuosMeasurementWindow = Int32.Parse(depthMeasurements[0]) + Int32.Parse(depthMeasurements[1]) + Int32.Parse(depthMeasurements[2]);
            for (int cur = 1; cur < depthMeasurements.Length - 2; cur++)
            {
                currentMeasurementWindow = Int32.Parse(depthMeasurements[cur]) + Int32.Parse(depthMeasurements[cur+1]) + Int32.Parse(depthMeasurements[cur+2]);
                if (currentMeasurementWindow > previuosMeasurementWindow)
                    increaseCount++;
                previuosMeasurementWindow = currentMeasurementWindow;
            }

            return increaseCount;
        }

        static int Day2_1()
        {
            string[] commands;
            string commandType;
            int horizontalPosition = 0, verticalPosition = 0, commandNumericalValue;

            // Read commands from input file 
            commands = File.ReadAllLines(@".\inputs\day2.txt");

            // Iterate through each command
            foreach (string command in commands)
            {
                // From each command get the command type and numerical value
                commandType = command.Split(' ')[0];
                commandNumericalValue = Int32.Parse(command.Split(' ')[1]);

                switch (commandType)
                {
                    case "up":
                        // up X decreases the depth by X units
                        verticalPosition -= commandNumericalValue;
                        break;
                    case "down":
                        // down X increases the depth by X units
                        verticalPosition += commandNumericalValue;
                        break;
                    case "forward":
                        // forward X increases the horizontal position by X units
                        horizontalPosition += commandNumericalValue;
                        break;
                    default:
                        break;
                }
            }

            // Multiply horizontal and vertical positions and return it as a result
            return horizontalPosition * verticalPosition;
        }

        static int Day2_2()
        {
            string[] commands;
            string commandType;
            int horizontalPosition = 0, aim = 0, depth = 0, commandNumericalValue;

            // Read commands from input file 
            commands = File.ReadAllLines(@".\inputs\day2.txt");

            // Iterate through each command
            foreach (string command in commands)
            {
                // From each command get the command type and numerical value
                commandType = command.Split(' ')[0];
                commandNumericalValue = Int32.Parse(command.Split(' ')[1]);

                switch (commandType)
                {
                    case "up":
                        // up X decreases your aim by X units
                        aim -= commandNumericalValue;
                        break;
                    case "down":
                        // down X increases your aim by X units
                        aim += commandNumericalValue;
                        break;
                    case "forward":
                        // forward X does two things:
                        //   It increases your horizontal position by X units.
                        horizontalPosition += commandNumericalValue;
                        //   It increases your depth by your aim multiplied by X.
                        depth += (aim * commandNumericalValue);
                        break;
                    default:
                        break;
                }
            }

            // What do you get if you multiply your final horizontal position by your final depth?
            return horizontalPosition * depth;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Day 1 part 1: {0}", Day1_1()));
            Console.WriteLine(String.Format("Day 1 part 2: {0}", Day1_2()));
            Console.WriteLine(string.Format("Day 2 part 1: {0}", Day2_1()));
            Console.WriteLine(string.Format("Day 2 part 2: {0}", Day2_2()));
        }
    }
}