using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using System.Diagnostics;

namespace AdventOfCode2017.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var list = (await inputReader.ReadLinesAsync(20).ToListAsync()).Select(ParseLine).ToList();

        return list.Select(x => Execute(x, 1000)).MinBy(x => Point3.ManhattanDistance(Point3.Zero, x.Position))!.Id.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var list = (await inputReader.ReadLinesAsync(20).ToListAsync()).Select(ParseLine).ToList();

        var c = list[0];
        for(var i = 0; i < 10; i++)
        {
            Console.WriteLine(c);
            c = Execute(c, 1);
        }


        var ticks = 100; // arbitrary amount - got lucky and worked first try

        for (var t = 0; t < ticks; t++)
        {
            list = list.Select(pva => Execute(pva, 1)).ToList();
            list = RemoveCollisions(list).ToList();
            Debug.WriteLine($"{t}: {list.Count}");
        }

        return list.Count.ToString();
    }

    private static IEnumerable<PVA> RemoveCollisions(List<PVA> source)
    {
        while (source.Any())
        {
            var particle = source.First();
            var collisions = source.Where(p => p.Position == particle.Position).ToList();

            if (collisions.Count() == 1)
            {
                yield return particle;
            }

            collisions.ForEach(c => source.Remove(c));
        }
    }

    private static PVA Execute(PVA item, int times)
    {
        for (var i = 0; i < times; i++)
        {
            var v = item.Velocity + item.Acceleration;
            var p = item.Position + item.Velocity;
            item = item with { Position = p, Velocity = v };
        }

        return item;
    }

    private static PVA ParseLine(string line, int index) => line
        .Extract(@"p=<(.+)>, v=<(.+)>, a=<(.+)>")
        .Select(x => x.SplitBy(",").As<int>().Into(parts => new Point3(parts.First(), parts.Second(), parts.Third())))
        .Into(parts => new PVA(index, parts.First(), parts.Second(), parts.Third()));

    private record PVA(int Id, Point3 Position, Point3 Velocity, Point3 Acceleration);
}