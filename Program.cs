using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Day2();
            Console.ReadLine();
        }

        static void Day1()
        {
            string fileLocation = @"M:\AoC2021Data\Day1.txt";
            List<int> depths = File.ReadAllLines(fileLocation).Select(int.Parse).ToList();

            int counter = 0;

            for (int i = 1; i < depths.Count; i++)
            {
                if (depths[i - 1] < depths[i])
                    counter++;
            }
            Console.WriteLine(counter);
        }
        static void Day1B()
        {
            string fileLocation = @"M:\AoC2021Data\Day1.txt";
            List<int> depths = File.ReadAllLines(fileLocation).Select(int.Parse).ToList();

            int counter = 0;

            for (int i = 3; i < depths.Count; i++)
            {
                if (depths[i - 3] < depths[i])
                    counter++;
            }
            Console.WriteLine(counter);
        }

        static void Day2()
        {
            string fileLocation = @"M:\AoC2021Data\Day2.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            int depth = 0;
            int distance = 0;

            foreach (string instruction in instructions)
            {
                string[] parsedInstructions = instruction.Split(' ');
                int value = int.Parse(parsedInstructions[1]);
                switch (parsedInstructions[0])
                {
                    case "forward":
                        distance += value;
                        break;
                    case "down":
                        depth += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                }
            }
            Console.WriteLine(depth*distance);
        }
    }
}
