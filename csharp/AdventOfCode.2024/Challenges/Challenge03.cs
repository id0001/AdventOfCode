using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(3)]
public partial class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadAllTextAsync(3);
        var mulls = input.ExtractAllValues<int>(@"mul\((\d+),(\d+)\)");
        return mulls.Sum(m => m.Product()).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadAllTextAsync(3);
        var matches = input.ExtractAll<int>(@"do\(\)|don't\(\)|mul\((\d+),(\d+)\)");
        var (sum, _) = matches.Aggregate((Sum: 0, Enabled: true), (acc, match) => (match.Match, acc.Enabled) switch
        {
            ("do()", _) => (acc.Sum, true),
            ("don't()", _) => (acc.Sum, false),
            (_, true)  => (acc.Sum + match.Values.Product(), true),
            (_, false) => (acc.Sum, false),
        });

        return sum.ToString();
    }
}