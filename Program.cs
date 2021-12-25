using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        // These two functions are for element comparison in list
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            Point p = (Point)obj;
            return (p.X == X && p.Y == Y);
        }

        public override int GetHashCode()
        {
            return String.Format("{0},{1}", X, Y).GetHashCode();
        }
    }

    class Line
    {
        public Line(int fromX, int fromY, int toX, int toY)
        {
            from = new Point(fromX, fromY);
            to = new Point(toX, toY);
        }

        public Point from { get; set; }
        public Point to { get; set; }

        public bool IsHorizontalOrVertical()
        {
            return from.X == to.X || from.Y == to.Y;
        }

        public List<Point> GetPoints()
        {
            List<Point> result = new List<Point>();
            Point currentPoint;
            int directionX, directionY, currentX, currentY;

            if (from.X == to.X)
                directionX = 0;
            else if (from.X < to.X)
                directionX = 1;
            else
                directionX = -1;
            
            if (from.Y == to.Y)
                directionY = 0;
            else if (from.Y < to.Y)
                directionY = 1;
            else
                directionY = -1;

            currentX = from.X;
            currentY = from.Y;

            while (currentX != to.X || currentY != to.Y)
            {
                // Add point to the list
                currentPoint = new Point(currentX, currentY);
                result.Add(currentPoint);
                
                // Look for next point
                currentX += directionX;
                currentY += directionY;
            }

            // Add final point to the list
            result.Add(to);

            return result;
        }
    }

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

        static int Day4_1()
        {
            string[] bingoNumbers, columnValues;
            List<List<string>> boards = new List<List<string>>();
            List<string> boardValues;
            int columnStartIndex, rowStartIndex, sumOfUnmarkedNumbers;

            void addBoardValues(ref List<string> board, string numberLine)
            {
                // Get values for current column
                columnValues = numberLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // Add values to the board
                board.Add(columnValues[0]);
                board.Add(columnValues[1]);
                board.Add(columnValues[2]);
                board.Add(columnValues[3]);
                board.Add(columnValues[4]);
            }
            
            // Read input file line by line
            var lines = File.ReadLines(@".\inputs\day4.txt").ToList();
            // First line is bingo numbers
            bingoNumbers = lines[0].Split(',');

            // Start with index 2 because 0 was bingo numbers and 1 is empty line
            // i+=6, because 5 rows for board and then one empty line
            for (int i = 2; i < lines.Count; i += 6)
            {
                // Create new board and fill with values
                 boardValues = new List<string>();
             
                addBoardValues(ref boardValues, lines[i]); // Column 1
                addBoardValues(ref boardValues, lines[i+1]); // Column 2
                addBoardValues(ref boardValues, lines[i+2]); // Column 3
                addBoardValues(ref boardValues, lines[i+3]); // Column 4
                addBoardValues(ref boardValues, lines[i+4]); // Column 5

                // Add board to the boards list
                boards.Add(boardValues);
            }

            // Now go through every bingo number
            foreach (var number in bingoNumbers)
            {
                // Look for that number in boards
                foreach (var board in boards)
                {
                    for (int field = 0; field < board.Count; field++)
                    {
                        // When found
                        if (board[field] == number)
                        {
                            // mark that field
                            board[field] = "*";

                            // see if this board has a bingo (look only at the column and row of marked field)
                            // find start index for current column and row
                            columnStartIndex = field - (field % 5);
                            rowStartIndex = field % 5;
                            
                            // look for a bingo
                            if ((board[columnStartIndex] == "*"
                                 && board[columnStartIndex+1] == "*"
                                 && board[columnStartIndex+2] == "*"
                                 && board[columnStartIndex+3] == "*"
                                 && board[columnStartIndex+4] == "*")
                            || (board[rowStartIndex] == "*"
                                && board[rowStartIndex+5] == "*"
                                && board[rowStartIndex+10] == "*"
                                && board[rowStartIndex+15] == "*"
                                && board[rowStartIndex+20] == "*"))
                            {
                                // Find sum of all unmarked numbers from winning board
                                sumOfUnmarkedNumbers = 0;
                                foreach (var boardNumber in board)
                                {
                                    if (boardNumber != "*")
                                        sumOfUnmarkedNumbers += Int32.Parse(boardNumber);
                                }

                                // Return the score - sum of all unmarked numbers mulitplied by last drawn number
                                return sumOfUnmarkedNumbers * Int32.Parse(number);
                            }
                        }
                    }
                }
            }

            return 0;
        }

        static int Day4_2()
        {
            string[] bingoNumbers, columnValues;
            List<List<string>> boards = new List<List<string>>();
            List<string> boardValues;
            int columnStartIndex, rowStartIndex, sumOfUnmarkedNumbers;

            void addBoardValues(ref List<string> board, string numberLine)
            {
                // Get values for current column
                columnValues = numberLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // Add values to the board
                board.Add(columnValues[0]);
                board.Add(columnValues[1]);
                board.Add(columnValues[2]);
                board.Add(columnValues[3]);
                board.Add(columnValues[4]);
            }
            
            // Read input file line by line
            var lines = File.ReadLines(@".\inputs\day4.txt").ToList();
            // First line is bingo numbers
            bingoNumbers = lines[0].Split(',');

            // Start with index 2 because 0 was bingo numbers and 1 is empty line
            // i+=6, because 5 rows for board and then one empty line
            for (int i = 2; i < lines.Count; i += 6)
            {
                // Create new board and fill with values
                 boardValues = new List<string>();
             
                addBoardValues(ref boardValues, lines[i]); // Column 1
                addBoardValues(ref boardValues, lines[i+1]); // Column 2
                addBoardValues(ref boardValues, lines[i+2]); // Column 3
                addBoardValues(ref boardValues, lines[i+3]); // Column 4
                addBoardValues(ref boardValues, lines[i+4]); // Column 5

                // Add board to the boards list
                boards.Add(boardValues);
            }

            // Now go through every bingo number
            foreach (var number in bingoNumbers)
            {
                // Look for that number in boards
                // add ".ToList()", so elements can be removed while iterating
                foreach (var board in boards.ToList())
                {
                    for (int field = 0; field < board.Count; field++)
                    {
                        // When found
                        if (board[field] == number)
                        {
                            // mark that field
                            board[field] = "*";

                            // see if this board has a bingo (look only at the column and row of marked field)
                            // find start index for current column and row
                            columnStartIndex = field - (field % 5);
                            rowStartIndex = field % 5;
                            
                            // look for a bingo
                            if ((board[columnStartIndex] == "*"
                                 && board[columnStartIndex+1] == "*"
                                 && board[columnStartIndex+2] == "*"
                                 && board[columnStartIndex+3] == "*"
                                 && board[columnStartIndex+4] == "*")
                            || (board[rowStartIndex] == "*"
                                && board[rowStartIndex+5] == "*"
                                && board[rowStartIndex+10] == "*"
                                && board[rowStartIndex+15] == "*"
                                && board[rowStartIndex+20] == "*"))
                            {
                                // If the board won and it is not the last board, remove it from boards list
                                // otherwise this was the last board to win, so calculate its score
                                if (boards.Count > 1)
                                {
                                    boards.Remove(board);
                                    continue;
                                }
                                else
                                {
                                    // Find sum of all unmarked numbers from winning board
                                    sumOfUnmarkedNumbers = 0;
                                    foreach (var boardNumber in board)
                                    {
                                        if (boardNumber != "*")
                                            sumOfUnmarkedNumbers += Int32.Parse(boardNumber);
                                    }

                                    // Return the score - sum of all unmarked numbers mulitplied by last drawn number
                                    return sumOfUnmarkedNumbers * Int32.Parse(number);
                                }
                            }
                        }
                    }
                }
            }

            return 0;
        }

        static int Day5_1()
        {
            int fromX, fromY, toX, toY;
            Match coordinatesMatch;
            Line hydrothermalVentLine;
            List<Line> hydrothermalVentLineList = new List<Line>();
            List<Point> line1Points, line2Points;
            // Use set for intersection points so there are only unique points
            HashSet<Point> intersectionPoints = new HashSet<Point>();

            // Read input file line by line
            var inputLines = File.ReadLines(@".\inputs\day5.txt").ToList();
            foreach (var line in inputLines)
            {
                // Get all the coordinates from line
                coordinatesMatch = Regex.Match(line, @"(?<fromX>[0-9]+),(?<fromY>[0-9]+)\s\->\s(?<toX>[0-9]+),(?<toY>[0-9]+)");
                fromX = Int32.Parse(coordinatesMatch.Groups["fromX"].Value);
                fromY = Int32.Parse(coordinatesMatch.Groups["fromY"].Value);
                toX = Int32.Parse(coordinatesMatch.Groups["toX"].Value);
                toY = Int32.Parse(coordinatesMatch.Groups["toY"].Value);

                hydrothermalVentLine = new Line(fromX, fromY, toX, toY);

                // For now, we only consider horizontal and vertical lines
                // So only add horizontal or vertical lines to the line list
                if (hydrothermalVentLine.IsHorizontalOrVertical())
                {
                    hydrothermalVentLineList.Add(hydrothermalVentLine);
                }
            }

            // Compare each line with each line to find their intersection points
            for (int i = 0; i < hydrothermalVentLineList.Count; i++)
            {
                for (int j = i+1; j < hydrothermalVentLineList.Count; j++)
                {
                    // Get list of points for each line
                    line1Points = hydrothermalVentLineList.ElementAt(i).GetPoints();
                    line2Points = hydrothermalVentLineList.ElementAt(j).GetPoints();

                    // Look for points that are the same
                    // TODO: use hashsets instead
                    List<Point> intersections = line1Points.Intersect(line2Points).ToList();

                    // Add found points to the result set
                    intersectionPoints.UnionWith(intersections);
                }
            }

            return intersectionPoints.Count;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Day 1 part 1: {0}", Day1_1()));
            Console.WriteLine(String.Format("Day 1 part 2: {0}", Day1_2()));
            Console.WriteLine(string.Format("Day 2 part 1: {0}", Day2_1()));
            Console.WriteLine(string.Format("Day 2 part 2: {0}", Day2_2()));
            Console.WriteLine(string.Format("Day 3 part 1: {0}", Day3_1()));
            Console.WriteLine(string.Format("Day 3 part 2: {0}", Day3_2()));
            Console.WriteLine(string.Format("Day 4 part 1: {0}", Day4_1()));
            Console.WriteLine(string.Format("Day 4 part 2: {0}", Day4_2()));
            Console.WriteLine(string.Format("Day 5 part 1: {0}", Day5_1()));
        }
    }
}