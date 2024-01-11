using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2019.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader InputReader)
{
    private readonly char[] _data = new char[5 * 5];

    [Setup]
    public async Task SetupAsync()
    {
        var i = 0;
        await foreach (var line in InputReader.ReadLinesAsync(24))
        foreach (var c in line)
            _data[i++] = c;
    }

    [Part1]
    public string Part1()
    {
        var automata = new char[2][];
        automata[0] = _data;
        automata[1] = new char[5 * 5];

        var current = 0; // current index
        var next = 1; // next index

        var history = new HashSet<string>();
        var dataString = new string(automata[current]);
        while (!history.Contains(dataString))
        {
            history.Add(dataString);

            for (var y = 0; y < 5; y++)
            for (var x = 0; x < 5; x++)
            {
                var p = new Point2(x, y); // current point
                var i = y * 5 + x;
                var bugCount = 0;

                foreach (var neighbor in p.GetNeighbors())
                {
                    if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= 5 || neighbor.Y >= 5)
                        continue;

                    if (!((neighbor.X == x) ^ (neighbor.Y == y)))
                        continue;

                    var ni = neighbor.Y * 5 + neighbor.X;

                    if (automata[current][ni] == '#')
                        bugCount++;
                }

                automata[next][i] = automata[current][i] switch
                {
                    '#' when bugCount != 1 => '.',
                    '.' when bugCount is >= 1 and <= 2 => '#',
                    _ => automata[current][i]
                };
            }

            current = next;
            next = Euclid.Modulus(next + 1, 2);
            dataString = new string(automata[current]);
        }

        return GetBiodiversity(automata[current]).ToString();
    }

    [Part2]
    public string Part2()
    {
        const int totalGen = 200;
        var upperBound = 1;
        var lowerBound = -1;

        var current = new Dictionary<int, char[]>
        {
            {0, _data},
            {-1, EmptyGrid()},
            {1, EmptyGrid()}
        };

        var emptyGridString = new string(EmptyGrid());

        for (var gen = 0; gen < totalGen; gen++)
        {
            var next = new Dictionary<int, char[]>();

            for (var level = lowerBound; level <= upperBound; level++)
            {
                if (!next.ContainsKey(level))
                    next.Add(level, EmptyGrid());

                for (var y = 0; y < 5; y++)
                for (var x = 0; x < 5; x++)
                {
                    if (x == 2 && y == 2)
                        continue;

                    var currentPoint = new Point3(x, y, level);
                    var bugCount = 0;

                    foreach (var neighbor in GetAdjacent(currentPoint))
                    {
                        if (!current.ContainsKey(neighbor.Z))
                            current.Add(neighbor.Z, EmptyGrid());

                        if (current[neighbor.Z][neighbor.Y * 5 + neighbor.X] == '#')
                            bugCount++;
                    }

                    next[level][y * 5 + x] = current[level][y * 5 + x] switch
                    {
                        '#' when bugCount != 1 => '.',
                        '.' when bugCount is >= 1 and <= 2 => '#',
                        _ => current[level][y * 5 + x]
                    };
                }
            }

            lowerBound--;
            upperBound++;

            // reduce levels
            if (!current.ContainsKey(lowerBound + 1) || new string(current[lowerBound + 1]) == emptyGridString)
            {
                current.Remove(lowerBound);
                lowerBound++;
            }

            if (!current.ContainsKey(upperBound - 1) || new string(current[upperBound - 1]) == emptyGridString)
            {
                current.Remove(upperBound);
                upperBound--;
            }

            current = next;
        }

        var bc = current.Sum(kv => kv.Value.Count(x => x == '#'));
        return bc.ToString();
    }

    private static IEnumerable<Point3> GetAdjacent(Point3 p)
    {
        var p2 = new Point2(p.X, p.Y);
        foreach (var neighbor in p2.GetNeighbors())
        {
            if (!((neighbor.X == p2.X) ^ (neighbor.Y == p2.Y))) // No diagonals
                continue;

            // Handle outer recursion
            if (neighbor.X == -1)
            {
                yield return new Point3(1, 2, p.Z - 1);
            }
            else if (neighbor.X == 5)
            {
                yield return new Point3(3, 2, p.Z - 1);
            }
            else if (neighbor.Y == -1)
            {
                yield return new Point3(2, 1, p.Z - 1);
            }
            else if (neighbor.Y == 5)
            {
                yield return new Point3(2, 3, p.Z - 1);
            }
            else if (neighbor.X == 2 && neighbor.Y == 2)
            {
                if (p.X == 1)
                    for (var y = 0; y < 5; y++)
                        yield return new Point3(0, y, p.Z + 1);
                else if (p.X == 3)
                    for (var y = 0; y < 5; y++)
                        yield return new Point3(4, y, p.Z + 1);
                else if (p.Y == 1)
                    for (var x = 0; x < 5; x++)
                        yield return new Point3(x, 0, p.Z + 1);
                else if (p.Y == 3)
                    for (var x = 0; x < 5; x++)
                        yield return new Point3(x, 4, p.Z + 1);
            }
            else
            {
                yield return new Point3(neighbor.X, neighbor.Y, p.Z);
            }
        }
    }

    private static int GetBiodiversity(IReadOnlyList<char> data)
    {
        var diversity = 0;
        for (var i = 0; i < data.Count; i++)
            if (data[i] == '#')
                diversity += (int) Math.Pow(2, i);

        return diversity;
    }

    private static char[] EmptyGrid()
    {
        var grid = new char[5 * 5];
        Array.Fill(grid, '.');
        return grid;
    }
}