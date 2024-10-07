using AdventOfCode.Core;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(11)]
public class Challenge11
{
    private const int SerialNumber = 4151;

    [Part1]
    public string Part1()
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels());
        var largest = Enumerable.Range(0, 297).SelectMany(y => Enumerable.Range(0, 297).Select(x => new Point2(x, y))).MaxBy(p => sat.SumQuery(p.X, p.Y, 3, 3));
        return $"{largest.X},{largest.Y}";
    }

    [Part2]
    public string Part2()
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels());
        var largest = Enumerable.Range(1, 301)
            .SelectMany(size => Enumerable.Range(0, 301 - size).SelectMany(y => Enumerable.Range(0, 301 - size).Select(x => new Point3(x, y, size))))
            .MaxBy(key => sat.SumQuery(key.X, key.Y, key.Z, key.Z));

        return $"{largest.X},{largest.Y},{largest.Z}";
    }

    private static int GetPowerlevel(int x, int y) => (((GetRackId(x) * y) + SerialNumber) * GetRackId(x)).ExtractDigit(2) - 5;

    private static int GetRackId(int x) => x + 10;

    private static int[,] PrecalculatePowerlevels()
    {
        var table = new int[300, 300];
        for (var y = 0; y < 300; y++)
        {
            for (var x = 0; x < 300; x++)
            {
                table[y, x] = GetPowerlevel(x, y);
            }
        }

        return table;
    }
}