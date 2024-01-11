using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2020.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader InputReader)
{
    private static readonly IReadOnlyDictionary<string, Point3> Neighbors = new Dictionary<string, Point3>
    {
        {"nw", new Point3(0, +1, -1)},
        {"ne", new Point3(1, 0, -1)},
        {"e", new Point3(1, -1, 0)},
        {"se", new Point3(0, -1, 1)},
        {"sw", new Point3(-1, 0, 1)},
        {"w", new Point3(-1, 1, 0)}
    };

    private readonly List<List<string>> _input = new();

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in InputReader.ReadLinesAsync(24))
        {
            var steps = new List<string>();
            for (var i = 0; i < line.Length; i++)
                if (line[i] == 'n' || line[i] == 's')
                {
                    steps.Add(line.Substring(i, 2));
                    i++;
                }
                else
                {
                    steps.Add(line[i].ToString());
                }

            _input.Add(steps);
        }
    }

    [Part1]
    public string Part1()
    {
        var floor = BuildFloorFromInput();

        return floor.Count(kv => kv.Value).ToString();
    }

    [Part2]
    public string Part2()
    {
        var floor = BuildFloorFromInput();

        for (var i = 0; i < 100; i++)
        {
            var newFloor = new Dictionary<Point3, bool>();

            var toCheck = new HashSet<Point3>();
            foreach (var tile in floor)
            {
                toCheck.Add(tile.Key);
                foreach (var n in Neighbors)
                {
                    var p = tile.Key + n.Value;
                    toCheck.Add(p);
                }
            }

            foreach (var tile in toCheck)
            {
                var value = floor.TryGetValue(tile, out var value1) && value1;

                var count = 0;
                foreach (var n in Neighbors)
                {
                    var p = tile + n.Value;

                    if (floor.TryGetValue(p, out var v) && v)
                        count++;
                }

                if (value && (count == 0 || count > 2))
                    newFloor.Add(tile, false);
                else if (!value && count == 2)
                    newFloor.Add(tile, true);
                else
                    newFloor.Add(tile, value);
            }

            floor = newFloor;
        }

        return floor.Count(kv => kv.Value).ToString();
    }

    private IDictionary<Point3, bool> BuildFloorFromInput()
    {
        var tileDict = new Dictionary<Point3, bool>();

        foreach (var line in _input)
        {
            var p = FindTile(line);
            if (!tileDict.ContainsKey(p))
                tileDict.Add(p, true);
            else
                tileDict[p] = !tileDict[p];
        }

        return tileDict;
    }

    private Point3 FindTile(IEnumerable<string> steps)
    {
        var p = Point3.Zero;

        foreach (var step in steps) p += Neighbors[step];

        return p;
    }
}