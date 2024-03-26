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
        short rtgs = 0;
        rtgs = SetLevel(rtgs, 0, 1); // TG
        rtgs = SetLevel(rtgs, 1, 1); // PG
        rtgs = SetLevel(rtgs, 2, 1); // SG
        rtgs = SetLevel(rtgs, 3, 3); // PRG
        rtgs = SetLevel(rtgs, 4, 3); // RG

        short chips = 0;
        chips = SetLevel(chips, 0, 1); // TM
        chips = SetLevel(chips, 1, 2); // PM
        chips = SetLevel(chips, 2, 2); // SM
        chips = SetLevel(chips, 3, 3); // PRM
        chips = SetLevel(chips, 4, 3); // RM

        var state = new State(1, rtgs, chips);
        var bfg = new BreadthFirstSearch<State>(GetAdjacent);

        bfg.TryPath(state, s => s.GoalReached, out var path);

        return (path.Count() - 1).ToString();
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
    }

    private IEnumerable<State> GetAdjacent(State current)
    {
        var currentChips = GetIndicesOnLevel(current.Chips, current.CurrentLevel).ToArray();
        var currentRtgs = GetIndicesOnLevel(current.Rtgs, current.CurrentLevel).ToArray();

        // Go down
        if (current.CurrentLevel > 1)
        {
            foreach (var next in GetNextNodes(current, currentChips, currentRtgs, -1))
                yield return next;
        }

        // Go up
        if (current.CurrentLevel < 4)
        {
            foreach (var next in GetNextNodes(current, currentChips, currentRtgs, 1))
                yield return next;
        }
    }

    private static IEnumerable<State> GetNextNodes(State current, byte[] chipsAtLevel, byte[] rtgsAtLevel, int moveAmount)
    {
        var newLevel = (byte)(current.CurrentLevel + moveAmount);

        // Single chip
        foreach (var chip in chipsAtLevel)
        {
            var chips = Move(current.Chips, chip, moveAmount);
            if (IsValid(current.Rtgs, chips, newLevel))
                yield return new State(newLevel, current.Rtgs, chips);
        }

        // Single rtg
        foreach (var rtg in rtgsAtLevel)
        {
            var rtgs = Move(current.Rtgs, rtg, moveAmount);
            if (IsValid(rtgs, current.Chips, newLevel))
                yield return new State(newLevel, rtgs, current.Chips);
        }

        // 2 chips
        for (var i = 0; i < chipsAtLevel.Length - 1; i++)
        {
            for (var j = i; j < chipsAtLevel.Length; j++)
            {
                var chips = current.Chips;
                chips = Move(chips, chipsAtLevel[i], moveAmount);
                chips = Move(chips, chipsAtLevel[j], moveAmount);

                if (IsValid(current.Rtgs, chips, newLevel))
                    yield return new State(newLevel, current.Rtgs, chips);
            }
        }

        // 2 rtgs
        for (var i = 0; i < rtgsAtLevel.Length - 1; i++)
        {
            for (var j = i; j < rtgsAtLevel.Length; j++)
            {
                var rtgs = current.Rtgs;
                rtgs = Move(rtgs, rtgsAtLevel[i], moveAmount);
                rtgs = Move(rtgs, rtgsAtLevel[j], moveAmount);

                if (IsValid(rtgs, current.Chips, newLevel))
                    yield return new State(newLevel, rtgs, current.Chips);
            }
        }

        // 1 chip + 1 rtg
        foreach (var chip in chipsAtLevel)
        {
            var chips = Move(current.Chips, chip, moveAmount);

            foreach (var rtg in rtgsAtLevel)
            {
                var rtgs = Move(current.Rtgs, rtg, moveAmount);

                if (IsValid(rtgs, chips, newLevel))
                    yield return new State(newLevel, rtgs, chips);
            }
        }
    }

    private record State(byte CurrentLevel, short Rtgs, short Chips)
    {
        public bool GoalReached => CurrentLevel == 4 && Enumerable.Range(0, DeviceCount).All(i => GetLevel(Rtgs, (byte)i) == 4 && GetLevel(Chips, (byte)i) == 4);
    }

    private static short Move(short bits, byte index, int amount) => SetLevel(bits, index, (byte)(GetLevel(bits, index) + amount));

    private static byte GetLevel(short bits, byte index)
    {
        var position = index * 3;
        var mask = 0b111 << position;
        return (byte)((bits & mask) >> position);
    }

    private static short SetLevel(short bits, byte index, byte value)
    {
        var vbits = value & 0b111;

        var position = index * 3;
        var mask = ~(0b111 << position);
        return (short)((bits & mask) | (vbits << position));
    }

    private static bool IsValid(short rtgs, short chips, byte level)
    {
        if (!GetIndicesOnLevel(rtgs, level).Any())
            return true;

        if (GetIndicesOnLevel(chips, level).All(i => GetLevel(rtgs, i) == level))
            return true;

        return false;
    }

    private static IEnumerable<byte> GetIndicesOnLevel(short bits, byte level) => Enumerable.Range(0, DeviceCount).Where(i => GetLevel(bits, (byte)i) == level).Select(i => (byte)i);

    private static void PrintState(State state)
    {
        var table = new Table();
        table.AddColumns("F#", "HG", "LG", "HM", "LM");

        for (var level = 1; level < 5; level++)
        {
            var r = Enumerable.Range(0, DeviceCount).Select(i => new Markup(GetLevel(state.Rtgs, (byte)i) == level ? "O" : "")).ToArray();
            var c = Enumerable.Range(0, DeviceCount).Select(i => new Markup(GetLevel(state.Chips, (byte)i) == level ? "O" : "")).ToArray();

            var levelMarkup = level == state.CurrentLevel
                ? new Markup($"[green]F{level}[/]")
                : new Markup($"F{level}");

            table.AddRow(new[] { levelMarkup }.Concat(r).Concat(c).ToArray());
        }

        AnsiConsole.Write(table);
    }
}
