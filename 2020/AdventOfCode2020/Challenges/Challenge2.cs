using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(2)]
    public class Challenge2
    {
        private readonly IChallengeInput challengeInput;
        private InputLine[] input;

        public Challenge2(IChallengeInput challengeInput)
        {
            this.challengeInput = challengeInput;
        }

        [Setup]
        public async Task SetupAsync()
        {
            input = await challengeInput.ReadLinesAsync(2).Select(line =>
            {
                Match m = Regex.Match(line, @"^(\d+)-(\d+) (\w): (\w+)$");
                if (!m.Success)
                    throw new InvalidOperationException("Line did not match pattern: " + line);

                int min = int.Parse(m.Groups[1].Value);
                int max = int.Parse(m.Groups[2].Value);
                char c = char.Parse(m.Groups[3].Value);
                string pass = m.Groups[4].Value;

                return new InputLine(min, max, c, pass);
            }).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            string[] result = (from line in input
                             let count = Regex.Matches(line.Password, line.Character.ToString()).Count
                             where count >= line.Min && count <= line.Max
                             select line.Password).ToArray();

            return result.Length.ToString();
        }

        [Part2]
        public string Part2()
        {
            string[] result = (from line in input
                               where line.Password[line.Min-1] == line.Character ^ line.Password[line.Max-1] == line.Character
                               select line.Password).ToArray();

            return result.Length.ToString();
        }

        private record InputLine(int Min, int Max, char Character, string Password);
    }
}
