using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2024.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
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

    private record GroupTemplate(string Army, int Id, int Units, int Health, int Damage);
}
