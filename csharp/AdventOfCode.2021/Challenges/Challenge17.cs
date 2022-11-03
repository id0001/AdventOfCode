using AdventOfCode.Core;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2021.Challenges;

[Challenge(17)]
public class Challenge17
{
    private const int TargetXMin = 192;
    private const int TargetXMax = 251;
    private const int TargetYMin = -59;
    private const int TargetYMax = -89;

    [Part1]
    public string Part1()
    {
        return Euclid.TriangularNumber(-TargetYMax - 1).ToString();
    }

    [Part2]
    public string Part2()
    {
        var count = 0;
        for (var y = TargetYMax; y <= -TargetYMax - 1; y++)
        for (var x = Euclid.InverseTriangleNumber(TargetXMin); x <= TargetXMax; x++)
        {
            if (Simulate(x, y, TargetXMin, TargetXMax, TargetYMin, TargetYMax))
                count++;
        }

        return count.ToString();
    }

    private static bool Simulate(int vx, int vy, int xlow, int xhigh, int ylow, int yhigh)
    {
        var x = 0;
        var y = 0;
        while (x < xhigh && y > yhigh)
        {
            x += vx;
            y += vy;
            vx -= Math.Sign(vx);
            vy -= 1;

            if (x >= xlow && x <= xhigh && y <= ylow && y >= yhigh)
                return true;
        }

        return false;
    }
}