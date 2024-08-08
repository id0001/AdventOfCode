using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var cloud = CreateMap(await inputReader.ReadGridAsync(22));
        var current = new Pose2(Point2.Zero, Face.Up);

        int infected = 0;
        for (var i = 0; i < 10000; i++)
        {
            current = cloud[current.Position] == 2 ? current.TurnRight() : current.TurnLeft();
            if (cloud[current.Position] == 0)
            {
                infected++;
                cloud[current.Position] = 2;
            }
            else
                cloud[current.Position] = 0;

            current = current.Step();
        }

        return infected.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var cloud = CreateMap(await inputReader.ReadGridAsync(22));
        var current = new Pose2(Point2.Zero, Face.Up);

        int infected = 0;
        for (var i = 0; i < 10000000; i++)
        {
            current = cloud[current.Position] switch
            {
                0 => current.TurnLeft(),
                1 => current,
                2 => current.TurnRight(),
                3 => current.TurnRight().TurnRight(),
                _ => throw new NotImplementedException()
            };

            cloud[current.Position] = (cloud[current.Position] + 1).Mod(4);
            if (cloud[current.Position] == 2)
                infected++;

            current = current.Step();
        }

        return infected.ToString();
    }

    private SparseSpatialMap<Point2, int, int> CreateMap(char[,] map)
    {
        var w = map.GetLength(1);
        var h = map.GetLength(0);

        var cloud = new SparseSpatialMap<Point2, int, int>();
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                if (map[y, x] == '#')
                    cloud.Set(new Point2(x - (w / 2), y - (h / 2)), 2);
            }
        }

        return cloud;
    }
}