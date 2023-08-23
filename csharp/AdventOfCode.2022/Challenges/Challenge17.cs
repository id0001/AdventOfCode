using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using System.Net.WebSockets;
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

        //[Part1]
        //public async Task<string> Part1Async()
        //{
        //    var dirs = await _inputReader.ReadLineAsync(17).ToArrayAsync();

        //    var space = new SparseSpatialMap<Point2, bool>();
        //    var position = new Point2(2, 3);
        //    Point2 nextPosition;
        //    int stoppedRocks = 0;
        //    int shapeIndex = 0;
        //    int dirIndex = 0;

        //    Shape currentShape = GetShape(Shapes[shapeIndex]);

        //    do
        //    {
        //        switch (dirs[dirIndex])
        //        {
        //            case '<':
        //                nextPosition = new Point2(position.X - 1, position.Y);
        //                if (nextPosition.X < 0)
        //                    break;
        //                if (!Collides(space, currentShape, nextPosition))
        //                    position = nextPosition;
        //                break;
        //            case '>':
        //                nextPosition = new Point2(position.X + 1, position.Y);
        //                if (nextPosition.X + currentShape.Width > 7)
        //                    break;
        //                if (!Collides(space, currentShape, nextPosition))
        //                    position = nextPosition;
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }

        //        nextPosition = new Point2(position.X, position.Y - 1);
        //        if (nextPosition.Y + (currentShape.Height-1) >= 0 && !Collides(space, currentShape, nextPosition))
        //            position = nextPosition;
        //        else
        //        {
        //            LockShape(space, currentShape, position);
        //            stoppedRocks++;
        //            shapeIndex = (shapeIndex + 1) % Shapes.Length;
        //            currentShape = GetShape(Shapes[shapeIndex]);

        //            position = new Point2(2, space.Bounds.GetMax(1) + currentShape.Height + 2);
        //        }

        //        dirIndex = (dirIndex + 1) % dirs.Length;
        //    }
        //    while (stoppedRocks < 2022);

        //    return space.Bounds.GetMax(1).ToString();
        //}

        [Part2]
        public async Task<string> Part2Async()
        {
            var dirs = await _inputReader.ReadLineAsync(17).ToArrayAsync();

            var space = new SparseSpatialMap<Point2, bool>();
            var position = new Point2(2, 3);
            Point2 nextPosition;
            long stoppedRocks = 0;
            int shapeIndex = 0;
            int dirIndex = 0;

            Shape currentShape = GetShape(Shapes[shapeIndex]);

            do
            {
                switch (dirs[dirIndex])
                {
                    case '<':
                        nextPosition = new Point2(position.X - 1, position.Y);
                        if (nextPosition.X < 0)
                            break;
                        if (!Collides(space, currentShape, nextPosition))
                            position = nextPosition;
                        break;
                    case '>':
                        nextPosition = new Point2(position.X + 1, position.Y);
                        if (nextPosition.X + currentShape.Width > 7)
                            break;
                        if (!Collides(space, currentShape, nextPosition))
                            position = nextPosition;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                nextPosition = new Point2(position.X, position.Y - 1);
                if (nextPosition.Y + (currentShape.Height - 1) >= 0 && !Collides(space, currentShape, nextPosition))
                    position = nextPosition;
                else
                {
                    LockShape(space, currentShape, position);
                    stoppedRocks++;
                    shapeIndex = (shapeIndex + 1) % Shapes.Length;
                    currentShape = GetShape(Shapes[shapeIndex]);

                    position = new Point2(2, space.Bounds.GetMax(1) + currentShape.Height + 2);
                }

                dirIndex = (dirIndex + 1) % dirs.Length;

                //if (dirIndex == 0 && shapeIndex == 0)
                //    break;
            }
            while (stoppedRocks < 1697);

            Print(space);

            var cycles = 1000000000000L / stoppedRocks;
            var remaining = 1000000000000L % stoppedRocks;
            var currentHeight = space.Bounds.GetMax(1);

            var finalHeight = currentHeight * cycles - ((cycles-1) * 2);

            var remHeight = await ExecuteDropsAsync((int)remaining);

            return (finalHeight + remHeight - 2).ToString();
        }

        private async Task<int> ExecuteDropsAsync(int amount)
        {
            var dirs = await _inputReader.ReadLineAsync(17).ToArrayAsync();

            var space = new SparseSpatialMap<Point2, bool>();
            var position = new Point2(2, 3);
            Point2 nextPosition;
            long stoppedRocks = 0;
            int shapeIndex = 0;
            int dirIndex = 0;

            Shape currentShape = GetShape(Shapes[shapeIndex]);

            do
            {
                switch (dirs[dirIndex])
                {
                    case '<':
                        nextPosition = new Point2(position.X - 1, position.Y);
                        if (nextPosition.X < 0)
                            break;
                        if (!Collides(space, currentShape, nextPosition))
                            position = nextPosition;
                        break;
                    case '>':
                        nextPosition = new Point2(position.X + 1, position.Y);
                        if (nextPosition.X + currentShape.Width > 7)
                            break;
                        if (!Collides(space, currentShape, nextPosition))
                            position = nextPosition;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                nextPosition = new Point2(position.X, position.Y - 1);
                if (nextPosition.Y + (currentShape.Height - 1) >= 0 && !Collides(space, currentShape, nextPosition))
                    position = nextPosition;
                else
                {
                    LockShape(space, currentShape, position);
                    stoppedRocks++;
                    shapeIndex = (shapeIndex + 1) % Shapes.Length;
                    currentShape = GetShape(Shapes[shapeIndex]);

                    position = new Point2(2, space.Bounds.GetMax(1) + currentShape.Height + 2);
                }

                dirIndex = (dirIndex + 1) % dirs.Length;

                if (dirIndex == 0 && shapeIndex == 0)
                    break;
            }
            while (stoppedRocks < amount);

            return space.Bounds.GetMax(1);
        }

        private static void Print(SparseSpatialMap<Point2, bool> space)
        {
            StringBuilder sw = new StringBuilder();
            for (int y = space.Bounds.GetMax(1); y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                    sw.Append(space[new Point2(x, y)] ? '#' : '.');

                if (sw.ToString().Any(x => x == '#'))
                    Console.WriteLine(sw.ToString());

                sw.Clear();
            }
        }

        public bool Collides(SparseSpatialMap<Point2, bool> space, Shape shape, Point2 next)
        {
            for (int y = 0; y < shape.Height; y++)
            {
                for (int x = 0; x < shape.Width; x++)
                {
                    if (shape.Grid[y, x] && space[new Point2(next.X + x, next.Y - y)])
                        return true;
                }
            }

            return false;
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

        private static void LockShape(SparseSpatialMap<Point2, bool> space, Shape shape, Point2 position)
        {
            for (int y = 0; y < shape.Height; y++)
            {
                for (int x = 0; x < shape.Width; x++)
                {
                    if (shape.Grid[y, x])
                        space[new Point2(position.X + x, position.Y - y)] = true;
                }
            }
        }


        public record Shape(int Width, int Height, char Type, bool[,] Grid);
    }
}
