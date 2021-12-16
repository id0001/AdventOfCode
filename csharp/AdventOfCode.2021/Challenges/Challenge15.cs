using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode.Lib.Pathfinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(15)]
    public class Challenge15
    {
        private readonly IInputReader inputReader;
        private int[,] grid;

        public Challenge15(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            grid = await inputReader.ReadGridAsync<int>(15);
        }

        [Part1]
        public string Part1()
        {
            var dijkstra = new Dijkstra<Point2>(GetAdjecentNodes);
            if (dijkstra.TryPath(new Point2(0, 0), new Point2(grid.GetLength(0) - 1, grid.GetLength(1) - 1), out DijkstraResult<Point2> result))
            {
                return result.Cost.ToString();
            }

            return null;
        }

        [Part2]
        public string Part2()
        {
            Point2 start = Point2.Zero;
            Point2 end = new Point2((grid.GetLength(0) * 5) - 1, (grid.GetLength(1) * 5) - 1);

            var dijkstra = new Dijkstra<Point2>(GetAdjecentNodes2);
            if (dijkstra.TryPath(start, end, out DijkstraResult<Point2> result))
            {
                return result.Cost.ToString();
            }

            return null;
        }

        private IEnumerable<(Point2, int)> GetAdjecentNodes(Point2 currentNode)
        {
            foreach (var neighbor in currentNode.GetNeighbors4())
            {
                if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= grid.GetLength(1) || neighbor.Y >= grid.GetLength(0))
                    continue;

                yield return (neighbor, grid[neighbor.Y, neighbor.X]);
            }
        }

        private IEnumerable<(Point2, int)> GetAdjecentNodes2(Point2 currentNode)
        {
            int width = grid.GetLength(1) * 5;
            int height = grid.GetLength(0) * 5;

            foreach (var neighbor in currentNode.GetNeighbors4())
            {
                if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= width || neighbor.Y >= height)
                    continue;

                int rx = neighbor.X % grid.GetLength(1);
                int ry = neighbor.Y % grid.GetLength(0);
                int fx = neighbor.X / grid.GetLength(1);
                int fy = neighbor.Y / grid.GetLength(0);
                int risk = ((grid[ry, rx] + fx + fy - 1) % 9) + 1;

                yield return (neighbor, risk);
            }
        }

        private record Node(Point2 Location, int Weight);
    }
}
