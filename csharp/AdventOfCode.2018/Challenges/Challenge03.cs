using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var cloud = new SparseSpatialMap<Point2, int, int>();
        await foreach (var claim in inputReader.ParseLinesAsync(3, ParseLine))
        foreach (var point in claim.Square.AsGridPoints())
            cloud[point]++;

        return cloud.Select(x => x.Value).Count(x => x > 1).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var claims = await inputReader.ParseLinesAsync(3, ParseLine).ToListAsync();
        return claims.First(c => !claims.Where(c2 => c != c2).Any(c2 => c.Square.IntersectsWith(c2.Square))).Id
            .ToString();
    }

    private Claim ParseLine(string line) => line
        .Extract<string, int, int, int, int>(@"#(.+) @ (\d+),(\d+): (\d+)x(\d+)")
        .Into(x => new Claim(x.First, new Rectangle(x.Second, x.Third, x.Fourth, x.Fifth)));

    private record Claim(string Id, Rectangle Square);
}