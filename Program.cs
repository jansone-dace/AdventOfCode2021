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

        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Day 1 part 1: {0}", Day1_1()));
            Console.WriteLine(String.Format("Day 1 part 2: {0}", Day1_2()));
        }
    }
}