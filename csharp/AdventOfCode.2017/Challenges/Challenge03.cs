using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017.Challenges;

[Challenge(3)]
public class Challenge03
{
    private const int Input = 265149;
    private static readonly Point2[] Directions = [Face.Right, Face.Up, Face.Left, Face.Down];


    [Part1]
    public string Part1()
    {
        var curr = Point2.Zero;
        var di = 0;
        var totalSteps = 1;
        var stepChange = 2;
        var tsi = 0;
        var sci = 0;

        for (var i = 1; i < Input; i++)
        {
            curr += Directions[di];
            tsi++;

            if (tsi == totalSteps)
            {
                di = (di + 1).Mod(4);
                tsi = 0;
                sci++;
            }

            if (sci == stepChange)
            {
                totalSteps++;
                sci = 0;
            }
        }

        return Point2.ManhattanDistance(Point2.Zero, curr).ToString();
    }

    [Part2]
    public string Part2()
    {
        var cloud = new SparseSpatialMap<Point2, int, int>();
        var curr = Point2.Zero;
        cloud[curr] = 1;
        var di = 0;
        var totalSteps = 1;
        var stepChange = 2;
        var tsi = 0;
        var sci = 0;

        while (true)
        {
            curr += Directions[di];
            cloud[curr] = curr.GetNeighbors(true).Select(n => cloud[n]).Sum();
            if (cloud[curr] > Input)
                return cloud[curr].ToString();

            tsi++;

            if (tsi == totalSteps)
            {
                di = (di + 1).Mod(4);
                tsi = 0;
                sci++;
            }

            if (sci == stepChange)
            {
                totalSteps++;
                sci = 0;
            }
        }
    }
}