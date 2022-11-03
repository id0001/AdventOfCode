using AdventOfCode.Lib;
using AdventOfCode2019.IntCode.Core;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(17)]
public class Challenge17
{
    private readonly IInputReader _inputReader;

    public Challenge17(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(17, ',').ToArrayAsync();

        var map = new Dictionary<Point2, int>();

        var x = 0;
        var y = 0;

        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o =>
        {
            if (o == 10)
            {
                y++;
                x = 0;
                return;
            }

            var p = new Point2(x, y);
            map.Add(p, (int)o);
            x++;
        });

        await cpu.StartAsync();

        var sum = 0;
        foreach (var kv in map.Where(kv => kv.Value == 35))
        {
            if (!IsIntersection(map, kv.Key)) continue;
            map[kv.Key] = 79;
            sum += kv.Key.X * kv.Key.Y;
        }

        PrintMap(map);

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(17, ',').ToArrayAsync();

        program[0] = 2;

        const string a = "R,12,R,4,R,10,R,12";
        const string b = "R,6,L,8,R,10";
        const string c = "L,8,R,4,R,4,R,6";
        const string main = "A,B,A,C,A,B,C,A,B,C";

        long dustCount = 0;
        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o => { dustCount = o; });

        var settings = new[]
        {
            Encoding.ASCII.GetBytes(main), Encoding.ASCII.GetBytes(a), Encoding.ASCII.GetBytes(b),
            Encoding.ASCII.GetBytes(c), new byte[] { 110 }
        };
        var i = 0;
        var j = 0;
        cpu.RegisterInput(() =>
        {
            if (i >= settings[j].Length)
            {
                cpu.WriteInput(10);
                j++;
                i = 0;
                return;
            }

            cpu.WriteInput(settings[j][i]);
            i++;
        });

        await cpu.StartAsync();

        return dustCount.ToString();
    }

    private static bool IsIntersection(IReadOnlyDictionary<Point2, int> map, Point2 p)
    {
        var neighborDeltas = new[] { new Point2(0, -1), new Point2(1, 0), new Point2(0, 1), new Point2(-1, 0) };
        for (var i = 0; i < 4; i++)
        {
            var np = p + neighborDeltas[i];
            if (!map.ContainsKey(np) || map[np] != 35)
                return false;
        }

        return true;
    }

    private static void PrintMap(Dictionary<Point2, int> map)
    {
        var xlen = map.Keys.Max(p => p.X + 1);
        var ylen = map.Keys.Max(p => p.Y + 1);

        for (var y = 0; y < ylen; y++)
        {
            for (var x = 0; x < xlen; x++) Console.Write((char)map[new Point2(x, y)]);

            Console.WriteLine();
        }
    }
}