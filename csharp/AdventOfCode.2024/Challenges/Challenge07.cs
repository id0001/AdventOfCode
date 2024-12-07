using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() =>
        (await inputReader
            .ParseLinesAsync(7, ParseInput)
            .Where(eq => IsCorrect(eq, 0, 0, false))
            .Select(eq => eq.Result)
            .SumAsync()
        ).ToString();

    [Part2]
    public async Task<string> Part2Async() =>
        (await inputReader
            .ParseLinesAsync(7, ParseInput)
            .Where(eq => IsCorrect(eq, 0, 0, true))
            .Select(eq => eq.Result)
            .SumAsync()
        ).ToString();

    private static bool IsCorrect(Equation equation, long result, int i, bool useConcat)
    {
        if (i == equation.Parts.Count)
            return result == equation.Result;

        if (result > equation.Result)
            return false;

        if (IsCorrect(equation, result + equation.Parts[i], i + 1, useConcat))
            return true;

        if (IsCorrect(equation, result * equation.Parts[i], i + 1, useConcat))
            return true;

        if (useConcat && IsCorrect(equation, long.Parse(result.ToString() + equation.Parts[i]), i + 1, useConcat))
            return true;

        return false;
    }

    private static Equation ParseInput(string input) => input
        .SplitBy(":")
        .Into(parts => new Equation(
            parts.First().As<long>(),
            parts.Second().SplitBy(" ").As<int>())
        );

    private record Equation(long Result, IList<int> Parts);
}