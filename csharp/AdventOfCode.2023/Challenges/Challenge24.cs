using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using Microsoft.Z3;

namespace AdventOfCode2023.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var min = 200000000000000L;
        var max = 400000000000000L;

        var hailstones = await InputReader.ParseLinesAsync(24, ParseLine).ToArrayAsync();

        var c = 0;
        for (var i = 0; i < hailstones.Length - 1; i++)
        for (var j = i + 1; j < hailstones.Length; j++)
            if (Line2.TryIntersect(hailstones[i].ToLine2(), hailstones[j].ToLine2(), out var intersection))
            {
                var h1 = hailstones[i];
                var h2 = hailstones[j];

                if ((intersection.X < h1.Intercept.X && h1.Slope.X > 0) || (intersection.X > h1.Intercept.X && h1.Slope.X < 0))
                    continue;

                if ((intersection.X < h2.Intercept.X && h2.Slope.X > 0) || (intersection.X > h2.Intercept.X && h2.Slope.X < 0))
                    continue;

                if (intersection.X >= min && intersection.Y >= min && intersection.X <= max && intersection.Y <= max)
                    c++;
            }


        return c.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var hailstones = await InputReader.ParseLinesAsync(24, ParseLine).ToListAsync();

        return Solve(hailstones).ToString();
    }

    private static long Solve(List<Line3> hailstones)
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

            var px = context.MkInt((long)hailstone.Intercept.X);
            var py = context.MkInt((long)hailstone.Intercept.Y);
            var pz = context.MkInt((long)hailstone.Intercept.Z);

            var pvx = context.MkInt((long)hailstone.Slope.X);
            var pvy = context.MkInt((long)hailstone.Slope.Y);
            var pvz = context.MkInt((long)hailstone.Slope.Z);

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


    private static Line3 ParseLine(string line)
    {
        return line.SplitBy("@")
            .Into(parts =>
            {
                return new Line3(
                    parts.First().SplitBy(",").As<long>().Into(p => new Vector3(p[0], p[1], p[2])),
                    parts.Second().SplitBy(",").As<long>().Into(p => new Vector3(p[0], p[1], p[2]))
                );
            });
    }
}