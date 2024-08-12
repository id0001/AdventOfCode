using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    private const int StepsTillDiagnostic = 12208951;

    [Part1]
    public async Task<string> Part1Async()
    {
        var rules = ParseRules(await inputReader.ReadAllTextAsync(25));

        var currentState = "A";
        var cursor = 0;
        var tape = new Dictionary<int, int>();

        for(var i = 0; i < StepsTillDiagnostic; i++)
        {
            var rule = rules[currentState];
            var v = tape.GetValueOrDefault(cursor, 0);
            if(v == 0)
            {
                tape[cursor] = rule.WriteZero;
                cursor += rule.MoveZero == "left" ? -1 : 1;
                currentState = rule.NextZero;
            }
            else
            {
                tape[cursor] = rule.writeOne;
                cursor += rule.MoveOne == "left" ? -1 : 1;
                currentState = rule.NextOne;
            }
        }

        return tape.Count(kv => kv.Value == 1).ToString();
    }

    private Dictionary<string, Rule> ParseRules(string text)
    {
        var nl = Environment.NewLine;
        return text.SplitBy($"{nl}{nl}").Skip(1).Select(text => text.SplitBy(nl).Into(lines =>
        {
            var state = lines[0].Extract(@"In state (.):").First();
            var writeZero = lines[2].Extract(@"Write the value (\d).").First().As<int>();
            var moveZero = lines[3].Extract(@"(left|right)").First();
            var nextZero = lines[4].Extract(@"Continue with state (.)\.").First();

            var writeOne = lines[6].Extract(@"Write the value (\d).").First().As<int>();
            var moveOne = lines[7].Extract(@"(left|right)").First();
            var nextOne = lines[8].Extract(@"Continue with state (.)\.").First();

            return new Rule(state, writeZero, moveZero, nextZero, writeOne, moveOne, nextOne);
        })).ToDictionary(kv => kv.State);
    }

    private record Rule(string State, int WriteZero, string MoveZero, string NextZero, int writeOne, string MoveOne, string NextOne);
}