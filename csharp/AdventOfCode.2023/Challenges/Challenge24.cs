using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using Microsoft.Z3;
using System.Drawing;
using System.Net.Mime;
using System.Runtime.InteropServices;

namespace AdventOfCode2023.Challenges;

[Challenge(24)]
public class Challenge24
{
    private readonly IInputReader _inputReader;

    public Challenge24(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var min = 200000000000000L;
        var max =  400000000000000L;

        var hailstones = await _inputReader.ParseLinesAsync(24, ParseLine).ToArrayAsync();

        var c = 0;
        for (var i = 0; i < hailstones.Length - 1; i++)
        {
            for (var j = i + 1; j < hailstones.Length; j++)
            {
                if (TryIntersects(hailstones[i], hailstones[j], out var ix, out var iy))
                {
                    var h1 = hailstones[i];
                    var h2 = hailstones[j];

                    if ((ix < h1.Position.X && h1.Velocity.X > 0) || (ix > h1.Position.X && h1.Velocity.X < 0))
                        continue;

                    if ((ix < h2.Position.X && h2.Velocity.X > 0) || (ix > h2.Position.X && h2.Velocity.X < 0))
                        continue;
                    
                    if (ix >= min && iy >= min && ix <= max && iy <= max)
                        c++;
                }
            }
        }


        return c.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var hailstones = await _inputReader.ParseLinesAsync(24, ParseLine).ToListAsync();

        return Solve(hailstones).ToString();
    }

    private static bool TryIntersects(Hailstone p1, Hailstone p2, out double ix, out double iy)
    {
        ix = 0L;
        iy = 0L;

        var (x1a, y1a, _) = p1.Position;
        var (x2a, y2a, _) = p2.Position;
        var (x1, y1, _) = p1.Velocity;
        var (x2, y2, _) = p2.Velocity;

        if (x1 * y2 - x2 * y1 == 0)
            return false;

        var t = ((x2a - x1a) * y2 - (y2a - y1a) * x2) / (double)(x1 * y2 - x2 * y1);
        ix = x1a + t * x1;
        iy = y1a + t * y1;

        return true;
    }

    private static long Solve(List<Hailstone> hailstones)
    {
        var context = new Context();
        var solver = context.MkSolver();

        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var z = context.MkIntConst("z");

        var vx = context.MkIntConst("vx");
        var vy = context.MkIntConst("vy");
        var vz = context.MkIntConst("vz");

        for (var i = 0; i < 3; i++)
        {
            var t = context.MkIntConst($"t{i}");
            var hailstone = hailstones[i];

            var px = context.MkInt(hailstone.Position.X);
            var py = context.MkInt(hailstone.Position.Y);
            var pz = context.MkInt(hailstone.Position.Z);

            var pvx = context.MkInt(hailstone.Velocity.X);
            var pvy = context.MkInt(hailstone.Velocity.Y);
            var pvz = context.MkInt(hailstone.Velocity.Z);

            var xl = context.MkAdd(x, context.MkMul(t, vx));
            var yl = context.MkAdd(y, context.MkMul(t, vy));
            var zl = context.MkAdd(z, context.MkMul(t, vz));

            var xr = context.MkAdd(px, context.MkMul(t, pvx));
            var yr = context.MkAdd(py, context.MkMul(t, pvy));
            var zr = context.MkAdd(pz, context.MkMul(t, pvz));

            solver.Add(t >= 0);
            solver.Add(context.MkEq(xl, xr));
            solver.Add(context.MkEq(yl, yr));
            solver.Add(context.MkEq(zl, zr));
        }

        solver.Check();
        var model = solver.Model;

        var rx = model.Eval(x);
        var ry = model.Eval(y);
        var rz = model.Eval(z);

        return rx.ToString().As<long>() + ry.ToString().As<long>() + rz.ToString().As<long>();
    }


    private static Hailstone ParseLine(string line)
    {
        return line.SplitBy("@")
            .Transform(parts =>
            {
                return new Hailstone(
                    parts.First().SplitBy(",").As<long>().Transform(p => new Point3L(p[0], p[1], p[2])),
                    parts.Second().SplitBy(",").As<long>().Transform(p => new Point3L(p[0], p[1], p[2]))
                    );
            });
    }

    private record Point3L(long X, long Y, long Z);

    private record Hailstone(Point3L Position, Point3L Velocity);
}
