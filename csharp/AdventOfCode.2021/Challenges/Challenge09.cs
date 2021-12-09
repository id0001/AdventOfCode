using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(9)]
    public class Challenge09
    {
        private readonly IInputReader inputReader;
        private int[,] grid;

        public Challenge09(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            grid = await inputReader.ReadGridAsync<int>(9);
        }

        [Part1]
        public string Part1()
        {
            return CalculateRiskLevel(grid).ToString();
        }

        [Part2]
        public string Part2()
        {
            List<Point2[]> basins = new List<Point2[]>();
            foreach (var lowestPoint in LowestPoints(grid))
            {
                var set = new HashSet<Point2>();
                FloodFill(lowestPoint, grid, set);
                basins.Add(set.ToArray());
            }

            return basins
                .Select(list => list.Count())
                .OrderByDescending(x => x)
                .Take(3)
                .Product()
                .ToString();
        }

        private static IEnumerable<Point2> LowestPoints(int[,] grid)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == 9)
                        continue;

                    var p = new Point2(x, y);
                    if (IsLowerThanNeighbors(grid, p))
                    {
                        yield return p;
                    }
                }
            }
        }

        private static void FloodFill(Point2 p, int[,] grid, ISet<Point2> points)
        {
            if (p.X < 0 || p.Y < 0 || p.X >= grid.GetLength(1) || p.Y >= grid.GetLength(0))
                return;

            if (points.Contains(p) || grid[p.Y, p.X] == 9)
                return;

            points.Add(p);
            foreach (var neighbor in p.GetNeighbors4())
                FloodFill(neighbor, grid, points);
        }

        private static int CalculateRiskLevel(int[,] grid)
        {
            int risk = 0;
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == 9)
                        continue;

                    var p = new Point2(x, y);
                    if (IsLowerThanNeighbors(grid, p))
                    {
                        risk += grid[y, x] + 1;
                    }
                }
            }

            return risk;
        }

        private static bool IsLowerThanNeighbors(int[,] grid, Point2 p) => p.GetNeighbors4()
            .All(n =>
        {
            if (n.X < 0 || n.Y < 0 || n.X >= grid.GetLength(1) || n.Y >= grid.GetLength(0))
                return true;

            return grid[p.Y, p.X] < grid[n.Y, n.X];
        });
    }
}
