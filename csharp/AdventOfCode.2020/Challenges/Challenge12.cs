using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2020.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var x = 0;
        var y = 0;
        var a = 0; // east

        await foreach (var line in InputReader.ReadLinesAsync(12))
        {
            var action = line[0];
            var value = int.Parse(line[1..]);

            switch (action)
            {
                case 'N':
                    y -= value;
                    break;
                case 'E':
                    x += value;
                    break;
                case 'S':
                    y += value;
                    break;
                case 'W':
                    x -= value;
                    break;
                case 'L':
                    a = Euclid.Modulus(a - value, 360);
                    break;
                case 'R':
                    a = Euclid.Modulus(a + value, 360);
                    break;
                case 'F':
                    var r = Trigonometry.DegreeToRadian(a);
                    x += (int) Math.Cos(r) * value;
                    y += (int) Math.Sin(r) * value;
                    break;
            }
        }

        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var s = new Point2(0, 0);
        var w = new Point2(10, -1);

        await foreach (var line in InputReader.ReadLinesAsync(12))
        {
            var action = line[0];
            var value = int.Parse(line[1..]);

            switch (action)
            {
                case 'N':
                    w = w with {Y = w.Y - value};
                    break;
                case 'E':
                    w = w with {X = w.X + value};
                    break;
                case 'S':
                    w = w with {Y = w.Y + value};
                    break;
                case 'W':
                    w = w with {X = w.X - value};
                    break;
                case 'L':
                    w = Point2.Turn(w, s, -Trigonometry.DegreeToRadian(value));
                    break;
                case 'R':
                    w = Point2.Turn(w, s, Trigonometry.DegreeToRadian(value));
                    break;
                case 'F':
                    var r = Math.Atan2(w.Y - s.Y, w.X - s.X);
                    var dist = Point2.Distance(s, w);

                    var d = new Point2((int) Math.Round(Math.Cos(r) * dist), (int) Math.Round(Math.Sin(r) * dist));
                    s += d * value;
                    w += d * value;
                    break;
            }
        }

        return (Math.Abs(s.X) + Math.Abs(s.Y)).ToString();
    }
}