using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2016.Challenges;

[Challenge(16)]
public class Challenge16
{
    private const string Input = "11100010111110100";

    [Part1]
    public string Part1()
    {
        var input = Input;
        var length = 272;
        while (input.Length < length)
            input = CreateDragonCurve(input);

        return CreateChecksum(input[..length]);
    }

    [Part2]
    public string Part2()
    {
        var input = Input;
        var length = 35651584;
        while (input.Length < length)
            input = CreateDragonCurve(input);

        return CreateChecksum(input[..length]);
    }

    private string CreateDragonCurve(string input)
    {
        var sb = new StringBuilder();
        for (var i = input.Length - 1; i >= 0; i--)
            sb.Append(input[i] == '0' ? '1' : '0');

        return input + "0" + sb;
    }

    private string CreateChecksum(string input)
    {
        var sb = new StringBuilder();

        do
        {
            foreach (var pair in input.Chunk(2)) sb.Append(pair[0] == pair[1] ? '1' : '0');

            input = sb.ToString();
            sb.Clear();
        } while (input.Length % 2 == 0);

        return input;
    }
}