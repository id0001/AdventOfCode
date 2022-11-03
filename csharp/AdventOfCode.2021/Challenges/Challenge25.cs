using AdventOfCode.Lib;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(25)]
public class Challenge25
{
    private readonly IInputReader _inputReader;

    public Challenge25(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(25);

        var width = grid.GetLength(1);
        var height = grid.GetLength(0);

        Dictionary<Point2, char> set1;
        var set2 = Enumerable.Range(0, height)
            .SelectMany(y =>
                Enumerable.Range(0, width).Where(x => grid[y, x] != '.')
                    .Select(x => (Location: new Point2(x, y), Value: grid[y, x])))
            .ToDictionary(kv => kv.Location, kv => kv.Value);
        var step = 0;
        do
        {
            set1 = set2;
            set2 = new Dictionary<Point2, char>();

            foreach (var cucumber in set1.Where(kv => kv.Value == '>'))
            {
                var next = GetPointEastOf(cucumber.Key, width);
                next = set1.ContainsKey(next) ? cucumber.Key : next;
                set2.Add(next, cucumber.Value);
            }

            foreach (var cucumber in set1.Where(kv => kv.Value == 'v'))
            {
                var next = GetPointSouthOf(cucumber.Key, height);
                next = (set1.ContainsKey(next) && set1[next] == 'v') || (set2.ContainsKey(next) && set2[next] == '>')
                    ? cucumber.Key
                    : next;
                set2.Add(next, cucumber.Value);
            }

            step++;
        } while (!set1.Keys.ToHashSet().SetEquals(set2.Keys));

        return step.ToString();
    }

    private static Point2 GetPointEastOf(Point2 p, int width) => p.X + 1 == width ? new Point2(0, p.Y) : new Point2(p.X + 1, p.Y);

    private static Point2 GetPointSouthOf(Point2 p, int height) => p.Y + 1 == height ? new Point2(p.X, 0) : new Point2(p.X, p.Y + 1);
}