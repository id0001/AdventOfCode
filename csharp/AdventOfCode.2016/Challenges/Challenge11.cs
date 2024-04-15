using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2016.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part1]
    public string Part1()
    {
        var bits = 0;
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
    public string Part2()
    {
        var bits = 0;
        bits = SetLevel(bits, 0, 0); // TM
        bits = SetLevel(bits, 1, 1); // PM
        bits = SetLevel(bits, 2, 1); // SM
        bits = SetLevel(bits, 3, 2); // PRM
        bits = SetLevel(bits, 4, 2); // RM
        bits = SetLevel(bits, 5, 0); // EM
        bits = SetLevel(bits, 6, 0); // DM

        bits = SetLevel(bits, 7, 0); // TG
        bits = SetLevel(bits, 8, 0); // PG
        bits = SetLevel(bits, 9, 0); // SG
        bits = SetLevel(bits, 10, 2); // PRG
        bits = SetLevel(bits, 11, 2); // RG
        bits = SetLevel(bits, 12, 0); // EG
        bits = SetLevel(bits, 13, 0); // DG

        var state = new State(0, bits, 7);
        var bfg = new BreadthFirstSearch<State>(GetAdjacent);

        bfg.TryPath(state, s => s.GoalReached, out var path);

        return (path.Count() - 1).ToString();
    }

    private IEnumerable<State> GetAdjacent(State current)
    {
        // Go up
        if (current.CurrentLevel < 3)
            foreach (var next in GetNextNodes(current, 1))
                yield return next;

        // Go down
        if (current.CurrentLevel > 0)
            foreach (var next in GetNextNodes(current, -1))
                yield return next;
    }

    private static IEnumerable<State> GetNextNodes(State current, int moveAmount)
    {
        var newLevel = (byte) (current.CurrentLevel + moveAmount);

        // Doubles
        foreach (var combination in GetIndicesOnLevel(current, current.CurrentLevel).Combinations(2))
        {
            var next = new State(newLevel,
                Move(Move(current.Bits, combination[0], moveAmount), combination[1], moveAmount), current.PairCount);
            if (next.IsValid)
                yield return next;
        }

        // Singles
        foreach (var index in GetIndicesOnLevel(current, current.CurrentLevel))
        {
            var next = new State(newLevel, Move(current.Bits, index, moveAmount), current.PairCount);
            if (next.IsValid)
                yield return next;
        }
    }

    private static int Move(int bits, int index, int amount) =>
        SetLevel(bits, index, (byte) (GetLevel(bits, index) + amount));

    private static byte GetLevel(int bits, int index)
    {
        var position = index * 2;
        var mask = 0b11 << position;
        return (byte) ((bits & mask) >> position);
    }

    private static int SetLevel(int bits, int index, byte value)
    {
        var position = index * 2;
        var mask = ~(0b11 << position);
        return (bits & mask) | (value << position);
    }

    private static IEnumerable<int> GetIndicesOnLevel(State state, int level) => Enumerable
        .Range(0, state.PairCount * 2)
        .Where(i => GetLevel(state.Bits, (byte) i) == level);

    private sealed record State(byte CurrentLevel, int Bits, int PairCount)
    {
        public bool GoalReached =>
            CurrentLevel == 3 && Enumerable.Range(0, PairCount * 2).All(i => GetLevel(Bits, i) == 3);

        public bool IsValid
        {
            get
            {
                return Enumerable
                    .Range(0, 4)
                    .All(level =>
                    {
                        if (!GetIndicesOnLevel(this, level).Any(i => i >= PairCount))
                            return true;

                        return GetIndicesOnLevel(this, level).Where(i => i < PairCount)
                            .All(i => GetLevel(Bits, i + PairCount) == level);
                    });
            }
        }

        public bool Equals(State? other)
        {
            if (other is null)
                return false;

            return GeEquivalenceArray().SequenceEqual(other.GeEquivalenceArray());
        }

        private int[] GeEquivalenceArray()
        {
            return Enumerable
                .Range(0, 4)
                .Select(level =>
                {
                    var indices = GetIndicesOnLevel(this, level).ToArray();
                    var generators = indices.Where(i => i >= PairCount).ToArray();
                    var chips = indices.Where(i => i < PairCount).ToArray();
                    var pairs = chips.Count(i => GetLevel(Bits, i + PairCount) == level);
                    return chips.Length | (generators.Length << 8) | (pairs << 16) | (CurrentLevel << 24);
                })
                .ToArray();
        }

        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach (var v in GeEquivalenceArray())
                hc.Add(v);

            return hc.ToHashCode();
        }
    }
}