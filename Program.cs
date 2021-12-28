using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Day3B();
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

        static void Day2B()
        {
            string fileLocation = @"M:\Temp\AdventOfCode\input2B.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            int depth = 0;
            int distance = 0;
            int aim = 0;

            foreach (string instruction in instructions)
            {
                string[] parsedInstructions = instruction.Split(' ');
                int value = int.Parse(parsedInstructions[1]);
                switch (parsedInstructions[0])
                {
                    case "forward":
                        distance += value;
                        depth += value * aim;
                        break;
                    case "down":
                        aim += value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                }
            }
            Console.WriteLine(depth * distance);
        }

        static void Day3A()
        {
            string fileLocation = @"M:\Temp\AdventOfCode\input3A.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            int[] bitCount = new int[instructions[0].Length];

            foreach (string instruction in instructions)
                for (int i = 0; i < instruction.Length; i++)
                    bitCount[i] += instruction[i] == '0' ? 0 : 1;
            int gamma = 0;
            int epsilon = 0;
            foreach (int bits in bitCount)
            {
                gamma = gamma * 2 + ((bits > instructions.Count / 2) ? 1 : 0);
                epsilon = epsilon * 2 + ((bits > instructions.Count / 2) ? 0 : 1);
            }
            Console.WriteLine(gamma * epsilon);
        }

        static void Day3B()
        {
            string fileLocation = @"M:\Temp\AdventOfCode\input3B.txt";
            List<string> instructionsGenerator = File.ReadAllLines(fileLocation).ToList();
            List<string> instructionsScrubber = File.ReadAllLines(fileLocation).ToList();

            int offset = 0;

            while (instructionsGenerator.Count > 1)
            {
                int bitCount = instructionsGenerator.Sum(s => (s[offset] == '1' ? 1 : 0));
                char filter = (bitCount >= instructionsGenerator.Count / 2d) ? '1' : '0';
                instructionsGenerator = instructionsGenerator.Where(x => x[offset] == filter).ToList();
                offset++;
            }

            offset = 0;
            while (instructionsScrubber.Count > 1)
            {
                int bitCount = instructionsScrubber.Sum(s => (s[offset] == '1' ? 1 : 0));
                char filter = (bitCount < instructionsScrubber.Count / 2d) ? '1' : '0';
                instructionsScrubber = instructionsScrubber.Where(x => x[offset] == filter).ToList();
                offset++;
            }

            int generator = 0;
            foreach (char c in instructionsGenerator[0])
                generator = generator * 2 + (c == '1' ? 1 : 0);

            int scrubber = 0;
            foreach (char c in instructionsScrubber[0])
                scrubber = scrubber * 2 + (c == '1' ? 1 : 0);
            Console.WriteLine(scrubber * generator);
        }
    }
}
