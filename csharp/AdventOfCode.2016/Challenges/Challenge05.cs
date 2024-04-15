using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2016.Challenges;

[Challenge(5)]
public class Challenge05
{
    private const string Input = "abbhdwsy";

    [Part1]
    public string Part1()
    {
        using var md5 = MD5.Create();

        StringBuilder pass = new();
        var index = 0;
        while (pass.Length < 8)
        {
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(Input + index));
            if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
                pass.Append((hash[2] & 0x0F).ToString("X"));

            index++;
        }

        return pass.ToString().ToLowerInvariant();
    }

    [Part2]
    public string Part2()
    {
        using var md5 = MD5.Create();

        var pass = new char[8];
        var charsFound = 0;
        var index = 0;
        while (charsFound < 8)
        {
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(Input + index));
            if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
                if (int.TryParse((hash[2] & 0x0F).ToString("X"), out var i) && i <= 7 && pass[i] == default)
                {
                    pass[i] = (hash[3] & 0xF0).ToString("X")[0];
                    charsFound++;
                }

            index++;
        }

        return string.Join(string.Empty, pass).ToLowerInvariant();
    }
}