using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Day5B();
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

        static void Day4A()
        {
            string fileLocation = @"M:\AoC2021Data\Day4AText.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            List<int> calledNumbers = instructions[0].Split(',').Select(int.Parse).ToList();


            int numBoards = (instructions.Count - 1) / 6;
            int[,][] matrix = new int[numBoards, 5][];

            for (int i = 0; i < numBoards; i++)
            {
                int firstLine = i * 6 + 2;
                for (int j = 0; j < 5; j++)
                {
                    matrix[i,j] = instructions[firstLine + j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                }
            }

            int winIndex = -1;

            int callCount = 4;

            while (winIndex == -1)
            {
                callCount++;
                winIndex = Winner(calledNumbers.Take(callCount).ToArray(), matrix);
            }

            int[] markedNumbers = calledNumbers.Take(callCount).ToArray();

            int sumUnmarked = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int value = matrix[winIndex, i][j];
                    if (!markedNumbers.Contains(value))
                    {
                        sumUnmarked += value;
                    }
                }
            }

            Console.WriteLine(sumUnmarked * calledNumbers[callCount-1]);
        }

        private static int Winner(int[] calledNumbers, int[,][] boards)
        {
            for (int i = 0; i < boards.GetLength(0); i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    bool winRow = true;
                    bool winCol = true;
                    for (int k = 0; k < 5; k++)
                    {
                        int colValue = boards[i, j][k];
                        int rowValue = boards[i, k][j];
                        if (!calledNumbers.Contains(colValue))
                            winCol = false;
                        if (!calledNumbers.Contains(rowValue))
                            winRow = false;
                        if ((winCol || winRow) == false)
                            break;
                    }

                    if (winCol || winRow)
                        return i;
                }
            }

            return -1;
        }

        static void Day4B()
        {
            string fileLocation = @"M:\AoC2021Data\Day4A.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            List<int> calledNumbers = instructions[0].Split(',').Select(int.Parse).ToList();


            int numBoards = (instructions.Count - 1) / 6;
            int[,][] matrix = new int[numBoards, 5][];

            for (int i = 0; i < numBoards; i++)
            {
                int firstLine = i * 6 + 2;
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = instructions[firstLine + j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                }
            }

            int winIndex = -1;

            int callCount = 4;

            while (winIndex == -1)
            {
                callCount++;
                winIndex = LastWinner(calledNumbers.Take(callCount).ToArray(), matrix);
            }

            int[] markedNumbers = calledNumbers.Take(callCount+1).ToArray();

            int sumUnmarked = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int value = matrix[winIndex, i][j];
                    if (!markedNumbers.Contains(value))
                    {
                        sumUnmarked += value;
                    }
                }
            }

            Console.WriteLine(sumUnmarked * calledNumbers[callCount]);
        }

        private static int LastWinner(int[] calledNumbers, int[,][] boards)
        {
            int loserIndex = -1;
            for (int i = 0; i < boards.GetLength(0); i++)
            {
                bool success = false;
                for (int j = 0; j < 5; j++)
                {
                    bool winRow = true;
                    bool winCol = true;
                    for (int k = 0; k < 5; k++)
                    {
                        int colValue = boards[i, j][k];
                        int rowValue = boards[i, k][j];
                        if (!calledNumbers.Contains(colValue))
                            winCol = false;
                        if (!calledNumbers.Contains(rowValue))
                            winRow = false;
                        if ((winCol || winRow) == false)
                            break;
                    }

                    if ((winCol || winRow))
                    {
                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    if (loserIndex == -1)
                        loserIndex = i;
                    else
                        return -1;
                }
            }

            return loserIndex;
        }
        static void Day5A()
        {
            string fileLocation = @"M:\AoC2021Data\Day5A.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            List<Tuple<Point, Point>> vents = instructions.Select(x => x.Split(" -> ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(',')).Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])))).Select(x => new Tuple<Point, Point>(x.First(), x.Last())).ToList();

            int xSize = vents.Max(x => Math.Max(x.Item1.X, x.Item2.X)) + 1;
            int ySize = vents.Max(x => Math.Max(x.Item1.Y, x.Item2.Y)) + 1;
            int[,] map = new int[xSize, ySize];

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    map[i, j] = 0;

            foreach (Tuple<Point, Point> vent in vents)
            {
                if (vent.Item1.X == vent.Item2.X)
                {
                    int lesserY = Math.Min(vent.Item1.Y, vent.Item2.Y);
                    int greaterY = Math.Max(vent.Item1.Y, vent.Item2.Y);

                    for (int i = lesserY; i <= greaterY; i++)
                    {
                        map[vent.Item1.X, i]++;
                    }
                }
                if (vent.Item1.Y == vent.Item2.Y)
                {
                    int lesserY = Math.Min(vent.Item1.X, vent.Item2.X);
                    int greaterY = Math.Max(vent.Item1.X, vent.Item2.X);

                    for (int i = lesserY; i <= greaterY; i++)
                    {
                        map[i, vent.Item1.Y]++;
                    }
                }
                // if (vent.Item1.X != vent.Item2.X && vent.Item1.Y != vent.Item2.Y)
                //     continue;
            }
            int result = 0;
            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    if (map[i, j] >= 2)
                        result++;
            Console.WriteLine(result);
        }

        static void Day5B()
        {
            string fileLocation = @"M:\AoC2021Data\Day5A.txt";
            List<string> instructions = File.ReadAllLines(fileLocation).ToList();

            List<Tuple<Point, Point>> vents = instructions.Select(x => x.Split(" -> ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(',')).Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])))).Select(x => new Tuple<Point, Point>(x.First(), x.Last())).ToList();

            int xSize = vents.Max(x => Math.Max(x.Item1.X, x.Item2.X)) + 1;
            int ySize = vents.Max(x => Math.Max(x.Item1.Y, x.Item2.Y)) + 1;
            int[,] map = new int[xSize, ySize];
            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    map[i, j] = 0;

            foreach (Tuple<Point, Point> vent in vents)
            {
                if (vent.Item1.X == vent.Item2.X)
                {
                    int lesserY = Math.Min(vent.Item1.Y, vent.Item2.Y);
                    int greaterY = Math.Max(vent.Item1.Y, vent.Item2.Y);

                    for (int i = lesserY; i <= greaterY; i++)
                    {
                        map[vent.Item1.X, i]++;
                    }
                }
                else if (vent.Item1.Y == vent.Item2.Y)
                {
                    int lesserY = Math.Min(vent.Item1.X, vent.Item2.X);
                    int greaterY = Math.Max(vent.Item1.X, vent.Item2.X);

                    for (int i = lesserY; i <= greaterY; i++)
                    {
                        map[i, vent.Item1.Y]++;
                    }
                }
                else if (vent.Item1.X > vent.Item2.X != vent.Item1.Y > vent.Item2.Y)
                {

                    int lesserY = Math.Min(vent.Item1.Y, vent.Item2.Y);
                    int greaterY = Math.Max(vent.Item1.Y, vent.Item2.Y);
                    int lesserX = Math.Min(vent.Item1.X, vent.Item2.X);
                    int greaterX = Math.Max(vent.Item1.X, vent.Item2.X);

                    for (int i = 0; i <= greaterX - lesserX; i++)
                        map[lesserX + i, greaterY - i]++;
                }
                else
                {
                    int lesserY = Math.Min(vent.Item1.Y, vent.Item2.Y);
                    int greaterY = Math.Max(vent.Item1.Y, vent.Item2.Y);
                    int lesserX = Math.Min(vent.Item1.X, vent.Item2.X);
                    int greaterX = Math.Max(vent.Item1.X, vent.Item2.X);

                    for (int i = 0; i <= greaterX - lesserX; i++)
                        map[lesserX + i, lesserY + i]++;
                }
                // if (vent.Item1.X != vent.Item2.X && vent.Item1.Y != vent.Item2.Y)
                //     continue;
            }
            int result = map.Cast<int>().Count(x => x >= 2);
            // for (int i = 0; i < xSize; i++)
            //     for (int j = 0; j < ySize; j++)
            //         if (map[i, j] >= 2)
            //             result++;
            Console.WriteLine(result);
        }
    }
}
