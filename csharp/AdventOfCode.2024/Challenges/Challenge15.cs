using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (map, robot, moves) = await inputReader.ParseTextAsync(15, ParseTextAsync);

        _ = moves.Aggregate(robot, (position, move) => Move1(map, position, move));
        return map.AsEnumerable().Where(kv => kv.Value == 'O').Sum(kv => 100 * kv.Key.Y + kv.Key.X).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (map, robot, moves) = await inputReader.ParseTextAsync(15, ParseTextAsync);
        map = ExpandMap(map);
        robot = robot with {X = robot.X * 2};

        _ = moves.Aggregate(robot, (position, move) => Move2(map, position, move));
        return map.AsEnumerable().Where(kv => kv.Value == '[').Sum(kv => 100 * kv.Key.Y + kv.Key.X).ToString();
    }

    private static Point2 Move1(char[,] map, Point2 robot, Point2 direction)
    {
        var next = robot + direction;
        return map[next.Y, next.X] switch
        {
            '#' => robot,
            '.' => next,
            'O' => MovePart1(map, robot, direction),
            _ => throw new NotImplementedException()
        };
    }

    private static Point2 Move2(char[,] map, Point2 robot, Point2 direction)
    {
        var next = robot + direction;
        return map[next.Y, next.X] switch
        {
            '#' => robot,
            '.' => next,
            '[' when direction == Face.Right => MoveHorizontally(map, robot, direction),
            ']' when direction == Face.Left => MoveHorizontally(map, robot, direction),
            '[' => MoveVertically(map, robot, next, next.Right, direction),
            ']' => MoveVertically(map, robot, next.Left, next, direction),
            _ => throw new NotImplementedException()
        };
    }

    private static Point2 MovePart1(char[,] map, Point2 robot, Point2 direction)
    {
        var next = robot + direction;
        while (map[next.Y, next.X] == 'O')
            next += direction;

        if (map[next.Y, next.X] == '#')
            return robot;

        do
        {
            var prev = next - direction;
            map[next.Y, next.X] = map[prev.Y, prev.X];
            next = prev;
        } while (map[next.Y, next.X] != '.');

        return robot + direction;
    }

    private static Point2 MoveHorizontally(char[,] map, Point2 robot, Point2 direction)
    {
        var next = robot + direction;
        while (map[next.Y, next.X] is '[' or ']')
            next += direction;

        if (map[next.Y, next.X] == '#')
            return robot;

        do
        {
            var prev = next - direction;
            map[next.Y, next.X] = map[prev.Y, prev.X];
            next = prev;
        } while (map[next.Y, next.X] != '.');

        return robot + direction;
    }

    private static Point2 MoveVertically(char[,] map, Point2 robot, Point2 boxLeft, Point2 boxRight, Point2 direction)
    {
        if (!CanMoveBox(map, boxLeft, boxRight, direction))
            return robot;

        MoveBoxes(map, boxLeft, boxRight, direction);
        return robot + direction;
    }

    private static bool CanMoveBox(char[,] map, Point2 boxLeft, Point2 boxRight, Point2 direction)
    {
        var nextLeft = boxLeft + direction;
        var nextRight = boxRight + direction;

        return (map[nextLeft.Y, nextLeft.X], map[nextRight.Y, nextRight.X]) switch
        {
            ('#', _) => false,
            (_, '#') => false,
            ('[', ']') when !CanMoveBox(map, nextLeft, nextRight, direction) => false,
            (']', _) when !CanMoveBox(map, nextLeft.Left, nextLeft, direction) => false,
            (_, '[') when !CanMoveBox(map, nextRight, nextRight.Right, direction) => false,
            _ => true
        };
    }

    private static void MoveBoxes(char[,] map, Point2 boxLeft, Point2 boxRight, Point2 direction)
    {
        var nextLeft = boxLeft + direction;
        var nextRight = boxRight + direction;

        if (map[nextLeft.Y, nextLeft.X] == ']')
            MoveBoxes(map, nextLeft.Left, nextLeft, direction);

        if (map[nextRight.Y, nextRight.X] == '[')
            MoveBoxes(map, nextRight, nextRight.Right, direction);

        if (map[nextLeft.Y, nextLeft.X] == '[')
            MoveBoxes(map, nextLeft, nextRight, direction);

        map[nextLeft.Y, nextLeft.X] = '[';
        map[nextRight.Y, nextRight.X] = ']';
        map[boxLeft.Y, boxLeft.X] = '.';
        map[boxRight.Y, boxRight.X] = '.';
    }

    private static char[,] ExpandMap(char[,] map)
    {
        var newMap = new char[map.GetLength(0), map.GetLength(1) * 2];
        for (var y = 0; y < map.GetLength(0); y++)
        for (var x = 0; x < map.GetLength(1); x++)
            switch (map[y, x])
            {
                case '#':
                    newMap[y, x * 2] = '#';
                    newMap[y, x * 2 + 1] = '#';
                    break;
                case '.':
                    newMap[y, x * 2] = '.';
                    newMap[y, x * 2 + 1] = '.';
                    break;
                case 'O':
                    newMap[y, x * 2] = '[';
                    newMap[y, x * 2 + 1] = ']';
                    break;
            }

        return newMap;
    }

    private (char[,] map, Point2, List<Point2>) ParseTextAsync(string input)
    {
        var paragraphs = input.SelectParagraphs();

        var robot = Point2.Zero;
        var map = new char[paragraphs[0].SelectLines().Count, paragraphs[0].SelectLines()[0].Length];
        for (var y = 0; y < map.GetLength(0); y++)
        for (var x = 0; x < map.GetLength(1); x++)
        {
            var c = paragraphs[0].SelectLines()[y][x];
            if (c == '@')
            {
                robot = new Point2(x, y);
                map[y, x] = '.';
                continue;
            }

            map[y, x] = paragraphs[0].SelectLines()[y][x];
        }

        var moves = paragraphs[1].SelectLines().SelectMany(line => line.ToCharArray()).Select(d => d switch
        {
            '>' => Face.Right,
            'v' => Face.Down,
            '<' => Face.Left,
            '^' => Face.Up,
            _ => throw new NotImplementedException()
        }).ToList();

        return (map, robot, moves);
    }
}