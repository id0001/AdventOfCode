using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using System.Net.WebSockets;
using System.Text;

namespace AdventOfCode2022.Challenges
{
    [Challenge(22)]
    public class Challenge22
    {
        private readonly IInputReader _inputReader;

        public Challenge22(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var lines = await _inputReader.ReadLinesAsync(22).ToListAsync();
            var (map, line) = ReadGridWithInstructions(lines);

            var pos = FindLeftMostOpenTile(map);
            var dir = new Point2(1, 0);
            var instructions = new Queue<char>(line);
            var numberBuilder = new StringBuilder();

            foreach (var instruction in instructions)
            {
                if (char.IsNumber(instruction))
                    numberBuilder.Append(instruction);
                else
                {
                    if (numberBuilder.Length > 0)
                    {
                        int num = int.Parse(numberBuilder.ToString());
                        numberBuilder.Clear();

                        for (int i = 0; i < num; i++)
                        {
                            var next = GetNext(map, pos, dir);
                            if (map[next.Y, next.X] == '.')
                                pos = next;
                        }
                    }

                    dir = instruction switch
                    {
                        'L' => Point2.Turn(dir, Point2.Zero, -Trigonometry.DegreeToRadian(90)),
                        'R' => Point2.Turn(dir, Point2.Zero, Trigonometry.DegreeToRadian(90)),
                        _ => throw new NotImplementedException()
                    };
                }
            }

            if (numberBuilder.Length > 0)
            {
                int num = int.Parse(numberBuilder.ToString());
                numberBuilder.Clear();

                for (int i = 0; i < num; i++)
                {
                    var next = GetNext(map, pos, dir);
                    if (map[next.Y, next.X] == '.')
                        pos = next;
                }
            }

            var facing = GetFacing(dir);
            var rows = (pos.Y + 1) * 1000;
            var cols = (pos.X + 1) * 4;
            return (rows + cols + facing).ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var lines = await _inputReader.ReadLinesAsync(22).ToListAsync();
            var (map, line) = ReadGridWithInstructions(lines);

            var pos = FindLeftMostOpenTile(map);
            var dir = new Point2(1, 0);
            var instructions = new Queue<char>(line);
            var numberBuilder = new StringBuilder();

            for(int y = 0; y < map.GetLength(0); y++)
            {
                for(int x = 0; x < map.GetLength(1); x++)
                {
                    int zoneX = (int)Math.Floor(x / 50d);
                    int zoneY = (int)Math.Floor(y / 50d);
                }
            }

            foreach (var instruction in instructions)
            {
                if (char.IsNumber(instruction))
                    numberBuilder.Append(instruction);
                else
                {
                    if (numberBuilder.Length > 0)
                    {
                        int num = int.Parse(numberBuilder.ToString());
                        numberBuilder.Clear();

                        for (int i = 0; i < num; i++)
                        {
                            var next = GetNext(map, pos, dir);
                            if (map[next.Y, next.X] == '.')
                                pos = next;
                        }
                    }

                    dir = instruction switch
                    {
                        'L' => Point2.Turn(dir, Point2.Zero, -Trigonometry.DegreeToRadian(90)),
                        'R' => Point2.Turn(dir, Point2.Zero, Trigonometry.DegreeToRadian(90)),
                        _ => throw new NotImplementedException()
                    };
                }
            }

            if (numberBuilder.Length > 0)
            {
                int num = int.Parse(numberBuilder.ToString());
                numberBuilder.Clear();

                for (int i = 0; i < num; i++)
                {
                    var next = GetNext(map, pos, dir);
                    if (map[next.Y, next.X] == '.')
                        pos = next;
                }
            }

            var facing = GetFacing(dir);
            var rows = (pos.Y + 1) * 1000;
            var cols = (pos.X + 1) * 4;
            return (rows + cols + facing).ToString();
        }

        private static int GetFacing(Point2 direction)
        {
            return direction switch
            {
                (1, 0) => 0,
                (0, 1) => 1,
                (-1, 0) => 2,
                (0, -1) => 3,
                _ => throw new NotImplementedException()
            };
        }

        private static Point2 GetNext(char[,] map, Point2 position, Point2 direction)
        {
            var next = new Point2(Euclid.Modulus(position.X + direction.X, map.GetLength(1)), Euclid.Modulus(position.Y + direction.Y, map.GetLength(0)));
            if (map[next.Y, next.X] == ' ')
            {
                if (direction.X == 1)
                {
                    for (var x = 0; x < map.GetLength(1); x++)
                        if (map[next.Y, x] != ' ')
                            return new Point2(x, next.Y);
                }
                else if (direction.X == -1)
                {
                    for (var x = map.GetLength(1) - 1; x >= 0; x--)
                        if (map[next.Y, x] != ' ')
                            return new Point2(x, next.Y);
                }
                else if (direction.Y == 1)
                {
                    for (var y = 0; y < map.GetLength(0); y++)
                        if (map[y, next.X] != ' ')
                            return new Point2(next.X, y);
                }
                else if (direction.Y == -1)
                {
                    for (var y = map.GetLength(0) - 1; y >= 0; y--)
                        if (map[y, next.X] != ' ')
                            return new Point2(next.X, y);
                }
            }

            return next;
        }

        private static Point2 GetNext2(char[,] map, Point2 position, Point2 direction)
        {
            /*    |    x0     |    x1   |     y0    |    y1     |
             * ---+-----------+---------+-----------+-----------+
             * x0 |###########| (x+l,y) | (y,x)     | (l-y,l-x) |
             * ---+-----------+---------+-----------+-----------+
             * x1 | (x-l,y)   | (x,l-y) | (l-y,l-x) |########## |
             * ---+-----------+---------+-----------+-----------+
             * y0 | (y,x)     | (l,h-x) |###########| (l-x,y)   |
             * ---+-----------+---------+-----------+-----------+
             * y1 | (l-y,l-x) |#########| (l-x,y)   | (l-x,y)   |
             * ---+-----------+---------+-----------+-----------+
             * 
             */





            var next = new Point2(Euclid.Modulus(position.X + direction.X, map.GetLength(1)), Euclid.Modulus(position.Y + direction.Y, map.GetLength(0)));
            if (map[next.Y, next.X] == ' ')
            {
                if (direction.X == 1)
                {
                    for (var x = 0; x < map.GetLength(1); x++)
                        if (map[next.Y, x] != ' ')
                            return new Point2(x, next.Y);
                }
                else if (direction.X == -1)
                {
                    for (var x = map.GetLength(1) - 1; x >= 0; x--)
                        if (map[next.Y, x] != ' ')
                            return new Point2(x, next.Y);
                }
                else if (direction.Y == 1)
                {
                    for (var y = 0; y < map.GetLength(0); y++)
                        if (map[y, next.X] != ' ')
                            return new Point2(next.X, y);
                }
                else if (direction.Y == -1)
                {
                    for (var y = map.GetLength(0) - 1; y >= 0; y--)
                        if (map[y, next.X] != ' ')
                            return new Point2(next.X, y);
                }
            }

            return next;
        }

        private static Point2 FindLeftMostOpenTile(char[,] map)
        {
            for (var x = 0; x < map.GetLength(1); x++)
                if (map[0, x] == '.')
                    return new Point2(x, 0);

            return Point2.Zero;
        }

        public (char[,], string) ReadGridWithInstructions(List<string> lines)
        {
            var breakIndex = lines.IndexOf(string.Empty);
            if (breakIndex == -1)
                throw new FormatException("Input does not have a break.");

            var map = new char[breakIndex, lines[0].Length];
            for (var y = 0; y < breakIndex; y++)
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    map[y, x] = x < lines[y].Length ? lines[y][x] : ' ';
                }

            return (map, lines[breakIndex + 1]);
        }
    }
}
