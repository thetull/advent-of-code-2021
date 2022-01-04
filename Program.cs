using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Day12B();
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

            Console.WriteLine(depth * distance);
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
                    matrix[i, j] = instructions[firstLine + j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
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

            Console.WriteLine(sumUnmarked * calledNumbers[callCount - 1]);
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

            int[] markedNumbers = calledNumbers.Take(callCount + 1).ToArray();

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

        static void Day6A()
        {
            string fileLocation = @"M:\AoC2021Data\Day6A.txt";
            List<int> startingConditions = File.ReadAllLines(fileLocation).ToList()[0].Split(',').Select(int.Parse).ToList();

            int[] fish = new int[9];

            foreach (int startingCondition in startingConditions)
            {
                fish[startingCondition]++;
            }

            for (int i = 0; i < 80; i++)
            {
                int breeders = fish[0];
                fish = fish.Skip(1).Append(breeders).ToArray();
                fish[6] += breeders;
            }

            Console.WriteLine(fish.Sum());
        }

        static void Day6B()
        {
            string fileLocation = @"M:\AoC2021Data\Day6A.txt";
            List<int> startingConditions = File.ReadAllLines(fileLocation).ToList()[0].Split(',').Select(int.Parse).ToList();

            long[] fish = new long[9];

            foreach (int startingCondition in startingConditions)
            {
                fish[startingCondition]++;
            }

            for (int i = 0; i < 256; i++)
            {
                long breeders = fish[0];
                fish = fish.Skip(1).Append(breeders).ToArray();
                fish[6] += breeders;
            }

            Console.WriteLine(fish.Sum());
        }

        static void Day7A()
        {
            string fileLocation = @"M:\AoC2021Data\Day7A.txt";
            List<int> startingConditions = File.ReadAllLines(fileLocation).ToList()[0].Split(',').Select(int.Parse).ToList();

            //int targetPosition = (int)Math.Round(Math.Sqrt(startingConditions.Average(x => x*x)));
            int targetPosition = startingConditions.Min();
            int bestResult = int.MaxValue;
            int bestPosition = -1;
            for (int i = startingConditions.Min(); i <= startingConditions.Max(); i++)
            {
                int result = startingConditions.Sum(x => Math.Abs(i - x));
                if (result < bestResult)
                {
                    bestResult = result;
                    bestPosition = i;
                }
            }
            //int sum = startingConditions.Sum(x => Math.Abs(targetPosition - x));

            Console.WriteLine(bestResult);
        }

        static void Day7B()
        {
            string fileLocation = @"M:\AoC2021Data\Day7A.txt";
            List<int> startingConditions = File.ReadAllLines(fileLocation).ToList()[0].Split(',').Select(int.Parse).ToList();

            //int targetPosition = (int)Math.Round(Math.Sqrt(startingConditions.Average(x => x*x)));
            int targetPosition = startingConditions.Min();
            int bestResult = int.MaxValue;
            int bestPosition = -1;
            for (int i = startingConditions.Min(); i <= startingConditions.Max(); i++)
            {
                int result = startingConditions.Sum(x => (Math.Abs(i - x)) * (Math.Abs(i - x) + 1) / 2);
                if (result < bestResult)
                {
                    bestResult = result;
                    bestPosition = i;
                }
            }
            //int sum = startingConditions.Sum(x => Math.Abs(targetPosition - x));

            Console.WriteLine(bestResult);
        }

        static void Day8()
        {
            string fileLocation = @"M:\AoC2021Data\Day8.txt";
            List<Tuple<List<string>, List<string>>> data = File.ReadAllLines(fileLocation).Select(x => x.Split(" | ", StringSplitOptions.RemoveEmptyEntries)).Select(x => new Tuple<List<string>, List<string>>(x[0].Split(' ').ToList(), x[1].Split(' ').ToList())).ToList();


            List<string> results = data.Select(Disambiguate).ToList();

            Console.WriteLine(results.Sum(x => x.Count(c => c == '1' || c == '4' || c == '7' || c == '8')));
            Console.WriteLine(results.Sum(int.Parse));
        }

        static string Disambiguate(Tuple<List<string>, List<string>> input)
        {
            HashSet<int>[] possibleDigits = new HashSet<int>[10].Select(x => new HashSet<int>(new[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })).ToArray();
            HashSet<char>[] possibleSegments = new HashSet<char>[7].Select(x => new HashSet<char>(new[]{ 'a', 'b', 'c', 'd', 'e', 'f', 'g' })).ToArray();

            int[] segmentUsages = new[]{ 8, 6, 8, 7, 4, 9, 7 };

            string[] validCombinations ={ "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg" };

            for (int i = 0; i < 10; i++)
            {
                int length = input.Item1[i].Length;
                for (int j = 0; j < 10; j++)
                {
                    if (validCombinations[j].Length != length)
                        possibleDigits[i].Remove(j);
                }
            }

            Dictionary<int, int> possibleDigitsDict = new Dictionary<int, int>();

            for (int i = 0; i < 10; i++)
            {
                if (possibleDigits[i].Count != 1)
                    continue;
                possibleDigitsDict[possibleDigits[i].First()] = i;
            }

            for (int i = 0; i < 7; i++)
            {
                char character = (char)('a' + i);
                int frequency = input.Item1.Count(x => x.Contains(character));
                for (int j = 0; j < 7; j++)
                {
                    if (segmentUsages[j] != frequency)
                        possibleSegments[i].Remove((char)('a' + j));
                }
            }

            for (int i = 0; i < 7; i++)
            {
                char character = (char)('a' + i);
                if (possibleSegments[i].Count == 1)
                    continue;
                if (possibleSegments[i].Contains('c'))
                {
                    int onesIndex = possibleDigitsDict[1];
                    if (input.Item1[onesIndex].Contains(character))
                    {
                        possibleSegments[i].Remove('a');
                    }
                    else
                    {
                        possibleSegments[i].Remove('c');
                    }
                }
                else
                {
                    int foursIndex = possibleDigitsDict[4];
                    if (input.Item1[foursIndex].Contains(character))
                    {
                        possibleSegments[i].Remove('g');
                    }
                    else
                    {
                        possibleSegments[i].Remove('d');
                    }
                }
            }

            Dictionary<char, char> translation = new Dictionary<char, char>(); // = possibleSegments.ToList().ToDictionary((x, i) => (char)('a' + i), x => x.First());
            for (int i = 0; i < 7; i++)
            {
                translation[(char)('a' + i)] = possibleSegments[i].First();
            }

            string result = "";

            for (int i = 0; i < 4; i++)
            {
                string toTranslate = input.Item2[i];
                List<char> translated = toTranslate.Select(x => translation[x]).ToList();
                translated.Sort();
                string finalTranslation = string.Join("", translated);
                result += validCombinations.ToList().IndexOf(finalTranslation);
            }

            return result;
        }

        static void Day9A()
        {
            string fileLocation = @"M:\AoC2021Data\Day9A.txt";
            string[] data = File.ReadAllLines(fileLocation);

            int[,] map = new int[data[0].Length, data.Length];

            for (int i = 0; i < data[0].Length; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    map[i, j] = int.Parse(data[j][i].ToString());
                }
            }

            int risk = 0;

            for (int i = 0; i < data[0].Length; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    bool isLowest = true;
                    int depth = map[i, j];
                    Point[] pointsToCheck ={ new Point(i, j + 1), new Point(i, j - 1), new Point(i + 1, j), new Point(i - 1, j) };

                    foreach (Point point in pointsToCheck)
                    {
                        int x = point.X;
                        int y = point.Y;
                        if (x < 0 || x >= data[0].Length || y < 0 || y >= data.Length)
                            continue;
                        if (depth >= map[x, y])
                            isLowest = false;
                    }

                    if (isLowest)
                        risk += (depth + 1);
                }
            }

            Console.WriteLine(risk);
            // List<string> results = data.Select(Disambiguate).ToList();
            //
            // Console.WriteLine(results.Sum(x => x.Count(c => c == '1' || c == '4' || c == '7' || c == '8')));
            // Console.WriteLine(results.Sum(int.Parse));
        }

        static void Day9B()
        {
            string fileLocation = @"M:\AoC2021Data\Day9A.txt";
            string[] data = File.ReadAllLines(fileLocation);

            int[,] map = new int[data.Length, data[0].Length];
            bool[,] basined = new bool[data.Length, data[0].Length];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    map[i, j] = int.Parse(data[i][j].ToString());
                    basined[i, j] = map[i, j] == 9;
                }
            }

            List<int> basinSizes = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    if (!basined[i, j])
                    {
                        HashSet<Point> pointsInBasin = new HashSet<Point>{ new Point(i, j) };
                        basined[i, j] = true;
                        int currentSize = int.MinValue;

                        while (pointsInBasin.Count != currentSize)
                        {
                            currentSize = pointsInBasin.Count;
                            foreach (Point point in pointsInBasin.ToList())
                            {
                                Point[] pointsToCheck ={ new Point(point.X, point.Y + 1), new Point(point.X, point.Y - 1), new Point(point.X + 1, point.Y), new Point(point.X - 1, point.Y) };
                                foreach (Point adjacentPoint in pointsToCheck)
                                {
                                    int x = adjacentPoint.X;
                                    int y = adjacentPoint.Y;
                                    if (x < 0 || x >= data.Length || y < 0 || y >= data[0].Length)
                                        continue;
                                    if (map[x, y] != 9)
                                    {
                                        pointsInBasin.Add(adjacentPoint);
                                        basined[x, y] = true;
                                    }
                                }
                            }
                        }

                        basinSizes.Add(pointsInBasin.Count);
                    }
                }
            }

            basinSizes.Sort();
            Console.WriteLine(Math.Round(Math.Exp(basinSizes.Skip(basinSizes.Count - 3).Select(x => Math.Log(x)).Sum())));
            // List<string> results = data.Select(Disambiguate).ToList();
            //
            // Console.WriteLine(results.Sum(x => x.Count(c => c == '1' || c == '4' || c == '7' || c == '8')));
            // Console.WriteLine(results.Sum(int.Parse));
        }

        static void Day10A()
        {
            string fileLocation = @"M:\AoC2021Data\Day10A.txt";
            string[] data = File.ReadAllLines(fileLocation);
            string openCharacters = "([{<";
            Dictionary<char, char> pairs = new Dictionary<char, char>{ { '(', ')' },{ '[', ']' },{ '{', '}' },{ '<', '>' } };
            Dictionary<char, int> points = new Dictionary<char, int>{ { ')', 3 },{ ']', 57 },{ '}', 1197 },{ '>', 25137 } };
            List<char> illegalCharacters = new List<char>();
            int sum = 0;

            foreach (string line in data)
            {
                Stack<char> currentStack = new Stack<char>();
                foreach (char c in line)
                {
                    if (openCharacters.Contains(c))
                        currentStack.Push(pairs[c]);
                    else
                    {
                        char expectation = currentStack.Pop();
                        if (c == expectation)
                            continue;
                        sum += points[c];
                        break;
                    }
                }
            }

            Console.Write(sum);
        }

        static void Day10B()
        {
            string fileLocation = @"M:\AoC2021Data\Day10A.txt";
            string[] data = File.ReadAllLines(fileLocation);
            string openCharacters = "([{<";
            Dictionary<char, char> pairs = new Dictionary<char, char>{ { '(', ')' },{ '[', ']' },{ '{', '}' },{ '<', '>' } };
            Dictionary<char, int> points = new Dictionary<char, int>{ { ')', 1 },{ ']', 2 },{ '}', 3 },{ '>', 4 } };
            List<char> illegalCharacters = new List<char>();
            List<long> scores = new List<long>();

            foreach (string line in data)
            {
                Stack<char> currentStack = new Stack<char>();
                foreach (char c in line)
                {
                    if (openCharacters.Contains(c))
                        currentStack.Push(pairs[c]);
                    else
                    {
                        char expectation = currentStack.Pop();
                        if (c != expectation)
                        {
                            currentStack.Clear();
                            break;
                        }
                    }
                }


                if (currentStack.Any())
                {
                    long partSum = 0;
                    while (currentStack.Any())
                    {
                        char next = currentStack.Pop();
                        partSum = partSum * 5 + points[next];
                    }

                    scores.Add(partSum);
                }
            }

            scores.Sort();
            Console.Write(scores[(scores.Count / 2)]);
        }

        static void Day11A()
        {
            string fileLocation = @"M:\AoC2021Data\Day11A.txt";
            string[] data = File.ReadAllLines(fileLocation);


            int[,] map = new int[data.Length, data[0].Length];
            bool[,] hasFlashed = new bool[data.Length, data[0].Length];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    map[i, j] = int.Parse(data[i][j].ToString());
                    hasFlashed[i, j] = false;
                }
            }

            int totalFlashed = 0;
            for (int k = 0; k < 100; k++)
            {
                map = ForEach2Dimensional(map, x => x + 1);
                hasFlashed = ForEach2Dimensional(hasFlashed, x => false);
                bool iterate;
                do
                {
                    iterate = false;
                    for (int i = 0; i < data.Length; i++)
                    {
                        for (int j = 0; j < data[0].Length; j++)
                        {
                            if (map[i, j] > 9 && !hasFlashed[i, j])
                            {
                                iterate = true;
                                hasFlashed[i, j] = true;
                                for (int l = -1; l <=1; l++)
                                {
                                    for (int m = -1; m <= 1; m++)
                                    {
                                        int x = i + l;
                                        int y = j + m;
                                        if (!(x < 0 || x >= data.Length || y < 0 || y >= data[0].Length))
                                        {
                                            map[x, y]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                while (iterate);

                map = ForEach2Dimensional(map, x =>
                {
                    if (x > 9)
                    {
                        totalFlashed++;
                        return 0;
                    }

                    return x;
                });

                Console.WriteLine(totalFlashed);
            }
        }

        static T[,] ForEach2Dimensional<T>(T[,] input, Func<T, T> action)
        {
            T[,] output = new T[input.GetUpperBound(0)+1, input.GetUpperBound(1)+1];
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= input.GetUpperBound(1); j++)
                {
                    output[i, j] = action(input[i, j]);
                }
            }
            return output;
        }

        static T[,] ForEach2DimensionalAdjacent<T>(T[,] input, int row, int column, Func<T, T> action)
        {
            T[,] output = new T[input.GetUpperBound(0), input.GetUpperBound(1)];
            Point[] adjacentPoints = new Point[8];
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= input.GetUpperBound(1); j++)
                {
                    output[i, j] = action(input[i, j]);
                }
            }
            return output;
        }

        static T[,] ForEach2Dimensional<T>(T[,] input, Func<int, int, T, T> action)
        {
            T[,] output = new T[input.GetUpperBound(0), input.GetUpperBound(1)];
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= input.GetUpperBound(1); j++)
                {
                    output[i, j] = action(i, j, input[i, j]);
                }
            }
            return output;
        }

        static void Day11B()
        {
            string fileLocation = @"M:\AoC2021Data\Day11A.txt";
            string[] data = File.ReadAllLines(fileLocation);


            int[,] map = new int[data.Length, data[0].Length];
            bool[,] hasFlashed = new bool[data.Length, data[0].Length];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    map[i, j] = int.Parse(data[i][j].ToString());
                    hasFlashed[i, j] = false;
                }
            }

            int totalFlashed = 0;
            for (int k = 0; k < 10000; k++)
            {
                map = ForEach2Dimensional(map, x => x + 1);
                hasFlashed = ForEach2Dimensional(hasFlashed, x => false);
                bool iterate;
                do
                {
                    iterate = false;
                    for (int i = 0; i < data.Length; i++)
                    {
                        for (int j = 0; j < data[0].Length; j++)
                        {
                            if (map[i, j] > 9 && !hasFlashed[i, j])
                            {
                                iterate = true;
                                hasFlashed[i, j] = true;
                                for (int l = -1; l <= 1; l++)
                                {
                                    for (int m = -1; m <= 1; m++)
                                    {
                                        int x = i + l;
                                        int y = j + m;
                                        if (!(x < 0 || x >= data.Length || y < 0 || y >= data[0].Length))
                                        {
                                            map[x, y]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                while (iterate);

                map = ForEach2Dimensional(map, x =>
                {
                    if (x > 9)
                    {
                        totalFlashed++;
                        return 0;
                    }

                    return x;
                });
                // int totalFlashes = 0;
                // ForEach2Dimensional(hasFlashed, x =>
                // {
                //     totalFlashes += x ? 1 : 0;
                //     return x;
                // });
                bool result = true;
                foreach (bool b in hasFlashed)
                    if (!b)
                    {
                        result = false;
                        break;
                    }

                if (result)
                {
                    Console.WriteLine(k+1);
                    break;
                }
                // if ((totalFlashes == (hasFlashed.GetUpperBound(0) + 1) * (hasFlashed.GetUpperBound(1) + 1)))
                //     break;

                Console.WriteLine(totalFlashed);
            }
        }

        static void Day12A()
        {
            string fileLocation = @"M:\AoC2021Data\Day12A.txt";
            string[] data = File.ReadAllLines(fileLocation);

            Graph g = new Graph();
            foreach (string s in data)
            {
                string[] nodes = s.Split('-');
                g.CreateEdgeBetween(nodes[1], nodes[0], 1);
            }

            List<List<string>> result = Explore(new List<string>{"start"}, g);

            Console.WriteLine(result.Count);
        }

        private static List<List<string>> Explore(List<string> currentPath, Graph g)
        {
            if (currentPath.Last() == "end")
                return new List<List<string>>{ currentPath };
            HashSet<string> nextDestination = g.GetConnectedIdentifiers(currentPath.Last());
            List<List<string>> result = new List<List<string>>();
            foreach (string destination in nextDestination)
            {
                if (destination == destination.ToLowerInvariant())
                    if (currentPath.Contains(destination))
                        continue;
                List<string> nextPath = currentPath.ToList();
                nextPath.Add(destination);
                result.AddRange(Explore(nextPath, g));
            }

            return result;
        }

        static void Day12B()
        {
            string fileLocation = @"M:\AoC2021Data\Day12A.txt";
            string[] data = File.ReadAllLines(fileLocation);

            Graph g = new Graph();
            foreach (string s in data)
            {
                string[] nodes = s.Split('-');
                g.CreateEdgeBetween(nodes[1], nodes[0], 1);
            }

            List<List<string>> result = ExploreB(new List<string> { "start" }, g, true);

            Console.WriteLine(result.Count);
        }

        private static List<List<string>> ExploreB(List<string> currentPath, Graph g, bool canDoubleVisit)
        {
            if (currentPath.Last() == "end")
                return new List<List<string>> { currentPath };
            HashSet<string> nextDestination = g.GetConnectedIdentifiers(currentPath.Last());
            List<List<string>> result = new List<List<string>>();
            foreach (string destination in nextDestination)
            {
                bool nextDoubleVisit = canDoubleVisit;
                if (destination == destination.ToLowerInvariant())
                    if (currentPath.Contains(destination))
                    {
                        if (destination == "start")
                            continue;
                        if (canDoubleVisit)
                        {
                            nextDoubleVisit = false;
                        }
                        else
                        {
                            continue;
                        }
                    }

                List<string> nextPath = currentPath.ToList();
                nextPath.Add(destination);
                result.AddRange(ExploreB(nextPath, g, nextDoubleVisit));
            }

            return result;
        }
    }
}
