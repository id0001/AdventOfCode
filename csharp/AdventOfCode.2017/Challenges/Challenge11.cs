using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(11)]
public class Challenge11(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => Hex
        .ManhattanDistance(Hex.Zero, await inputReader.ReadLineAsync(11, ',').AggregateAsync(Hex.Zero, Next))
        .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadLineAsync(11, ',')
            .AggregateAsync((MaxDistance: 0, Hex: Hex.Zero), NextWithMaxDistance))
        .MaxDistance
        .ToString();

    private Hex Next(Hex current, string dir) => dir switch
    {
        "n" => current.North,
        "ne" => current.NorthEast,
        "se" => current.SouthEast,
        "s" => current.South,
        "sw" => current.SouthWest,
        "nw" => current.NorthWest,
        _ => throw new NotImplementedException()
    };

    private (int MaxDistance, Hex Hex) NextWithMaxDistance((int MaxDistance, Hex Hex) current, string dir)
    {
        var next = Next(current.Hex, dir);
        return (Math.Max(current.MaxDistance, Hex.ManhattanDistance(Hex.Zero, next)), next);
    }
}