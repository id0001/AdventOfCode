using AdventOfCode.Core;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2015.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part1]
    public string Part1()
    {
        const int r = 2978;
        const int c = 3083;

        var n = 20151125L;

        var ix = GetIndex(r - 1, c - 1);
        for (var i = 0; i < ix; i++) n = Euclid.Modulus(n * 252533, 33554393);

        return n.ToString();
    }

    private static int GetIndex(int r, int c) => Euclid.TriangularNumber(r + c) + c;
}