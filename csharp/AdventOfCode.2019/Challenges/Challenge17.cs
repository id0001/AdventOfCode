using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(17)]
    public class Challenge17
    {
        private readonly IInputReader inputReader;
        private long[] program;

        public Challenge17(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            program = await inputReader.ReadLineAsync<long>(17, ',').ToArrayAsync();
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var map = new Dictionary<Point2, int>();

            int x = 0;
            int y = 0;

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

            int sum = 0;
            foreach (var kv in map.Where(x => x.Value == 35))
            {
                if (IsIntersection(map, kv.Key))
                {
                    map[kv.Key] = 79;
                    sum += (kv.Key.X) * (kv.Key.Y);
                }
            }

            PrintMap(map);

            return sum.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            program[0] = 2;

            string A = "R,12,R,4,R,10,R,12";
            string B = "R,6,L,8,R,10";
            string C = "L,8,R,4,R,4,R,6";
            string main = "A,B,A,C,A,B,C,A,B,C";

            var map = new Dictionary<Point2, int>();

            long dustCount = 0;
            var cpu = new Cpu();
            cpu.SetProgram(program);
            cpu.RegisterOutput(o =>
            {
                dustCount = o;
            });

            byte[][] settings = new[] { Encoding.ASCII.GetBytes(main), Encoding.ASCII.GetBytes(A), Encoding.ASCII.GetBytes(B), Encoding.ASCII.GetBytes(C),  new byte[] { 110 } };
            int i = 0;
            int j = 0;
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

        private bool IsIntersection(Dictionary<Point2, int> map, Point2 p)
        {
            var neighborDeltas = new[] { new Point2(0, -1), new Point2(1, 0), new Point2(0, 1), new Point2(-1, 0) };
            for (int i = 0; i < 4; i++)
            {
                var np = p + neighborDeltas[i];
                if (!map.ContainsKey(np) || map[np] != 35)
                    return false;
            }

            return true;
        }

        private void PrintMap(Dictionary<Point2, int> map)
        {
            int xlen = map.Keys.Max(p => p.X + 1);
            int ylen = map.Keys.Max(p => p.Y + 1);

            for (int y = 0; y < ylen; y++)
            {
                for (int x = 0; x < xlen; x++)
                {
                    Console.Write((char)map[new Point2(x, y)]);
                }

                Console.WriteLine();
            }
        }
    }
}
