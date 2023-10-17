using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.Math;
using System.Text;

namespace AdventOfCode2022.Challenges
{
    [Challenge(17)]
    public class Challenge17
    {
        private readonly IInputReader _inputReader;

        public Challenge17(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var dirs = await _inputReader.ReadLineAsync(17).ToListAsync();

            var result = SimulateUntil(new PointCloud<Point2>(), dirs, 0, 0, 0, 0, state => state.FallenRocks == 2022);
            return result.CurrentHeight.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var dirs = await _inputReader.ReadLineAsync(17).ToListAsync();

            var targetRocks = 1_000_000_000_000;
            var cache = new Dictionary<StateKey, State>();

            // Detect cycle
            var cycleEnd = SimulateUntil(new PointCloud<Point2>(), dirs, 0, 0, 0, 0, state =>
            {
                if (cache.ContainsKey(state.Key))
                    return true;
                else
                {
                    cache.Add(state.Key, state);
                    return false;
                }
            });

            var cycleStart = cache[cycleEnd.Key];

            // Get the difference between the end and start of the cycle
            int diffRocks = cycleEnd.FallenRocks - cycleStart.FallenRocks;
            int diffHeight = cycleEnd.CurrentHeight - cycleStart.CurrentHeight;

            // Calculate the amount of cycles 
            long cycles = (targetRocks - cycleStart.FallenRocks) / diffRocks;

            // Calculate the height after the last cycle.
            long heightAfterLastCycle = cycleStart.CurrentHeight + (cycles * diffHeight);

            // Calculate the remaining rocks
            long fallenRocks = cycleStart.FallenRocks + (cycles * diffRocks);
            var remainingRocks = targetRocks - fallenRocks;

            // Simulate again from the given state until the desired amount of fallen rocks is reached
            var rest = SimulateUntil(new PointCloud<Point2>(), dirs, cycleEnd.Key.DirIndex, cycleEnd.Key.ShapeIndex + 1, 0, 0, state => state.FallenRocks == remainingRocks);

            // Add the remaining height to the height after the last cycle
            return (heightAfterLastCycle + rest.CurrentHeight).ToString();
        }

        private Point2 MoveHorizontal(PointCloud<Point2> cloud, Point2 position, char direction, Shape shape)
        {
            switch (direction)
            {
                case '>':
                    var pRight = new Point2(position.X + 1, position.Y);
                    if (!Collides(cloud, pRight, shape))
                        return pRight;
                    break;
                case '<':
                    var pLeft = new Point2(position.X - 1, position.Y);
                    if (!Collides(cloud, pLeft, shape))
                        return pLeft;
                    break;
            }

            return position;
        }

        private bool TryMoveDown(PointCloud<Point2> cloud, Point2 position, Shape shape, out Point2 newPosition)
        {
            newPosition = position;
            if (Collides(cloud, new Point2(position.X, position.Y - 1), shape))
                return false;

            newPosition = new Point2(position.X, position.Y - 1);
            return true;
        }

        private bool TryMove(PointCloud<Point2> cloud, Point2 position, char direction, Shape shape, out Point2 newPosition)
        {
            newPosition = MoveHorizontal(cloud, position, direction, shape);
            return TryMoveDown(cloud, newPosition, shape, out newPosition);
        }

        private bool Collides(PointCloud<Point2> cloud, Point2 position, Shape shape)
        {
            // Detect boundaries
            if (position.X < 0 || position.X + shape.Width > 7 || position.Y < 0)
                return true;

            // Collide with baked shapes
            for (int ly = 0; ly < shape.Height; ly++)
            {
                for (int lx = 0; lx < shape.Width; lx++)
                {
                    var p = new Point2(position.X + lx, position.Y + (shape.Height - 1 - ly));
                    if (shape.Map[ly, lx] && cloud.Contains(p))
                        return true;
                }
            }

            return false;
        }

        private void BakeShape(PointCloud<Point2> cloud, Point2 position, Shape shape)
        {
            for (int ly = 0; ly < shape.Height; ly++)
            {
                for (int lx = 0; lx < shape.Width; lx++)
                {
                    var p = new Point2(position.X + lx, position.Y + (shape.Height - 1 - ly));
                    if (shape.Map[ly, lx])
                        cloud.Set(p);
                }
            }
        }

        private short Hash(PointCloud<Point2> cloud, int currentHeight)
        {
            short code = 0;
            for (var x = 0; x < 7; x++)
            {
                if (cloud.Contains(new Point2(x, currentHeight - 1)))
                    code |= (short)(1 << (6 - x));
            }

            return code;
        }

        private State SimulateUntil(PointCloud<Point2> cloud, List<char> dirs, int dirIndex, int shapeIndex, int currentHeight, int fallenRocks, Func<State, bool> predicate)
        {
            var shape1 = new Shape(4, 1, new bool[1, 4] { { true, true, true, true } }); // -
            var shape2 = new Shape(3, 3, new bool[3, 3] { { false, true, false }, { true, true, true }, { false, true, false } }); // +
            var shape3 = new Shape(3, 3, new bool[3, 3] { { false, false, true }, { false, false, true }, { true, true, true } }); // L
            var shape4 = new Shape(1, 4, new bool[4, 1] { { true }, { true }, { true }, { true } }); // |
            var shape5 = new Shape(2, 2, new bool[2, 2] { { true, true }, { true, true } }); // #

            var shapes = new[] { shape1, shape2, shape3, shape4, shape5 };
            shapeIndex = Euclid.Modulus(shapeIndex, shapes.Length);
            dirIndex = Euclid.Modulus(dirIndex, dirs.Count);

            var position = new Point2(2, currentHeight + 3);

            while (true)
            {
                // Make the first 3 moves
                TryMove(cloud, position, dirs[Euclid.Modulus(dirIndex, dirs.Count)], shapes[shapeIndex], out position);
                TryMove(cloud, position, dirs[Euclid.Modulus(dirIndex + 1, dirs.Count)], shapes[shapeIndex], out position);
                TryMove(cloud, position, dirs[Euclid.Modulus(dirIndex + 2, dirs.Count)], shapes[shapeIndex], out position);

                dirIndex = Euclid.Modulus(dirIndex + 3, dirs.Count);

                while (TryMove(cloud, position, dirs[dirIndex], shapes[shapeIndex], out position))
                {
                    dirIndex = Euclid.Modulus(dirIndex + 1, dirs.Count);
                }

                dirIndex = Euclid.Modulus(dirIndex + 1, dirs.Count);

                BakeShape(cloud, position, shapes[shapeIndex]);
                currentHeight = Math.Max(currentHeight, position.Y + shapes[shapeIndex].Height);
                fallenRocks++;

                var key = new StateKey(Hash(cloud, currentHeight), dirIndex, shapeIndex);
                var state = new State(key, fallenRocks, currentHeight);
                if (predicate(state))
                    return state;

                position = new Point2(2, currentHeight + 3);
                shapeIndex = Euclid.Modulus(shapeIndex + 1, shapes.Length);
            }
        }

        private void PrintState(PointCloud<Point2> cloud, int currentHeight, Shape shape, Point2 position)
        {
            var sb = new StringBuilder();
            for (int y = currentHeight + 3 + shape.Height; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    var p = new Point2(x, y);

                    if (p.X >= position.X && p.X < position.X + shape.Width && p.Y >= position.Y && p.Y < position.Y + shape.Height)
                    {
                        var lp = new Point2(p.X - position.X, (shape.Height - 1) - (p.Y - position.Y));
                        sb.Append(shape.Map[lp.Y, lp.X] ? '@' : '.');
                    }
                    else
                    {
                        sb.Append(cloud.Contains(p) ? '#' : '.');
                    }
                }

                Console.WriteLine(sb.ToString());
                sb.Clear();
            }

            Console.WriteLine();
        }

        private void PrintFinal(PointCloud<Point2> cloud, int currentHeight)
        {
            var sb = new StringBuilder();
            for (int y = currentHeight + 3; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    var p = new Point2(x, y);
                    sb.Append(cloud.Contains(p) ? '#' : '.');
                }

                Console.WriteLine(sb.ToString());
                sb.Clear();
            }

            Console.WriteLine();
        }

        private record Shape(int Width, int Height, bool[,] Map);

        private record StateKey(short Hash, int DirIndex, int ShapeIndex);

        private record State(StateKey Key, int FallenRocks, int CurrentHeight);
    }
}
