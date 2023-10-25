using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(23)]
public class Challenge23
{
    private const byte NW_N_NE = 0x07;
    private const byte SW_S_SE = 0xE0;
    private const byte NW_W_SW = 0x29;
    private const byte NE_E_SE = 0x94;

    private readonly IInputReader _inputReader;

    public Challenge23(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var map = await _inputReader.ReadGridAsync<char>(23);

        var state = new PointCloud<Point2>();
        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == '#')
                    state.Set(new Point2(x, y));
            }
        }

        var directions = new[] { NW_N_NE, SW_S_SE, NW_W_SW, NE_E_SE };
        var movement = new[] { new Point2(0, -1), new Point2(0, 1), new Point2(-1, 0), new Point2(1, 0) };
        var dirIndex = 0;

        for (var round = 0; round < 10; round++)
        {
            var proposedMoves = state.ToDictionary(kv => kv);
            var newState = new PointCloud<Point2>();

            foreach (var elf in state)
            {
                byte bits = elf.GetNeighbors(true)
                    .Select((n, i) => new { Neighbor = n, Index = i })
                    .Aggregate((byte)0, (bits, item) => bits |= state.Contains(item.Neighbor) ? (byte)(1 << item.Index) : (byte)0);

                if (bits == 0)
                    continue;

                for (var i = 0; i < 4; i++)
                {
                    if ((bits & directions[Euclid.Modulus(dirIndex + i, 4)]) == 0)
                    {
                        proposedMoves[elf] = elf + movement[Euclid.Modulus(dirIndex + i, 4)];
                        break;
                    }
                }
            }

            var grouped = proposedMoves.GroupBy(kv => kv.Value, kv => kv.Key);
            foreach (var group in grouped)
            {
                if (group.Count() > 1)
                {
                    foreach (var elf in group)
                        newState.Set(elf);
                }
                else
                {
                    newState.Set(group.Key);
                }
            }

            state = newState;
            dirIndex++;
        }

        int emptySpaces = 0;
        for (var y = state.Bounds.GetMin(1); y < state.Bounds.GetMax(1); y++)
        {
            for (var x = state.Bounds.GetMin(0); x < state.Bounds.GetMax(0); x++)
            {
                if (!state.Contains(new Point2(x, y)))
                    emptySpaces++;
            }
        }

        return emptySpaces.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var map = await _inputReader.ReadGridAsync<char>(23);

        var state = new PointCloud<Point2>();
        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == '#')
                    state.Set(new Point2(x, y));
            }
        }

        var directions = new[] { NW_N_NE, SW_S_SE, NW_W_SW, NE_E_SE };
        var movement = new[] { new Point2(0, -1), new Point2(0, 1), new Point2(-1, 0), new Point2(1, 0) };
        var dirIndex = 0;

        int round = 0;
        bool moved = true;
        while (moved)
        {
            moved = false;
            var proposedMoves = state.ToDictionary(kv => kv);
            var newState = new PointCloud<Point2>();

            foreach (var elf in state)
            {
                byte bits = elf.GetNeighbors(true)
                    .Select((n, i) => new { Neighbor = n, Index = i })
                    .Aggregate((byte)0, (bits, item) => bits |= state.Contains(item.Neighbor) ? (byte)(1 << item.Index) : (byte)0);

                if (bits == 0)
                    continue;

                for (var i = 0; i < 4; i++)
                {
                    if ((bits & directions[Euclid.Modulus(dirIndex + i, 4)]) == 0)
                    {
                        proposedMoves[elf] = elf + movement[Euclid.Modulus(dirIndex + i, 4)];
                        break;
                    }
                }
            }

            var grouped = proposedMoves.GroupBy(kv => kv.Value, kv => kv.Key);
            foreach (var group in grouped)
            {
                if (group.Count() > 1)
                {
                    foreach (var elf in group)
                        newState.Set(elf);
                }
                else
                {
                    newState.Set(group.Key);

                    if (group.Key != group.First())
                        moved = true;
                }
            }

            state = newState;
            dirIndex++;
            round++;
        }

        return round.ToString();
    }
}
