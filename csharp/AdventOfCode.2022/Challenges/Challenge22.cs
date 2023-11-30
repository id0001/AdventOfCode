using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using AdventOfCode.Lib.Misc;
using static AdventOfCode.Lib.Misc.CubeWalker;

namespace AdventOfCode2022.Challenges;

[Challenge(22)]
public class Challenge22
{
    private readonly Edge[] _edges;
    private readonly IDictionary<Face, Point2> _faceOffset;
    private readonly IInputReader _inputReader;

    public Challenge22(IInputReader inputReader)
    {
        _inputReader = inputReader;

        _faceOffset = new Dictionary<Face, Point2>
        {
            {Face.Top, new Point2(50, 0)},
            {Face.Right, new Point2(100, 0)},
            {Face.Front, new Point2(50, 50)},
            {Face.Left, new Point2(0, 100)},
            {Face.Bottom, new Point2(50, 100)},
            {Face.Back, new Point2(0, 150)}
        };

        _edges = new Edge[]
        {
            new(Face.Top, Face.Right, Side.Right, Side.Left),
            new(Face.Top, Face.Front, Side.Bottom, Side.Top),
            new(Face.Top, Face.Left, Side.Left, Side.Left),
            new(Face.Top, Face.Back, Side.Top, Side.Left),
            new(Face.Front, Face.Right, Side.Right, Side.Bottom),
            new(Face.Front, Face.Left, Side.Left, Side.Top),
            new(Face.Front, Face.Bottom, Side.Bottom, Side.Top),
            new(Face.Right, Face.Bottom, Side.Right, Side.Right),
            new(Face.Right, Face.Back, Side.Top, Side.Bottom),
            new(Face.Left, Face.Back, Side.Bottom, Side.Top),
            new(Face.Left, Face.Bottom, Side.Right, Side.Left),
            new(Face.Bottom, Face.Back, Side.Bottom, Side.Right)
        };
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await _inputReader.ReadLinesAsync(22).ToListAsync();
        var (map, line) = ReadGridWithInstructions(lines);

        var position = FindLeftMostOpenTile(map);
        var direction = new Point2(1, 0);
        var instructions = new Queue<char>(line);
        var steps = new StringBuilder();

        foreach (var instruction in instructions)
            (position, direction) = char.IsNumber(instruction) switch
            {
                true => AppendSteps(position, direction, steps, instruction),
                _ => MoveAndTurn(map, position, direction, steps, instruction)
            };

        position = Move(map, position, direction, int.Parse(steps.ToString()));

        var facing = GetFacing(direction);
        var rows = (position.Y + 1) * 1000;
        var cols = (position.X + 1) * 4;
        return (rows + cols + facing).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await _inputReader.ReadLinesAsync(22).ToListAsync();
        var (map, line) = ReadGridWithInstructions(lines);

        var position = new CubeCoord(Face.Top, Point2.Zero);
        var direction = new Point2(1, 0);
        var instructions = new Queue<char>(line);
        var steps = new StringBuilder();

        var cubeWalker = new CubeWalker(50, _edges);

        foreach (var instruction in instructions)
            (position, direction) = char.IsNumber(instruction) switch
            {
                true => AppendSteps(position, direction, steps, instruction),
                _ => MoveAndTurnPart2(map, cubeWalker, _faceOffset, position, direction, steps, instruction)
            };

        (position, direction) =
            MovePart2(map, cubeWalker, _faceOffset, position, direction, int.Parse(steps.ToString()));

        var facing = GetFacing(direction);
        var p = _faceOffset[position.Face] + position.Position;
        var rows = (p.Y + 1) * 1000;
        var cols = (p.X + 1) * 4;
        return (rows + cols + facing).ToString();
    }

    private static (TPosition, TDirection) AppendSteps<TPosition, TDirection>(TPosition position, TDirection direction, StringBuilder moveAmount, char c)
    {
        moveAmount.Append(c);
        return (position, direction);
    }

    private static (Point2, Point2) MoveAndTurn(char[,] map, Point2 position, Point2 direction, StringBuilder steps,
        char c)
    {
        if (steps.Length > 0)
            position = Move(map, position, direction, int.Parse(steps.ToString()));

        steps.Clear();

        direction = Turn(direction, c);

        return (position, direction);
    }

    private static (CubeCoord, Point2) MoveAndTurnPart2(char[,] map, CubeWalker cubeWalker,
        IDictionary<Face, Point2> faceOffset, CubeCoord position, Point2 direction, StringBuilder steps, char c)
    {
        if (steps.Length > 0)
            (position, direction) =
                MovePart2(map, cubeWalker, faceOffset, position, direction, int.Parse(steps.ToString()));

        steps.Clear();

        direction = Turn(direction, c);

        return (position, direction);
    }

    private static Point2 Turn(Point2 dir, char instruction)
    {
        return instruction switch
        {
            'L' => Point2.Turn(dir, Point2.Zero, -(Math.PI / 2d)),
            'R' => Point2.Turn(dir, Point2.Zero, Math.PI / 2d),
            _ => throw new NotImplementedException()
        };
    }

    private static Point2 Move(char[,] map, Point2 pos, Point2 dir, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var next = GetNextPosition(map, pos, dir);
            if (map[next.Y, next.X] == '.')
                pos = next;
        }

        return pos;
    }

    private static (CubeCoord, Point2) MovePart2(char[,] map, CubeWalker cubeWalker,
        IDictionary<Face, Point2> faceOffset, CubeCoord position, Point2 direction, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var (nextCube, nextDir) = cubeWalker.Move(position, direction);
            var next = faceOffset[nextCube.Face] + nextCube.Position;
            if (map[next.Y, next.X] == '.')
            {
                position = nextCube;
                direction = nextDir;
            }
        }

        return (position, direction);
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

    private static Point2 GetNextPosition(char[,] map, Point2 position, Point2 direction)
    {
        var next = new Point2(Euclid.Modulus(position.X + direction.X, map.GetLength(1)),
            Euclid.Modulus(position.Y + direction.Y, map.GetLength(0)));
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

        var width = 150;

        var map = new char[breakIndex, width];
        for (var y = 0; y < breakIndex; y++)
        for (var x = 0; x < map.GetLength(1); x++)
            map[y, x] = x < lines[y].Length ? lines[y][x] : ' ';

        return (map, lines[breakIndex + 1]);
    }
}