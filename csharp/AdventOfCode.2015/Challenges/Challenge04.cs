using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2015.Challenges;

[Challenge(4)]
public class Challenge04
{
    private const string Input = "ckczppom";

    [Part1]
    public string Part1() => FindHashStartingWith("00000").ToString();

    [Part2]
    public string Part2() => FindHashStartingWith("000000").ToString();

    private static int FindHashStartingWith(string startsWith)
    {
        for(var i = 0; ; i++)
        {
            using var md5 = MD5.Create();
            var input = Encoding.ASCII.GetBytes($"{Input}{i}");
            var output = md5.ComputeHash(input);
            var hex = BitConverter.ToString(output).Replace("-", string.Empty);
            if (hex.StartsWith(startsWith))
                return i;
        }
    }
}