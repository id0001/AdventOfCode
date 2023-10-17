using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2022.Challenges;

[Challenge(10)]
public class Challenge10
{
    private readonly IInputReader _inputReader;

    public Challenge10(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var cycle = 0;
        var x = 1;

        var totalSum = 0;
        var check = new HashSet<int> {20, 60, 100, 140, 180, 220};

        await foreach (var (opcode, value) in _inputReader.ParseLinesAsync(10, ParseLine))
        {
            Cycle1(ref cycle, x, ref totalSum, check);

            if (opcode == "addx")
            {
                Cycle1(ref cycle, x, ref totalSum, check);

                x += value;
            }
        }

        return totalSum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var cycle = 0;
        var r = 1;

        var crt = new char[40 * 6];

        await foreach (var (opcode, value) in _inputReader.ParseLinesAsync(10, ParseLine))
        {
            Cycle2(ref cycle, r, crt);

            if (opcode == "addx")
            {
                Cycle2(ref cycle, r, crt);

                r += value;
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine();
        for (var y = 0; y < 6; y++)
        {
            for (var x = 0; x < 40; x++)
                sb.Append(crt[y * 40 + x]);

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static void Cycle1(ref int cycle, int x, ref int totalSum, HashSet<int> check)
    {
        cycle++;

        if (check.Contains(cycle))
            totalSum += x * cycle;
    }

    private static void Cycle2(ref int cycle, int r, char[] crt)
    {
        cycle++;

        var pos = (cycle - 1) % 40;

        if (pos >= r - 1 && pos <= r + 1)
            crt[cycle - 1] = '#';
        else
            crt[cycle - 1] = '.';
    }

    private (string, int) ParseLine(string line)
    {
        var split = line.Split(' ');
        return (split[0], split.Length > 1 ? int.Parse(split[1]) : 0);
    }
}