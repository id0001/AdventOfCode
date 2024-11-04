using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2018.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = (await inputReader.ReadAllTextAsync(20))[1..^1];
        var map = MapRooms(input);

        var astar = new AStar<Room>(r => GetAdjacent(map, r));
        var distances = astar.CalculateDistances(map.Get(Point2.Zero)!);

        return distances.Values.Max().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = (await inputReader.ReadAllTextAsync(20))[1..^1];
        var map = MapRooms(input);

        var astar = new AStar<Room>(r => GetAdjacent(map, r));
        var distances = astar.CalculateDistances(map.Get(Point2.Zero)!);

        return distances.Values.Count(v => v >= 1000).ToString();
    }

    private static IEnumerable<Room> GetAdjacent(SparseSpatialMap<Point2, int, Room> map, Room current)
    {
        if (current.Doors[0])
            yield return map[current.Location.Up]!;
        if (current.Doors[1])
            yield return map[current.Location.Right]!;
        if (current.Doors[2])
            yield return map[current.Location.Down]!;
        if (current.Doors[3])
            yield return map[current.Location.Left]!;
    }

    private SparseSpatialMap<Point2, int, Room> MapRooms(string input)
    {
        SparseSpatialMap<Point2, int, Room> map = new();
        Stack<Room> stack = new();

        var from = map.GetOrAdd(Point2.Zero, new Room(Point2.Zero));
        stack.Push(from);

        for (var i = 0; i < input.Length; i++)
        {
            Room to;
            switch (input[i])
            {
                case 'N':
                    to = map.GetOrAdd(from.Location.Up, new Room(from.Location.Up));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'E':
                    to = map.GetOrAdd(from.Location.Right, new Room(from.Location.Right));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'S':
                    to = map.GetOrAdd(from.Location.Down, new Room(from.Location.Down));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'W':
                    to = map.GetOrAdd(from.Location.Left, new Room(from.Location.Left));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case '(':
                    stack.Push(from);
                    break;
                case ')':
                    from = stack.Pop();
                    break;
                case '|':
                    from = stack.Peek();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return map;
    }

    private void MoveBetween(Room from, Room to)
    {
        var dir = to.Location - from.Location;

        if (dir == Face.Up)
        {
            to.Doors[2] = true;
            from.Doors[0] = true;
            return;
        }

        if (dir == Face.Right)
        {
            to.Doors[3] = true;
            from.Doors[1] = true;
            return;
        }

        if (dir == Face.Down)
        {
            to.Doors[0] = true;
            from.Doors[2] = true;
            return;
        }

        if (dir == Face.Left)
        {
            to.Doors[1] = true;
            from.Doors[3] = true;
            return;
        }
    }

    private class Room(Point2 location)
    {
        public Point2 Location { get; } = location;

        public bool[] Doors { get; } = new bool[4];
    }
}