using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(18)]
    public class Challenge18
    {
        private const string NumberNumberPattern = @"^\[(\d),(\d)\]$";
        private const string NumberPairPattern = @"^\[(\d),(.+)\]$";
        private const string PairNumberPattern = @"^\[(.+),(\d)\]$";

        private readonly IInputReader inputReader;
        private string[] lines;

        public Challenge18(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            lines = await inputReader.ReadLinesAsync(18).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            string number = lines[0];
            for (int i = 1; i < lines.Length; i++)
            {
                number = Add(number, lines[i]);
                number = Reduce(number);
            }

            return GetMagnitude(number).ToString();
        }

        [Part2]
        public string Part2()
        {
            int maxMagnitude = 0;
            for (int y = 0; y < lines.Length; y++)
            {
                string line1 = lines[y];
                for (int x = 0; x < lines.Length; x++)
                {
                    string line2 = lines[x];

                    string num1 = Add(line1, line2);
                    int mag = GetMagnitude(Reduce(num1));
                    if (mag > maxMagnitude)
                        maxMagnitude = mag;

                    string num2 = Add(line2, line1);
                    mag = GetMagnitude(Reduce(num2));
                    if (mag > maxMagnitude)
                        maxMagnitude = mag;
                }
            }

            return maxMagnitude.ToString();
        }

        private string Add(string number1, string number2)
        {
            return $"[{number1},{number2}]";
        }

        private string Reduce(string line)
        {
            string result = line;
            while (true)
            {
                if (!TryExplode(result, out result) && !TrySplit(result, out result))
                    return result;
            }
        }

        private bool TryExplode(string line, out string result)
        {
            int depth = 0;
            int lastNumberIndex = -1;
            int lastNumberLength = 0;
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '[' when depth == 4:
                        Match m = Regex.Match(line.Substring(i), @"^\[(\d+),(\d+)\]");
                        int n1 = int.Parse(m.Groups[1].Value);
                        int n2 = int.Parse(m.Groups[2].Value);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(line.Substring(0, i));
                        if (lastNumberIndex > 0)
                        {
                            int num = int.Parse(line.Substring(lastNumberIndex, lastNumberLength));
                            sb.Remove(lastNumberIndex, lastNumberLength);
                            sb.Insert(lastNumberIndex, n1 + num);
                        }

                        sb.Append('0');
                        i += m.Length;

                        for (; i < line.Length && !char.IsDigit(line[i]); i++)
                            sb.Append(line[i]);

                        if (i != line.Length)
                        {
                            string num = GetNumber(line.Substring(i));
                            sb.Append(int.Parse(num) + n2);
                            i += num.Length;
                        }

                        sb.Append(line.Substring(i));
                        result = sb.ToString();
                        return true;
                    case '[':
                        depth++;
                        break;
                    case ']':
                        depth--;
                        break;
                    case char c when char.IsDigit(c):
                        lastNumberIndex = i;
                        lastNumberLength = Regex.Match(line.Substring(i), @"^(\d+)").Length;
                        i += lastNumberLength - 1;
                        break;
                    default:
                        break;
                }
            }

            result = line;
            return false;
        }

        private bool TrySplit(string line, out string result)
        {
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case char c when char.IsDigit(c):
                        string num = GetNumber(line.Substring(i));
                        if (num.Length >= 2)
                        {
                            int number = int.Parse(num);
                            int l = number / 2;
                            int r = number - l;
                            StringBuilder sb = new StringBuilder();
                            sb.Append(line.Substring(0, i));
                            sb.Append($"[{l},{r}]");
                            i += num.Length;
                            sb.Append(line.Substring(i));
                            result = sb.ToString();
                            return true;
                        }
                        break;
                }
            }

            result = line;
            return false;
        }

        private string GetNumber(string line)
        {
            var m = Regex.Match(line, @"^(\d+)");
            return m.Groups[1].Value;
        }

        private int GetMagnitude(string line)
        {
            var match = Regex.Match(line, NumberNumberPattern);
            if (match.Success)
            {
                return (3 * int.Parse(match.Groups[1].Value)) + (2 * int.Parse(match.Groups[2].Value));
            }

            match = Regex.Match(line, NumberPairPattern);
            if (match.Success)
            {
                return (3 * int.Parse(match.Groups[1].Value)) + (2 * GetMagnitude(match.Groups[2].Value));
            }

            match = Regex.Match(line, PairNumberPattern);
            if (match.Success)
            {
                return (3 * GetMagnitude(match.Groups[1].Value)) + (2 * int.Parse((match.Groups[2].Value)));
            }

            // Find splitting point
            int splitIndex = FindSplitIndex(line);
            return (3 * GetMagnitude(line.Substring(1, splitIndex - 1))) + (2 * GetMagnitude(line.Substring(splitIndex + 1, line.Length - splitIndex - 2)));
        }

        private static int FindSplitIndex(string line)
        {
            int bracket = 0;
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case ',' when bracket == 1:
                        return i;
                    case '[':
                        bracket++;
                        break;
                    case ']':
                        bracket--;
                        break;
                    default:
                        break;
                }
            }

            return -1;
        }
    }
}
