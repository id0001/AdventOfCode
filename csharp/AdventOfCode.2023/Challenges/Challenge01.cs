using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2023.Challenges;

[Challenge(1)]
public class Challenge01
{
    private readonly IInputReader _inputReader;

    public Challenge01(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var sum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(1))
        {
            char? firstNum = null;
            char? lastNum = null;

            var i = 0;
            var j = line.Length - 1;
            foreach (var _ in line)
            {
                if (!firstNum.HasValue)
                {
                    if (char.IsDigit(line[i]))
                        firstNum = line[i];

                    i++;
                }

                if (!lastNum.HasValue)
                {
                    if (char.IsDigit(line[j]))
                        lastNum = line[j];

                    j--;
                }

                if (!firstNum.HasValue || !lastNum.HasValue) 
                    continue;
                
                sum += int.Parse($"{firstNum}{lastNum}");
                break;
            }
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var numbers = new[] {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        var pattern = new Regex("(zero|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled);

        var sum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(1))
        {
            string firstNum = null;
            string lastNum = null;

            var i = 0;
            var j = line.Length - 1;
            foreach (var _ in line)
            {
                if (string.IsNullOrEmpty(firstNum))
                {
                    if (char.IsDigit(line[i]))
                    {
                        firstNum = line[i].ToString();
                    }
                    else
                    {
                        var match = pattern.Match(line[..(i + 1)]);
                        if (match.Success)
                            firstNum = Array.IndexOf(numbers, match.Groups[1].Value).ToString();
                    }

                    i++;
                }

                if (string.IsNullOrEmpty(lastNum))
                {
                    if (char.IsDigit(line[j]))
                    {
                        lastNum = line[j].ToString();
                    }
                    else
                    {
                        var match = pattern.Match(line[j..]);
                        if (match.Success)
                            lastNum = Array.IndexOf(numbers, match.Groups[1].Value).ToString();
                    }

                    j--;
                }

                if (string.IsNullOrEmpty(firstNum) || string.IsNullOrEmpty(lastNum)) 
                    continue;
                
                sum += int.Parse($"{firstNum}{lastNum}");
                break;
            }
        }

        return sum.ToString();
    }
}