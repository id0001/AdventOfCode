using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(2)]
    public class Challenge02
    {
        private readonly IInputReader inputReader;
        private Movement[] movements;

        public Challenge02(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            movements = await inputReader.ReadLinesAsync(2)
                .Select(l =>
                {
                    string[] s = l.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    return new Movement(s[0], int.Parse(s[1]));
                }).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int x = 0;
            int y = 0;

            foreach(var movement in movements)
            {
                switch (movement.Direction)
                {
                    case "forward":
                        x += movement.Amount;
                        break;
                    case "down":
                        y += movement.Amount;
                        break;
                    case "up":
                        y -= movement.Amount;
                        break;
                }
            }

            return (x*y).ToString();
        }

        [Part2]
        public string Part2()
        {
            int x = 0;
            int y = 0;
            int aim = 0;

            foreach (var movement in movements)
            {
                switch (movement.Direction)
                {
                    case "forward":
                        x += movement.Amount;
                        y += movement.Amount * aim;
                        break;
                    case "down":
                        aim += movement.Amount;
                        break;
                    case "up":
                        aim -= movement.Amount;
                        break;
                }
            }

            return (x * y).ToString();
        }

        private record Movement(string Direction, int Amount);
    }
}
