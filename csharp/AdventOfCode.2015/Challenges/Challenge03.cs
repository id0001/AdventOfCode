using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(3)]
public class Challenge03
{
    private readonly IInputReader _inputReader;

    public Challenge03(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var map = new Dictionary<Point2, int>();
        var location = Point2.Empty;
        
        await foreach (var c in _inputReader.ReadLineAsync(3))
        {
            map.AddOrUpdate(location, v => v + 1);
            location = c switch
            {
                '^' => new Point2(location.X, location.Y - 1),
                '>' => new Point2(location.X + 1, location.Y),
                'v' => new Point2(location.X, location.Y + 1),
                '<' => new Point2(location.X - 1, location.Y),
                _ => throw new NotSupportedException()
            };
        }

        return map.Count.ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var map = new Dictionary<Point2, int>();
        var locations = new[]{ Point2.Empty, Point2.Empty };
        int index = 0;
        
        await foreach (var c in _inputReader.ReadLineAsync(3))
        {
            map.AddOrUpdate(locations[index], v => v + 1);
            locations[index] = c switch
            {
                '^' => new Point2(locations[index].X, locations[index].Y - 1),
                '>' => new Point2(locations[index].X + 1, locations[index].Y),
                'v' => new Point2(locations[index].X, locations[index].Y + 1),
                '<' => new Point2(locations[index].X - 1, locations[index].Y),
                _ => throw new NotSupportedException()
            };

            index = (index + 1) % 2;
        }

        return map.Count.ToString();
    }
}