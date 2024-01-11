using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (workflows, parts) = ParseInput(await InputReader.ReadAllTextAsync(19));

        var accepted = new List<Part>();

        foreach (var part in parts)
        {
            var workflow = workflows["in"];
            while (true)
            {
                var result = Evaluate(part, workflow);
                if (result.IsAccepted)
                {
                    accepted.Add(part);
                    break;
                }

                if (result.IsRejected)
                    break;

                workflow = workflows[result.Jump];
            }
        }

        return accepted.Select(a => a.X + a.M + a.A + a.S).Sum().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (workflows, _) = ParseInput(await InputReader.ReadAllTextAsync(19));

        var input = new Ranges(1, 4000, 1, 4000, 1, 4000, 1, 4000);

        var combinations = Analyze(workflows, input, workflows["in"]);
        return combinations.ToString();
    }

    private Result Evaluate(Part input, string line)
    {
        if (!TryParseRule(line, out var rule))
            return new Result(line == "A", line == "R", line);

        var value = GetComponent(input, rule.Component);
        switch (rule.Operator)
        {
            case '>' when value > rule.Value:
                return Evaluate(input, rule.Truthy);
            case '<' when value < rule.Value:
                return Evaluate(input, rule.Truthy);
            default:
                return Evaluate(input, rule.Falsy);
        }
    }

    private long Analyze(Dictionary<string, string> workflows, Ranges ranges, string line)
    {
        if (ranges.XFrom >= ranges.XTo || ranges.MFrom >= ranges.MTo || ranges.AFrom >= ranges.ATo ||
            ranges.SFrom >= ranges.STo)
            return 0;

        if (line == "A")
            return ranges.Combinations;

        if (line == "R")
            return 0;

        if (!TryParseRule(line, out var rule))
            return Analyze(workflows, ranges, workflows[line]);

        long combinations = 0;
        switch (rule.Component)
        {
            case 'x':
                if (rule.Operator == '<')
                {
                    combinations += Analyze(workflows, ranges with {XTo = rule.Value - 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {XFrom = rule.Value}, rule.Falsy);
                }
                else if (rule.Operator == '>')
                {
                    combinations += Analyze(workflows, ranges with {XFrom = rule.Value + 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {XTo = rule.Value}, rule.Falsy);
                }

                break;
            case 'm':
                if (rule.Operator == '<')
                {
                    combinations += Analyze(workflows, ranges with {MTo = rule.Value - 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {MFrom = rule.Value}, rule.Falsy);
                }
                else if (rule.Operator == '>')
                {
                    combinations += Analyze(workflows, ranges with {MFrom = rule.Value + 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {MTo = rule.Value}, rule.Falsy);
                }

                break;
            case 'a':
                if (rule.Operator == '<')
                {
                    combinations += Analyze(workflows, ranges with {ATo = rule.Value - 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {AFrom = rule.Value}, rule.Falsy);
                }
                else if (rule.Operator == '>')
                {
                    combinations += Analyze(workflows, ranges with {AFrom = rule.Value + 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {ATo = rule.Value}, rule.Falsy);
                }

                break;
            case 's':
                if (rule.Operator == '<')
                {
                    combinations += Analyze(workflows, ranges with {STo = rule.Value - 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {SFrom = rule.Value}, rule.Falsy);
                }
                else if (rule.Operator == '>')
                {
                    combinations += Analyze(workflows, ranges with {SFrom = rule.Value + 1}, rule.Truthy);
                    combinations += Analyze(workflows, ranges with {STo = rule.Value}, rule.Falsy);
                }

                break;
            default:
                throw new NotImplementedException();
        }

        return combinations;
    }

    private static int GetComponent(Part input, char component) => component switch
    {
        'x' => input.X,
        'm' => input.M,
        'a' => input.A,
        's' => input.S,
        _ => throw new ArgumentOutOfRangeException(nameof(component))
    };

    private static (Dictionary<string, string>, List<Part>) ParseInput(string text)
    {
        var nl = Environment.NewLine;
        var split = text.SplitBy($"{nl}{nl}");

        var workflows = split.First()
            .SplitBy(nl)
            .Select(ParseWorkflow)
            .ToDictionary(kv => kv.Item1, kv => kv.Item2);

        var parts = split
            .Second()
            .SplitBy(nl)
            .Select(line => Regex.Match(line, @"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}")
                .Groups.Values
                .Skip(1)
                .Select(g => g.Value.As<int>())
                .ToList()
                .Into(group => new Part(group[0], group[1], group[2], group[3]))
            ).ToList();

        return (workflows, parts);
    }

    private static (string, string) ParseWorkflow(string line)
    {
        return Regex.Match(line, @"(\w+)\{(.+)\}")
            .Groups.Values
            .Skip(1)
            .Select(g => g.Value)
            .ToList()
            .Into(group => (group[0], group[1]));
    }

    private bool TryParseRule(string line, out Rule rule)
    {
        if (!line.Contains(':'))
        {
            rule = new Rule('-', '-', 0, string.Empty, string.Empty);
            return false;
        }

        var indexOfTrue = line.IndexOf(":");
        var indexOfFalse = line.IndexOf(",");

        var condition = line[..indexOfTrue];
        var truthy = line[(indexOfTrue + 1)..indexOfFalse];
        var falsy = line[(indexOfFalse + 1)..];

        rule = Regex.Match(condition, @"(\w)([\<\>])(\d+)")
            .Groups.Values
            .Skip(1)
            .Select(x => x.Value)
            .ToList()
            .Into(group => new Rule(group[0].As<char>(), group[1].As<char>(), group[2].As<int>(), truthy, falsy));

        return true;
    }

    public record Part(int X, int M, int A, int S);

    public record Rule(char Component, char Operator, int Value, string Truthy, string Falsy);

    public record Result(bool IsAccepted, bool IsRejected, string Jump);

    public record Ranges(int XFrom, int XTo, int MFrom, int MTo, int AFrom, int ATo, int SFrom, int STo)
    {
        public long Combinations =>
            (long) (XTo - XFrom + 1) * (MTo - MFrom + 1) * (ATo - AFrom + 1) * (STo - SFrom + 1);
    }
}