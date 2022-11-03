using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(6)]
public class Challenge06
{
    private readonly IInputReader _inputReader;

    public Challenge06(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var yesCount = 0;
        ISet<char> answers = new HashSet<char>();
        await foreach (var line in _inputReader.ReadLinesAsync(6))
        {
            if (string.IsNullOrEmpty(line))
            {
                yesCount += answers.Count;
                answers.Clear();
                continue;
            }

            foreach (var c in line) answers.Add(c);
        }

        yesCount += answers.Count;

        return yesCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var yesCount = 0;
        var groupCount = 0;
        IDictionary<char, int> answers = new Dictionary<char, int>();
        await foreach (var line in _inputReader.ReadLinesAsync(6))
        {
            if (string.IsNullOrEmpty(line))
            {
                yesCount += answers.Count(e => e.Value == groupCount);
                answers.Clear();
                groupCount = 0;
                continue;
            }

            groupCount++;

            foreach (var c in line)
            {
                if (!answers.ContainsKey(c))
                    answers.Add(c, 0);

                answers[c] += 1;
            }
        }

        yesCount += answers.Count(e => e.Value == groupCount);

        return yesCount.ToString();
    }
}