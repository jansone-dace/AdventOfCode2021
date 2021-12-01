using System.IO;

namespace AdventOfCode2021
{
    class Program
    {
        static int Day1_1()
        {
            string[] depthMeasurements;
            int previuosDepthMeasurement, currentDepthMeasurement, increaseCount = 0;

            depthMeasurements = File.ReadAllLines(@".\inputs\day1_1.txt");
            
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

        static void Main(string[] args)
        {
            Console.WriteLine(Day1_1());
        }
    }
}