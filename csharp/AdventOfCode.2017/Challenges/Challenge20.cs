using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var list = (await inputReader.ReadLinesAsync(20).ToListAsync()).Select(ParseLine).ToList();
        return list.Select(x => ExecuteXTimes(x, 1000)).MinBy(x => Point3.ManhattanDistance(Point3.Zero, x.Position))!
            .Id.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var list = (await inputReader.ReadLinesAsync(20).ToListAsync()).Select(ParseLine).ToList();

        for (var i = 0; i < 1000; i++)
            list = list.Select(Execute)
                .GroupBy(p => p.Position)
                .Where(g => g.Count() == 1)
                .SelectMany(g => g)
                .ToList();

        return list.Count.ToString();
    }

    private static PosVelAcc ExecuteXTimes(PosVelAcc item, int times)
    {
        for (var i = 0; i < times; i++)
            item = Execute(item);

        return item;
    }

    private static PosVelAcc Execute(PosVelAcc item)
    {
        var v = item.Velocity + item.Acceleration;
        var p = item.Position + v;
        return item with {Position = p, Velocity = v};
    }

    private static PosVelAcc ParseLine(string line, int index) => line
        .Extract(@"p=<(.+)>, v=<(.+)>, a=<(.+)>")
        .Select(x => x.SplitBy(",").As<int>().Into(parts => new Point3(parts.First(), parts.Second(), parts.Third())))
        .ToArray()
        .Into(parts => new PosVelAcc(index, parts.First(), parts.Second(), parts.Third()));

    private record PosVelAcc(int Id, Point3 Position, Point3 Velocity, Point3 Acceleration);
}