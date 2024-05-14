using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2016.Challenges;

[Challenge(21)]
public class Challenge21(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var password = "abcdefgh";
        await foreach (var ops in inputReader.ParseLinesAsync(21, l => ParseLine(l)))
            password = ops(password);
        return password;
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var password = "fbgdceah";
        await foreach (var ops in inputReader.ParseLinesAsync(21, l => ParseLine(l, true)).Reverse())
            password = ops(password);
        return password;
    }

    private Func<string, string> ParseLine(string line, bool isPart2 = false)
    {
        if (line.TryExtract<int>(@"swap position (\d)+ with position (\d)+", out var m1))
            return s => SwapPositions(s, m1[0], m1[1]);

        if (line.TryExtract<char>(@"swap letter (\w) with letter (\w)", out var m2))
            return s => SwapLetters(s, m2[0], m2[1]);

        if (line.TryExtract(@"rotate (right|left) (\d+) steps?", out var m3))
            return s => Rotate(s,
                m3[0] == "left" ? (isPart2 ? -1 : 1) * m3[1].As<int>() : (isPart2 ? 1 : -1) * m3[1].As<int>());

        if (line.TryExtract<char>(@"rotate based on position of letter (\w)", out var m4))
            return isPart2 ? s => RotateFromLetterReversed(s, m4[0]) : s => RotateFromLetter(s, m4[0]);

        if (line.TryExtract<int>(@"reverse positions (\d+) through (\d+)", out var m5))
            return s => Reverse(s, m5[0], m5[1]);

        if (line.TryExtract<int>(@"move position (\d+) to position (\d+)", out var m6))
            return isPart2 ? s => Move(s, m6[1], m6[0]) : s => Move(s, m6[0], m6[1]);

        throw new NotImplementedException();
    }

    private static string SwapPositions(string password, int a, int b) => OrderInput(a, b,
        (x, y) => password[..x] + password[y] + password[(x + 1)..y] + password[x] + password[(y + 1)..]);

    private static string SwapLetters(string password, char x, char y) => password
        .Aggregate(new StringBuilder(), (result, c) => result.Append(c == x ? y : c == y ? x : c)).ToString();

    private static string Rotate(string password, int x) => password[Euclid.Modulus(x, password.Length)..] +
                                                            password[..Euclid.Modulus(x, password.Length)];

    private static string RotateFromLetter(string password, char x)
    {
        var index = password.IndexOf(x);
        return Rotate(password, -(index + (index < 4 ? 1 : 2)));
    }

    private static string RotateFromLetterReversed(string password, char x)
    {
        var index = password.IndexOf(x);
        return Rotate(password,
            index - Enumerable.Range(0, 8)
                .First(i => Euclid.Modulus(2 * i + (i < 4 ? 1 : 2), password.Length) == index));
    }

    private static string Reverse(string password, int a, int b) => OrderInput(a, b,
        (x, y) => password[..x] +
                  password[x..(y + 1)].Aggregate(new StringBuilder(), (result, c) => result.Insert(0, c)) +
                  password[(y + 1)..]);

    private static string Move(string password, int x, int y) => x < y
        ? password[..x] + password[(x + 1)..(y + 1)] + password[x] + password[(y + 1)..]
        : password[..y] + password[x] + password[y..x] + password[(x + 1)..];

    private static string OrderInput(int x, int y, Func<int, int, string> rest) => x < y ? rest(x, y) : rest(y, x);
}