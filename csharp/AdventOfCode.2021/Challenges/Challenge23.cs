using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode.Lib.Pathfinding;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(23)]
    public class Challenge23
    {
        private readonly IInputReader inputReader;
        private char[,] map;
        private State initialState;

        private Point2[] oddSpaces = new[] { new Point2(3, 1), new Point2(5, 1), new Point2(7, 1), new Point2(9, 1) };

        private Dictionary<char, int> FishCost = new Dictionary<char, int>
        {
            { 'A', 1},
            { 'B', 10},
            { 'C', 100},
            { 'D', 1000}
        };


        public Challenge23(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            map = await inputReader.ReadGridAsync(23);

            var a = new List<Fish>();
            var b = new List<Fish>();
            var c = new List<Fish>();
            var d = new List<Fish>();

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    char ch = map[y, x];
                    switch (ch)
                    {
                        case 'A':
                            a.Add(new Fish('A', new Point2(x, y)));
                            break;
                        case 'B':
                            b.Add(new Fish('B', new Point2(x, y)));
                            break;
                        case 'C':
                            c.Add(new Fish('C', new Point2(x, y)));
                            break;
                        case 'D':
                            d.Add(new Fish('D', new Point2(x, y)));
                            break;
                        default:
                            break;
                    }
                }
            }

            initialState = new State(a[0], a[1], b[0], b[1], c[0], c[1], d[0], d[1]);
        }

        [Part1]
        public string Part1()
        {
            var dijkstra = new Dijkstra<State>(GetAdjecent);
            if (dijkstra.TryPath(initialState, GoalReached, out var result))
            {
                return result.Cost.ToString();
            }

            return null;
        }

        [Part2]
        public string Part2()
        {
            return null;
        }

        private IEnumerable<(State, int)> GetAdjecent(State state)
        {
            var fishes = state.Fishes.ToArray();
            //Print(state);
            foreach (var fish in fishes)
            {
                if (IsInCorrectRoom(fish) && GetFishesInRoom(fish.Type, fishes).All(x => x == fish.Type))
                    continue;

                var visited = ImmutableHashSet.Create(fishes.Select(f => f.Position).ToArray());
                var moves = FindPossibleMoves(fishes.ToArray(), fish, fish.Position, visited, ImmutableArray.Create<Point2>());

                foreach (var move in moves)
                {
                    var newState = GetNewState(state, fish.Position, move.Target);
                    yield return (newState, move.Cost);
                }
            }
        }

        private bool IsInHallway(Point2 p) => p.Y == 1;

        private bool IsInRoom(Point2 p) => p.Y == 2 || p.Y == 3;

        private bool IsWall(Point2 p) => map[p.Y, p.X] == '#';

        private char GetRoom(Point2 p) => p.Y == 1 ? '-' : p.X switch
        {
            3 => 'A',
            5 => 'B',
            7 => 'C',
            9 => 'D',
            _ => throw new InvalidOperationException()
        };

        private bool IsInCorrectRoom(Fish fish) => (fish.Position.Y == 2 || fish.Position.Y == 3) && fish.Type switch
        {
            'A' => fish.Position.X == 3,
            'B' => fish.Position.X == 5,
            'C' => fish.Position.X == 7,
            'D' => fish.Position.X == 9,
            _ => throw new InvalidOperationException()
        };

        private char[] GetFishesInRoom(char room, Fish[] fishes) => room switch
        {
            'A' => new[] { fishes.FirstOrDefault(f => f.Position == new Point2(3, 3))?.Type ?? '-', fishes.FirstOrDefault(f => f.Position == new Point2(3, 2))?.Type ?? '-' },
            'B' => new[] { fishes.FirstOrDefault(f => f.Position == new Point2(5, 3))?.Type ?? '-', fishes.FirstOrDefault(f => f.Position == new Point2(5, 2))?.Type ?? '-' },
            'C' => new[] { fishes.FirstOrDefault(f => f.Position == new Point2(7, 3))?.Type ?? '-', fishes.FirstOrDefault(f => f.Position == new Point2(7, 2))?.Type ?? '-' },
            'D' => new[] { fishes.FirstOrDefault(f => f.Position == new Point2(9, 3))?.Type ?? '-', fishes.FirstOrDefault(f => f.Position == new Point2(9, 2))?.Type ?? '-' },
            _ => throw new InvalidOperationException()
        };

        private bool GoalReached(State state) =>
            IsInCorrectRoom(state.A1) &&
            IsInCorrectRoom(state.A2) &&
            IsInCorrectRoom(state.B1) &&
            IsInCorrectRoom(state.B2) &&
            IsInCorrectRoom(state.C1) &&
            IsInCorrectRoom(state.C2) &&
            IsInCorrectRoom(state.D1) &&
            IsInCorrectRoom(state.D2);

        private State GetNewState(State oldState, Point2 oldPosition, Point2 newPosition) => oldPosition switch
        {
            Point2 p when p == oldState.A1.Position => oldState with { A1 = new Fish('A', newPosition) },
            Point2 p when p == oldState.A2.Position => oldState with { A2 = new Fish('A', newPosition) },
            Point2 p when p == oldState.B1.Position => oldState with { B1 = new Fish('B', newPosition) },
            Point2 p when p == oldState.B2.Position => oldState with { B2 = new Fish('B', newPosition) },
            Point2 p when p == oldState.C1.Position => oldState with { C1 = new Fish('C', newPosition) },
            Point2 p when p == oldState.C2.Position => oldState with { C2 = new Fish('C', newPosition) },
            Point2 p when p == oldState.D1.Position => oldState with { D1 = new Fish('D', newPosition) },
            Point2 p when p == oldState.D2.Position => oldState with { D2 = new Fish('D', newPosition) },
            _ => throw new InvalidOperationException()
        };

        private IEnumerable<Move> FindPossibleMoves(Fish[] fishes, Fish fish, Point2 from, IImmutableSet<Point2> visited, ImmutableArray<Point2> path)
        {
            bool startFromHallway = IsInHallway(fish.Position);

            var list = new List<Move>();
            foreach (var neighbor in from.GetNeighbors4().Where(p => !visited.Contains(p) && !IsWall(p)))
            {
                var newPath = path.Add(neighbor);
                var newVisited = visited.Add(neighbor);
                list.AddRange(FindPossibleMoves(fishes, fish, neighbor, newVisited, newPath));

                //if (startFromHallway)
                //{
                //    if (IsInRoom(neighbor) && !IsInCorrectRoom(new Fish(fish.Type, neighbor)))
                //        continue;
                //}
                //else
                //{
                //    if (IsInRoom(neighbor) && !IsInCorrectRoom(new Fish(fish.Type, neighbor)))
                //        continue;
                //}

                if (IsInRoom(neighbor) && !IsInCorrectRoom(new Fish(fish.Type, neighbor)))
                    continue;

                if (IsInCorrectRoom(new Fish(fish.Type, neighbor)))
                {
                    char atBottom = fishes.FirstOrDefault(x => x.Position == new Point2(neighbor.X, 3))?.Type ?? '-';
                    if (atBottom != '-' && atBottom != fish.Type)
                        continue;

                    if (atBottom == '-' && neighbor.Y == 2)
                        continue;
                }

                if (oddSpaces.Contains(neighbor))
                    continue;

                int cost = newPath.Length * FishCost[fish.Type];
                list.Add(new Move(neighbor, cost));
            }

            return list;
        }

        private void Print(State state)
        {
            Fish[] fishes = { state.A1, state.A2, state.B1, state.B2, state.C1, state.C2, state.D1, state.D2 };

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Fish f = fishes.FirstOrDefault(e => e.Position == new Point2(x, y));
                    if (f is object)
                    {
                        Console.Write(f.Type);
                    }
                    else if (map[y, x] == '#')
                        Console.Write('#');
                    else
                        Console.Write('.');
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private record State(Fish A1, Fish A2, Fish B1, Fish B2, Fish C1, Fish C2, Fish D1, Fish D2)
        {
            public IEnumerable<Fish> Fishes
            {
                get
                {
                    yield return A1;
                    yield return A2;
                    yield return B1;
                    yield return B2;
                    yield return C1;
                    yield return C2;
                    yield return D1;
                    yield return D2;
                }
            }
        }

        private record Fish(char Type, Point2 Position);

        private record Move(Point2 Target, int Cost);
    }
}
