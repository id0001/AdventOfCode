using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadAllTextAsync(3);
        var mulls = input.ExtractAll(@"mul\((\d+),(\d+)\)");
        return mulls.Sum(digits => digits.As<int>().Product()).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var enabled = true;
        var input = await inputReader.ReadAllTextAsync(3);
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] != ')')
                continue; // Optimized

            if (input[..(i + 1)].EndsWith("don't()"))
            {
                enabled = false;
                continue;
            }

            if (input[..(i + 1)].EndsWith("do()"))
            {
                enabled = true;
                continue;
            }

            if(!enabled)
                continue;

            if (input[..(i + 1)].TryExtract<int>(@"mul\((\d+),(\d+)\)$", out var numbers))
                sum += numbers.Product();
        }

        return sum.ToString();
    }
}