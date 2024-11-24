using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2024.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        return await inputReader.ReadAllTextAsync(0);
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
    }
}
