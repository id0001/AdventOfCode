using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(2)]
public class Challenge02
{
    private readonly IInputReader _inputReader;

    public Challenge02(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var resultSum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(2))
        {
            var split = line.Split(' ');
            var (other, you) = (char.Parse(split[0]), char.Parse(split[1]));
            resultSum += Score(you, RoundResult(other, you));
        }

        return resultSum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var resultSum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(2))
        {
            var split = line.Split(' ');
            var (other, you) = (char.Parse(split[0]), char.Parse(split[1]));
            you = Choose(other, you);
            resultSum += Score(you, RoundResult(other, you));
        }

        return resultSum.ToString();
    }

    private static int RoundResult(char other, char you)
    {
        return (you - 'X' - (other - 'A')) switch
        {
            0 => 3, // draw
            1 or -2 => 6, // win
            _ => 0 // lose
        };
    }

    private static int Score(char you, int roundResult)
    {
        return you - 'X' + 1 + roundResult;
    }

    private static char Choose(char other, char you)
    {
        return (other - 'A', you) switch
        {
            (var o, 'Z') => (char) ('X' + (o + 1) % 3),
            (var o, 'Y') => (char) ('X' + o),
            (var o, 'X') => (char) (Euclid.Modulus(o - 1, 3) + 'X'),
            _ => throw new NotImplementedException()
        };
    }
}