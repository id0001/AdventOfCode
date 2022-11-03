using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2015.Challenges;

[Challenge(10)]
public class Challenge10
{
    private const string OriginalInput = "1113222113";

    [Part1]
    public string Part1()
    {
        var input = OriginalInput;
        for (var i = 0; i < 40; i++)
            input = Transform(input);

        return input.Length.ToString();
    }

    [Part2]
    public string Part2()
    {
        var input = OriginalInput;
        for (var i = 0; i < 50; i++)
            input = Transform(input);

        return input.Length.ToString();
    }

    private static string Transform(string input)
    {
        var stringBuilder = new StringBuilder();

        var count = 1;
        var type = input[0];
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i] == type)
            {
                count++;
                continue;
            }

            stringBuilder.Append(count);
            stringBuilder.Append(type);
            type = input[i];
            count = 1;
        }

        stringBuilder.Append(count);
        stringBuilder.Append(type);
        return stringBuilder.ToString();
    }
}