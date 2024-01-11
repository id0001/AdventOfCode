using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(11)]
public class Challenge11(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await InputReader.ReadGridAsync<int>(11);

        var totalFlashCount = 0;

        for (var steps = 0; steps < 100; steps++)
        {
            var queue = new Queue<Point2>();
            var visited = new HashSet<Point2>();

            // Increment step
            Increment(input, queue, visited);

            // Flash step
            Flash(input, queue, visited);
            totalFlashCount += visited.Count;
        }

        return totalFlashCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await InputReader.ReadGridAsync<int>(11);

        var step = 0;
        while (true)
        {
            var queue = new Queue<Point2>();
            var visited = new HashSet<Point2>();

            // Increment step
            Increment(input, queue, visited);

            // Flash step
            Flash(input, queue, visited);

            step++;
            if (visited.Count == 100) // all octopusses flashed this step
                break;
        }

        return step.ToString();
    }

    private static void Flash(int[,] input, Queue<Point2> queue, ISet<Point2> visited)
    {
        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            input[p.Y, p.X] = 0;

            foreach (var neighbor in p.GetNeighbors(true))
            {
                if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= input.GetLength(1) ||
                    neighbor.Y >= input.GetLength(0))
                    continue;

                if (visited.Contains(neighbor)) continue;

                input[neighbor.Y, neighbor.X]++;

                if (input[neighbor.Y, neighbor.X] <= 9) continue;

                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
    }

    private static void Increment(int[,] input, Queue<Point2> flashQueue, ISet<Point2> hasFlashed)
    {
        for (var y = 0; y < 10; y++)
        for (var x = 0; x < 10; x++)
        {
            input[y, x]++;
            if (input[y, x] <= 9) continue;

            flashQueue.Enqueue(new Point2(x, y));
            hasFlashed.Add(new Point2(x, y));
        }
    }
}