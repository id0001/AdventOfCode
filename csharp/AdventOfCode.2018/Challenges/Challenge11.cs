using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Factories;

namespace AdventOfCode2018.Challenges;

[Challenge(11)]
public class Challenge11
{
    private const int SerialNumber = 4151;

    [Part1]
    public string Part1()
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels());
        var largest = Array2d.Range(297, 297).MaxBy(p => sat.SumQuery(p.X, p.Y, 3, 3));
        return $"{largest.X},{largest.Y}";
    }

    [Part2]
    public string Part2()
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels());
        var largest = Enumerable.Range(1, 301).SelectMany(size => Array2d.Range(301 - size, 301 - size).Select(p => new Point3(p.X, p.Y, size))).MaxBy(p => sat.SumQuery(p.X, p.Y, p.Z, p.Z));

        return $"{largest.X},{largest.Y},{largest.Z}";
    }

    private static int GetPowerlevel(int x, int y) => (((GetRackId(x) * y) + SerialNumber) * GetRackId(x)).ExtractDigit(2) - 5;

    private static int GetRackId(int x) => x + 10;

    private static int[,] PrecalculatePowerlevels() => Array2d.Create(300, 300, p => GetPowerlevel(p.X, p.Y));
}