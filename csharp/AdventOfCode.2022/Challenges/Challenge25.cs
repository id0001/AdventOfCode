using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2022.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        return Encode(await inputReader.ParseLinesAsync(25, Decode).SumAsync());
    }

    private static long Decode(string line)
    {
        long number = 0;
        for (var i = 0; i < line.Length; i++)
        {
            var col = (long) Math.Pow(5d, line.Length - 1 - i);

            number += line[i] switch
            {
                '2' => col * 2L,
                '1' => col,
                '0' => 0L,
                '-' => -col,
                '=' => col * -2L,
                _ => throw new NotImplementedException()
            };
        }

        return number;
    }

    private static string Encode(long number)
    {
        var sb = new StringBuilder();
        if (number == 0)
            return "0";

        while (number != 0)
        {
            var remainder = number % 5;
            number /= 5;
            if (remainder > 2)
            {
                number++;
                remainder -= 5;
            }

            sb.Insert(0, remainder switch
            {
                -2 => '=',
                -1 => '-',
                _ => remainder.ToString()[0]
            });
        }

        return sb.ToString();
    }
}