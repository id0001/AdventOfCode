using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(2)]
public class Challenge02
{
    private readonly IInputReader _inputReader;

    public Challenge02(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async() =>
        (await _inputReader
            .ParseLinesAsync(2, ParseLine)
            .SumAsync(TotalPaperNeeded))
        .ToString();

    [Part2]
    public async Task<string?> Part2Async() =>
        (await _inputReader
            .ParseLinesAsync(2, ParseLine)
            .SumAsync(TotalRibbonNeeded))
        .ToString();

    private static Cube ParseLine(string line)
    {
        var split = line.Split('x').Select(int.Parse).OrderBy(_ => _).ToArray();
        return new Cube(0, 0, 0, split[0], split[1], split[2]);
    }

    private static int TotalPaperNeeded(Cube cube) => cube.TotalSurfaceArea + cube.SmallestArea;

    private static int TotalRibbonNeeded(Cube cube) => cube.SmallestPerimeter + cube.Volume;
}