using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2016.Challenges;

[Challenge(14)]
public class Challenge14
{
    private const string Input = "cuanljph";

    [Part1]
    public string Part1()
    {
        using var md5 = MD5.Create();

        var buffer = new CircularBuffer<string>(1001);

        var index = 0;
        var keysFound = 0;
        while (true)
        {
            buffer.Push(GetHash(md5, index, 0));

            if (index >= 1001)
                if (ContainsTriplet(buffer[0]!, out var symbol) &&
                    buffer.Skip(1).Any(hash => ContainsQuintuplet(hash!, symbol)))
                {
                    keysFound++;
                    if (keysFound == 64)
                        return (index - 1000).ToString();
                }

            index++;
        }
    }

    [Part2]
    public string Part2()
    {
        using var md5 = MD5.Create();

        var buffer = new CircularBuffer<string>(1001);

        var index = 0;
        var keysFound = 0;
        while (true)
        {
            buffer.Push(GetHash(md5, index, 2016));

            if (index >= 1001)
                if (ContainsTriplet(buffer[0]!, out var symbol) &&
                    buffer.Skip(1).Any(hash => ContainsQuintuplet(hash!, symbol)))
                {
                    keysFound++;
                    if (keysFound == 64)
                        return (index - 1000).ToString();
                }

            index++;
        }
    }

    private bool ContainsTriplet(string input, out char symbol)
    {
        symbol = '!';

        foreach (var window in input.Windowed(3))
        {
            var s = window.First();
            if (window.All(c => c == s))
            {
                symbol = s;
                return true;
            }
        }

        return false;
    }

    private bool ContainsQuintuplet(string input, char symbol)
    {
        foreach (var window in input.Windowed(5))
            if (window.All(c => c == symbol))
                return true;

        return false;
    }

    private static string GetHash(MD5 md5, int index, int stretch)
    {
        var hash = Convert.ToHexString(md5.ComputeHash(Encoding.Default.GetBytes(Input + index))).ToLowerInvariant();

        for (var i = 0; i < stretch; i++)
            hash = Convert.ToHexString(md5.ComputeHash(Encoding.Default.GetBytes(hash))).ToLowerInvariant();

        return hash;
    }
}