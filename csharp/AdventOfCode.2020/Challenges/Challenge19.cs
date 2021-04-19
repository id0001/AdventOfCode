using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(19)]
    public class Challenge19
    {
        private readonly IInputReader inputReader;
        private IDictionary<int, string> rules;
        private List<string> input;


        public Challenge19(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            int state = 0; // 0 = rules, 1 = input
            rules = new Dictionary<int, string>();
            input = new List<string>();

            await foreach (var line in inputReader.ReadLinesAsync(19))
            {
                if (string.IsNullOrEmpty(line))
                    state++;

                if (state == 0)
                {
                    var match = Regex.Match(line, @"^(\d+): (.+)$");
                    rules.Add(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
                }
                else
                {
                    input.Add(line);
                }
            }
        }

        [Part1]
        public string Part1()
        {
            string pattern = BuildPattern(rules, 0, new Dictionary<int, string>());

            int count = 0;
            foreach (var line in input)
            {
                if (Regex.IsMatch(line, pattern))
                    count++;
            }

            return count.ToString();
        }

        [Part2]
        public string Part2()
        {
            rules[8] = "42 | 42 8";
            rules[11] = "42 31 | 42 11 31";
            string pattern = BuildPattern(rules, 0, new Dictionary<int, string>());

            int count = 0;
            foreach (var line in input)
            {
                if (Regex.IsMatch(line, pattern))
                    count++;
            }

            return count.ToString();
        }

        private string BuildPattern(IDictionary<int, string> rules, int key, IDictionary<int, string> cache, bool print = false)
        {
            if (cache.ContainsKey(key))
                return cache[key];

            if (rules[key] == "\"a\"")
                return "a";

            if (rules[key] == "\"b\"")
                return "b";

            string[] orGroups = rules[key].Split("|");

            for (int o = 0; o < orGroups.Length; o++)
            {
                if (key == 8 && orGroups[o].Contains("8"))
                {
                    var p42 = BuildPattern(rules, 42, cache, print);
                    orGroups[o] = $"{p42}+";
                }
                else if (key == 11 && orGroups[o].Contains("11"))
                {
                    var p42 = BuildPattern(rules, 42, cache, print);
                    var p31 = BuildPattern(rules, 31, cache, print);

                    orGroups[o] = "(?:" + string.Join("|", Enumerable.Range(1, 10).Select(i => $"{p42}{{{i}}}{p31}{{{i}}}")) + ")";
                }
                else
                {
                    string[] parts = orGroups[o].Trim().Split(" ");
                    for (int p = 0; p < parts.Length; p++)
                    {
                        int rule = int.Parse(parts[p].Trim());
                        parts[p] = BuildPattern(rules, rule, cache, print);
                    }

                    orGroups[o] = string.Concat(parts);
                }
            }

            string result = key == 0 ? $"^{string.Join("|", orGroups)}$" : $"(?:{string.Join("|", orGroups)})";

            if (print)
                Console.WriteLine($"{key}: {result}");

            if (!cache.ContainsKey(key))
                cache.Add(key, result);

            return result;
        }
    }
}
