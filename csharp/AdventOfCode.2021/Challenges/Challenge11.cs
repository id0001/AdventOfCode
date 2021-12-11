using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(11)]
    public class Challenge11
    {
        private readonly IInputReader inputReader;
        private int[,] input;

        public Challenge11(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            input = await inputReader.ReadGridAsync<int>(11);
        }

        [Part1]
        public string Part1()
        {
            int totalFlashCount = 0;

            for (int steps = 0; steps < 100; steps++)
            {
                Queue<Point2> queue = new Queue<Point2>();
                ISet<Point2> visited = new HashSet<Point2>();

                // Increment step
                Increment(queue, visited);

                // Flash step
                Flash(queue, visited);
                totalFlashCount += visited.Count;
            }

            return totalFlashCount.ToString();
        }

        [Part2]
        public string Part2()
        {
            int step = 0;
            while (true)
            {
                Queue<Point2> queue = new Queue<Point2>();
                ISet<Point2> visited = new HashSet<Point2>();

                // Increment step
                Increment(queue, visited);

                // Flash step
                Flash(queue, visited);

                step++;
                if (visited.Count == 100) // all octopusses flashed this step
                    break;
            }

            return step.ToString();
        }

        private void Flash(Queue<Point2> queue, ISet<Point2> visited)
        {
            while (queue.Count > 0)
            {
                Point2 p = queue.Dequeue();
                input[p.Y, p.X] = 0;

                foreach (var neighbor in p.GetNeighbors())
                {
                    if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= input.GetLength(1) || neighbor.Y >= input.GetLength(0))
                        continue;

                    if (!visited.Contains(neighbor))
                    {
                        input[neighbor.Y, neighbor.X]++;
                        if (input[neighbor.Y, neighbor.X] > 9)
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }
            }
        }

        private void Increment(Queue<Point2> flashQueue, ISet<Point2> hasFlashed)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    input[y, x]++;
                    if (input[y, x] > 9)
                    {
                        flashQueue.Enqueue(new Point2(x, y));
                        hasFlashed.Add(new Point2(x, y));
                    }
                }
            }
        }
    }
}
