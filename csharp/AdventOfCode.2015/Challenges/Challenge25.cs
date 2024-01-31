using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2015.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var r = 2978;
        var c = 3083;

        var n = 20151125L;

        var ix = GetIndex(r-1,c-1);
        for (var i = 0; i < ix; i++)
        {
            n = Euclid.Modulus(n * 252533, 33554393);
        }

        return n.ToString();
    }

    private int GetIndex(int r, int c) => Euclid.TriangularNumber(r + c) + c;
}
