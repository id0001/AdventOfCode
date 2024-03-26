using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.PathFinding;
using AdventOfCode.Lib;
using System.Collections;
using Spectre.Console;
using System.Reflection.Emit;
using System.ComponentModel;
using Spectre.Console.Rendering;

namespace AdventOfCode2016.Challenges;

[Challenge(11)]
public class Challenge11(IInputReader inputReader)
{
    private const int DeviceCount = 5;

    [Part1]
    public async Task<string> Part1Async()
    {
        int bits = 0;
        bits = SetLevel(bits, 0, 0); // TM
        bits = SetLevel(bits, 1, 1); // PM
        bits = SetLevel(bits, 2, 1); // SM
        bits = SetLevel(bits, 3, 2); // PRM
        bits = SetLevel(bits, 4, 2); // RM
        bits = SetLevel(bits, 5, 0); // TG
        bits = SetLevel(bits, 6, 0); // PG
        bits = SetLevel(bits, 7, 0); // SG
        bits = SetLevel(bits, 8, 2); // PRG
        bits = SetLevel(bits, 9, 2); // RG

        var state = new State(0, bits, 5);
        var bfg = new BreadthFirstSearch<State>(GetAdjacent);

        bfg.TryPath(state, s => s.GoalReached, out var path);
        return (path.Count() - 1).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        int bits = 0;
        bits = SetLevel(bits, 0, 0); // TM
        bits = SetLevel(bits, 1, 1); // PM
        bits = SetLevel(bits, 2, 1); // SM
        bits = SetLevel(bits, 3, 2); // PRM
        bits = SetLevel(bits, 4, 2); // RM
        bits = SetLevel(bits, 5, 0); // TG
        bits = SetLevel(bits, 6, 1); // EM
        bits = SetLevel(bits, 7, 1); // DM
        bits = SetLevel(bits, 8, 0); // PG
        bits = SetLevel(bits, 9, 0); // SG
        bits = SetLevel(bits, 10, 2); // PRG
        bits = SetLevel(bits, 11, 2); // RG
        bits = SetLevel(bits, 12, 1); // EG
        bits = SetLevel(bits, 13, 1); // DG

        var state = new State(0, bits, 7);
        var bfg = new BreadthFirstSearch<State>(GetAdjacent);

        bfg.TryPath(state, s => s.GoalReached, out var path);
        return (path.Count() - 1).ToString();
    }

    private IEnumerable<State> GetAdjacent(State current)
    {
        // Go up
        if (current.CurrentLevel < 3)
        {
            foreach (var next in GetNextNodes(current, 1))
                yield return next;
        }

        // Go down
        if (current.CurrentLevel > 0)
        {
            foreach (var next in GetNextNodes(current, -1))
                yield return next;
        }
    }

    private static IEnumerable<State> GetNextNodes(State current, int moveAmount)
    {
        var newLevel = (byte)(current.CurrentLevel + moveAmount);

        // Doubles
        foreach (var combination in GetIndicesOnLevel(current).Combinations(2))
        {
            var next = new State(newLevel, Move(Move(current.Bits, combination[0], moveAmount), combination[1], moveAmount), current.Count);
            if (next.IsValid)
                yield return next;
        }

        // Singles
        foreach (var index in GetIndicesOnLevel(current))
        {
            var next = new State(newLevel, Move(current.Bits, index, moveAmount), current.Count);
            if (next.IsValid)
                yield return next;
        }
    }

    private record State(byte CurrentLevel, int Bits, int Count)
    {
        public bool GoalReached => CurrentLevel == 3 && Enumerable.Range(0, Count * 2).All(i => GetLevel(Bits, i) == 3);

        public bool IsValid
        {
            get
            {
                if (!GetIndicesOnLevel(this).Any(i => i >= Count))
                    return true;

                return GetIndicesOnLevel(this).Where(i => i < Count).All(i => GetLevel(Bits, i + Count) == CurrentLevel);
            }
        }
    }

    private static int Move(int bits, int index, int amount) => SetLevel(bits, index, (byte)(GetLevel(bits, index) + amount));

    private static byte GetLevel(int bits, int index)
    {
        var position = index * 2;
        var mask = 0b11 << position;
        return (byte)((bits & mask) >> position);
    }

    private static int SetLevel(int bits, int index, byte value)
    {
        var position = index * 2;
        var mask = ~(0b11 << position);
        return (bits & mask) | (value << position);
    }

    private static IEnumerable<int> GetIndicesOnLevel(State state) => Enumerable
        .Range(0, state.Count * 2)
        .Where(i => GetLevel(state.Bits, (byte)i) == state.CurrentLevel);

    private static void PrintState(State state)
    {
        var table = new Table();
        var cols = new List<string>();
        cols.Add("F#");
        for (var i = 0; i < state.Count; i++)
            cols.Add($"M{i + 1}");

        for (var i = 0; i < state.Count; i++)
            cols.Add($"G{i + 1}");

        table.AddColumns(cols.ToArray());

        for (var level = 0; level < 4; level++)
        {
            var arr = Enumerable.Range(0, state.Count * 2).Select(i => new Markup(GetLevel(state.Bits, i) == level ? "O" : string.Empty)).ToArray();

            var levelMarkup = level == state.CurrentLevel
                ? new Markup($"[green]F{level}[/]")
                : new Markup($"F{level}");

            table.AddRow(new[] { levelMarkup }.Concat(arr).ToArray());
        }

        AnsiConsole.Write(table);
    }
}
