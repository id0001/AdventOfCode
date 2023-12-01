using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(11)]
public class Challenge11
{
    private static readonly Regex OperationPattern = new(@"new = old ([*+]) (old|\d+)");
    private static readonly Regex TestPattern = new(@"divisible by (\d+)");
    private static readonly Regex ActionPattern = new(@"throw to monkey (\d+)");

    private readonly IInputReader _inputReader;

    public Challenge11(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var monkeys = new List<Monkey>();
        var lines = new List<string>();
        await foreach (var line in _inputReader.ReadLinesAsync(11))
        {
            if (string.IsNullOrEmpty(line))
            {
                monkeys.Add(ParseMonkey(lines.ToArray()));
                lines.Clear();
                continue;
            }

            lines.Add(line[(line.IndexOf(':') + 1)..]);
        }

        // add last monkey
        monkeys.Add(ParseMonkey(lines.ToArray()));

        var inspectCount = new int[monkeys.Count];
        for (var round = 0; round < 20; round++)
        for (var m = 0; m < monkeys.Count; m++)
        {
            var monkey = monkeys[m];
            while (monkey.Items.Count > 0)
            {
                var item = monkey.Items.Dequeue();
                item = monkey.Operation(item);
                inspectCount[m]++;
                item /= 3;
                var test = monkey.Test(item);
                monkeys[test].Items.Enqueue(item);
            }
        }

        return inspectCount.OrderByDescending(x => x).Take(2).Product().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var monkeys = new List<Monkey>();
        var lines = new List<string>();
        await foreach (var line in _inputReader.ReadLinesAsync(11))
        {
            if (string.IsNullOrEmpty(line))
            {
                monkeys.Add(ParseMonkey(lines.ToArray()));
                lines.Clear();
                continue;
            }

            lines.Add(line[(line.IndexOf(':') + 1)..]);
        }

        // add last monkey
        monkeys.Add(ParseMonkey(lines.ToArray()));

        var lcm = monkeys.Product(x => (long) x.Divisor);

        var inspectCount = new ulong[monkeys.Count];
        for (var round = 0; round < 10000; round++)
        for (var m = 0; m < monkeys.Count; m++)
        {
            var monkey = monkeys[m];
            while (monkey.Items.Count > 0)
            {
                var item = monkey.Items.Dequeue();
                item = monkey.Operation(item);
                inspectCount[m]++;
                item %= (ulong) lcm;
                var test = monkey.Test(item);
                monkeys[test].Items.Enqueue(item);
            }
        }

        //if(new[] { 0, 19, 999, 1999, 2999, 3999, 4999, 5999, 6999, 7999, 8999, 9999 }.Contains(round))
        //{
        //    Console.WriteLine($"== After round {round + 1} ==");

        //    for (int i = 0; i < monkeys.Count; i++)
        //    {
        //        Console.WriteLine($"Monkey {i} inspected items {inspectCount[i]} times. [{string.Join(",", monkeys[i].Items)}]");
        //    }

        //    Console.WriteLine();
        //}

        return inspectCount.OrderByDescending(x => x).Take(2).Product().ToString();
    }

    private static Monkey ParseMonkey(string[] lines)
    {
        var startingItems =
            new Queue<ulong>(lines[1].Split(new[] {","}, StringSplitOptions.TrimEntries).Select(ulong.Parse));

        var operationPattern = OperationPattern.Match(lines[2]);
        Func<ulong, ulong, ulong> op = operationPattern.Groups[1].Value switch
        {
            "*" => Multiply,
            "+" => Sum,
            _ => throw new NotImplementedException()
        };

        var useOld = operationPattern.Groups[2].Value == "old";
        var oy = useOld ? 0 : ulong.Parse(operationPattern.Groups[2].Value);
        var operation = new Func<ulong, ulong>(x => op(x, useOld ? x : oy));

        var testPattern = TestPattern.Match(lines[3]);
        var truePattern = ActionPattern.Match(lines[4]);
        var falsePattern = ActionPattern.Match(lines[5]);

        var trueY = int.Parse(truePattern.Groups[1].Value);
        var falseY = int.Parse(falsePattern.Groups[1].Value);
        var ty = ulong.Parse(testPattern.Groups[1].Value);

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        var test = new Func<ulong, int>(x => x % ty == 0 ? trueY : falseY);

        return new Monkey(startingItems, operation, test, ty);
    }

    private static ulong Multiply(ulong a, ulong b)
    {
        return a * b;
    }

    private static ulong Sum(ulong a, ulong b)
    {
        return a + b;
    }

    private record Monkey(Queue<ulong> Items, Func<ulong, ulong> Operation, Func<ulong, int> Test, ulong Divisor);
}