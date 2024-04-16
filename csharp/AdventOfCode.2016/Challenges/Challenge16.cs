using AdventOfCode.Core;
using AdventOfCode.Lib;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(16)]
public class Challenge16()
{
    private const string Input = "11100010111110100";

    [Part1]
    public async Task<string> Part1Async()
    {
        string input = Input;
        int length = 272;
        while (input.Length < length)
            input = CreateDragonCurve(input);

        return CreateChecksum(input[0..length]);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        string input = Input;
        int length = 35651584;
        while (input.Length < length)
            input = CreateDragonCurve(input);

        return CreateChecksum(input[0..length]);
    }

    private string CreateDragonCurve(string input)
    {
        var sb = new StringBuilder();
        for (var i = input.Length - 1; i >= 0; i--)
            sb.Append(input[i] == '0' ? '1' : '0');

        return input + "0" + sb.ToString();
    }

    private string CreateChecksum(string input)
    {
        var sb = new StringBuilder();

        do
        {
            foreach (var pair in input.Chunk(2))
            {
                sb.Append(pair[0] == pair[1] ? '1' : '0');
            }

            input = sb.ToString();
            sb.Clear();
        }
        while (input.Length % 2 == 0);

        return input;
    }
}
