using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader InputReader)
{
    [Part1]
    public async Task<string?> Part1Async() =>
        (await InputReader
            .ParseLinesAsync(2, ParseLine)
            .SumAsync(TotalPaperNeeded))
        .ToString();

    [Part2]
    public async Task<string?> Part2Async() =>
        (await InputReader
            .ParseLinesAsync(2, ParseLine)
            .SumAsync(TotalRibbonNeeded))
        .ToString();

    private static Cube ParseLine(string line) => line
        .SplitBy("x")
        .As<int>()
        .Order()
        .ToArray()
        .Into(x => new Cube(0, 0, 0, x.First(), x.Second(), x.Third()));

    private static int TotalPaperNeeded(Cube cube) => cube.TotalSurfaceArea + cube.SmallestArea;

    private static int TotalRibbonNeeded(Cube cube) => cube.SmallestPerimeter + cube.Volume;
}