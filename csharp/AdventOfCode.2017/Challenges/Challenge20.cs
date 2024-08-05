using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

//[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        return string.Empty;
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
    }

    private static PVA ParseLine(string line) => line
        .Extract(@"p=<(.+)>, v=<(.+)>, a=<(.+)>")
        .Select(x => x.SplitBy(",").As<int>().Into(parts => new Point3(parts.First(), parts.Second(), parts.Third())))
        .ToList()
        .Into(parts => new PVA(parts.First(), parts.Second(), parts.Third()));

    private record PVA(Point3 Position, Point3 Velocity, Point3 Acceleration);
}