using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using System.Text;

namespace AdventOfCode2022.Challenges
{
    [Challenge(17)]
    public class Challenge17
    {
        private static readonly char[] Shapes = new[] { '-', '+', 'L', '|', '#' };

        private readonly IInputReader _inputReader;

        public Challenge17(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var dirs = await _inputReader.ReadLineAsync(0).ToArrayAsync();
            var dirIndex = 0;


            var index = 0;
            var grid = new bool[2022 * 4, 7];
            var currentBottom = grid.GetLength(0);
            var currentShape = GetShape(Shapes[index]);
            int x = 2;
            int y = currentBottom - 3 - currentShape.Height;

            int rocksStopped = 0;
            while (rocksStopped < 2022)
            {
                //PrintState(grid, currentShape, x, y);
                //Console.WriteLine();
                //Console.ReadKey(false);

                char dir = dirs[dirIndex % dirs.Length];

                switch (dir)
                {
                    case '>':
                        if (!Collides(grid, currentShape, x + 1, y))
                            x++;
                        break;
                    case '<':
                        if (!Collides(grid, currentShape, x - 1, y))
                            x--;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (Collides(grid, currentShape, x, y + 1))
                {
                    BakeShape(grid, currentShape, x, y);
                    index = (index + 1) % Shapes.Length;
                    currentShape = GetShape(Shapes[index]);
                    currentBottom = y;
                    x = 2;
                    y = currentBottom - 3 - currentShape.Height;
                    dirIndex++;
                    rocksStopped++;
                    continue;
                }

                y++;
                dirIndex++;
            }

            Print(grid);
            return currentBottom.ToString();
        }

        private static void Print(bool[,] grid)
        {
            StringBuilder sw = new StringBuilder();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                    sw.Append(grid[y, x] ? '#' : '.');

                if (sw.ToString().Any(x => x == '#'))
                    Console.WriteLine(sw.ToString());

                sw.Clear();
            }
        }

        private static void PrintState(bool[,] grid, Shape shape, int left, int top)
        {
            bool doPrint = false;
            StringBuilder sw = new StringBuilder();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    bool isBlock = false;

                    if (grid[y, x])
                        isBlock = true;
                    else if (x >= left && x < left + shape.Width && y >= top && y < top + shape.Height)
                    {
                        if (shape.Grid[y - top, x - left])
                            isBlock = true;
                    }

                    sw.Append(isBlock ? '#' : '.');
                }

                if (sw.ToString().Any(x => x == '#'))
                    doPrint = true;

                if (doPrint)
                    Console.WriteLine(sw.ToString());

                sw.Clear();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static Shape GetShape(char type) => type switch
        {
            '-' => new Shape(4, 1, type, new[,] { { true, true, true, true } }),
            '+' => new Shape(3, 3, type, new[,] { { false, true, false }, { true, true, true }, { false, true, false } }),
            'L' => new Shape(3, 3, type, new[,] { { false, false, true }, { false, false, true }, { true, true, true } }),
            '|' => new Shape(1, 4, type, new[,] { { true }, { true }, { true }, { true } }),
            '#' => new Shape(2, 2, type, new[,] { { true, true }, { true, true } }),
            _ => throw new NotImplementedException()
        };

        public static bool Collides(bool[,] grid, Shape shape, int left, int top)
        {
            if (left < 0 || left + shape.Width - 1 == grid.GetLength(1))
                return true;

            if (top + shape.Height - 1 == grid.GetLength(0))
                return true;

            for (int ly = shape.Height - 1; ly >= 0; ly--)
            {
                for (int lx = 0; lx < shape.Width; lx++)
                {
                    int y = top + ly;
                    int x = left + lx;

                    if (shape.Grid[ly, lx] && grid[y, x])
                        return true;
                }
            }

            return false;
        }

        public static void BakeShape(bool[,] grid, Shape shape, int left, int top)
        {
            for (int y = 0; y < shape.Height; y++)
            {
                for (int x = 0; x < shape.Width; x++)
                {
                    grid[top + y, left + x] = shape.Grid[y, x];
                }
            }
        }

        public record Shape(int Width, int Height, char Type, bool[,] Grid);
    }
}
