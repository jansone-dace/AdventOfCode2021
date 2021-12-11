﻿using System.IO;

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

        static int Day3_1()
        {
            string[] inputBinaryNumbers;
            string gammaRate = "", epsilonRate = "";
            int zeroCount, onesCount;

            // Read binary numbers from input file 
            inputBinaryNumbers = File.ReadAllLines(@".\inputs\day3.txt");

            // Each bit in the gamma rate can be determined by finding the most common bit 
            // in the corresponding position of all numbers in the diagnostic report.

            // (we assume correct input for everything, but in this puzzle there are many things that could be checked)

            // First read by columns 
            for (int column = 0; column < inputBinaryNumbers[0].Length; column++)
            {
                // Start new count
                zeroCount = onesCount = 0;

                // Then read by rows
                for (int row = 0; row < inputBinaryNumbers.Length; row++)
                {
                    // Count how many zeros and ones are in each column
                    if (inputBinaryNumbers[row][column] == '0')
                        zeroCount++;
                    else
                        onesCount++;
                }

                // See which bit was most common and build gamma rate
                if (zeroCount > onesCount)
                    gammaRate += "0";
                else
                    gammaRate += "1";
            }

            // The epsilon rate is calculated in a similar way; rather than use the most common bit, 
            // the least common bit from each position is used.

            // Invert gamma rate to get the epsilon rate
            for (int i = 0; i < gammaRate.Length; i++)
            {
                if (gammaRate[i] == '0')
                    epsilonRate += "1";
                else
                    epsilonRate += "0";
            }

            // Multiply decimal values of gamma rate and epsilon rate and return the value
            return Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);
        }

        static int Day3_2()
        {
            string[] inputBinaryNumbers;
            List<string> oxygenBinaryNumbers, scrubberBinaryNumbers;
            int zeroCount, onesCount;

            // Read binary numbers from input file into both lists
            oxygenBinaryNumbers = scrubberBinaryNumbers = File.ReadAllLines(@".\inputs\day3.txt").ToList();

            // - Keep only numbers selected by the bit criteria for the type of rating value for which you are searching. 
            //   Discard numbers which do not match the bit criteria.
            // - If you only have one number left, stop; this is the rating value for which you are searching.
            // - Otherwise, repeat the process, considering the next bit to the right.

            // - To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position, 
            //   and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 1 in the position being considered.
            // - To find CO2 scrubber rating, determine the least common value (0 or 1) in the current bit position, 
            //   and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 0 in the position being considered.


            // First - find oxygen generator rating
            // Start reading input by columns from left
            // TODO: make it a function
            while (oxygenBinaryNumbers.Count > 1)
            {
                for (int column = 0; column < oxygenBinaryNumbers[0].Length; column++)
                {
                    // Start new count
                    zeroCount = onesCount = 0;

                    // Then read one digit from each row
                    for (int row = 0; row < oxygenBinaryNumbers.Count; row++)
                    {
                        // Count how many zeros and ones are in each column
                        if (oxygenBinaryNumbers[row][column] == '0')
                            zeroCount++;
                        else
                            onesCount++;
                    }

                    // See which bit was most and least common and filter the list
                    if (onesCount > zeroCount)
                    {
                        // If there are more 1s than 0s, then for oxygen generator rating keep all the numbers with 1 in current position
                        if (oxygenBinaryNumbers.Count > 1)
                            oxygenBinaryNumbers = oxygenBinaryNumbers.Where(val => val[column] == '1').ToList();
                    }
                    else if (onesCount < zeroCount)
                    {
                        // If there are more 0s than 1s, then for oxygen generator rating keep all the numbers with 0 in current position
                        if (oxygenBinaryNumbers.Count > 1)
                            oxygenBinaryNumbers = oxygenBinaryNumbers.Where(val => val[column] == '0').ToList();
                    }
                    else
                    {
                        // If there are equal amount of 1s and 0s, then for oxygen generator rating keep all the number with 1 in current postion
                        if (oxygenBinaryNumbers.Count > 1)
                            oxygenBinaryNumbers = oxygenBinaryNumbers.Where(val => val[column] == '1').ToList();
                    }
                }
            }

            // Then find the CO2 scrubber rating
            // Start reading input by columns from left
            while (scrubberBinaryNumbers.Count > 1)
            {
                for (int column = 0; column < scrubberBinaryNumbers[0].Length; column++)
                {
                    // Start new count
                    zeroCount = onesCount = 0;

                    // Then read one digit from each row
                    for (int row = 0; row < scrubberBinaryNumbers.Count; row++)
                    {
                        // Count how many zeros and ones are in each column
                        if (scrubberBinaryNumbers[row][column] == '0')
                            zeroCount++;
                        else
                            onesCount++;
                    }

                    // See which bit was most and least common and filter the list
                    if (onesCount > zeroCount)
                    {
                        // If there are more 1s than 0s, then for CO2 scrubber rating keep all the numbers with 0 in current position
                        if (scrubberBinaryNumbers.Count > 1)
                            scrubberBinaryNumbers = scrubberBinaryNumbers.Where(val => val[column] == '0').ToList();
                    }
                    else if (onesCount < zeroCount)
                    {
                        // If there are more 0s than 1s, then for CO2 scrubber rating keep all the numbers with 1 in current position
                        if (scrubberBinaryNumbers.Count > 1)
                            scrubberBinaryNumbers = scrubberBinaryNumbers.Where(val => val[column] == '1').ToList();
                    }
                    else
                    {
                        // If there are equal amount of 1s and 0s, then for CO2 scrubber rating keep all the numbers with 0 in current position
                        if (scrubberBinaryNumbers.Count > 1)
                            scrubberBinaryNumbers = scrubberBinaryNumbers.Where(val => val[column] == '0').ToList();
                    }
                }
            }

            // Finally, to find the life support rating, multiply the oxygen generator rating (decimal) by the CO2 scrubber rating (decimal)
            return Convert.ToInt32(oxygenBinaryNumbers[0], 2) * Convert.ToInt32(scrubberBinaryNumbers[0], 2);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Day 1 part 1: {0}", Day1_1()));
            Console.WriteLine(String.Format("Day 1 part 2: {0}", Day1_2()));
            Console.WriteLine(string.Format("Day 2 part 1: {0}", Day2_1()));
            Console.WriteLine(string.Format("Day 2 part 2: {0}", Day2_2()));
            Console.WriteLine(string.Format("Day 3 part 1: {0}", Day3_1()));
            Console.WriteLine(string.Format("Day 3 part 2: {0}", Day3_2()));
        }
    }
}