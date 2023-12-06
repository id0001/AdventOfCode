using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(6)]
public class Challenge06
{
    private readonly IInputReader _inputReader;
    
    public Challenge06(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var (time, distance) = ParseInput(await _inputReader.ReadAllTextAsync(6));
        return time.Zip(distance).Select(zip => CountWinningDistances(zip.First, zip.Second)).Product().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (time, distance) = ParseInput2(await _inputReader.ReadAllTextAsync(6));
        return CountWinningDistances(time, distance).ToString();
    }

    private long CountWinningDistances(long time, long distance)
    {
        int c = 0;
        for (long i = 0; i <= time; i++)
        {
            var d = i * (time - i);
            if (d > distance)
                c++;
        }

        return c;
    }

    private (long[], long[]) ParseInput(string input)
    {
        var nl = Environment.NewLine;
        return input.SplitBy(nl, split => (
                split.First.SplitBy(":").Second.SplitBy(" ").Select(long.Parse).ToArray(),
                split.Second.SplitBy(":").Second.SplitBy(" ").Select(long.Parse).ToArray()
            )
        );
    }
    
    private (long, long) ParseInput2(string input)
    {
        var nl = Environment.NewLine;
        return input.SplitBy(nl, split => (
                int.Parse(split.First.SplitBy(":").Second.Replace(" ", "")),
                long.Parse(split.Second.SplitBy(":").Second.Replace(" ", ""))
            )
        );
    }
}
