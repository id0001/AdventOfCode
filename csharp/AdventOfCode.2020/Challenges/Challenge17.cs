using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(17)]
public class Challenge17
{
    private readonly IInputReader _inputReader;

    public Challenge17(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }


    [Part1]
    public async Task<string> Part1Async()
    {
        var state = new SparseSpatialMap<Point3, bool>();

        var lines = await _inputReader.ReadLinesAsync(17).ToArrayAsync();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == '#')
                state.Set(new Point3(x, y, 0), true);
        }

        for (var i = 0; i < 6; i++)
        {
            var newState = new SparseSpatialMap<Point3, bool>();

            for (var z = state.Bounds.GetMin(2) - 1; z < state.Bounds.GetMax(2) + 1; z++)
            for (var y = state.Bounds.GetMin(1) - 1; y < state.Bounds.GetMax(1) + 1; y++)
            for (var x = state.Bounds.GetMin(0) - 1; x < state.Bounds.GetMax(0) + 1; x++)
            {
                var p = new Point3(x, y, z);
                var neighbors = state.GetNeighbors(p, true).ToArray();
                switch (state.Get(p, false))
                {
                    case true when neighbors.Count(n => n.Value) is 2 or 3:
                        newState.Set(p, true);
                        break;
                    case false when neighbors.Count(n => n.Value) == 3:
                        newState.Set(p, true);
                        break;
                    default:
                        newState.Unset(p);
                        break;
                }
            }

            state = newState;
        }

        return state.Count(e => e.Value).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var state = new SparseSpatialMap<Point4, bool>();

        var lines = await _inputReader.ReadLinesAsync(17).ToArrayAsync();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == '#')
                state.Set(new Point4(x, y, 0, 0), true);
        }

        for (var i = 0; i < 6; i++)
        {
            var newState = new SparseSpatialMap<Point4, bool>();

            for (var w = state.Bounds.GetMin(3) - 1; w < state.Bounds.GetMax(3) + 1; w++)
            for (var z = state.Bounds.GetMin(2) - 1; z < state.Bounds.GetMax(2) + 1; z++)
            for (var y = state.Bounds.GetMin(1) - 1; y < state.Bounds.GetMax(1) + 1; y++)
            for (var x = state.Bounds.GetMin(0) - 1; x < state.Bounds.GetMax(0) + 1; x++)
            {
                var p = new Point4(x, y, z, w);
                var neighbors = state.GetNeighbors(p, true).ToArray();
                switch (state.Get(p, false))
                {
                    case true when neighbors.Count(n => n.Value) is 2 or 3:
                        newState.Set(p, true);
                        break;
                    case false when neighbors.Count(n => n.Value) == 3:
                        newState.Set(p, true);
                        break;
                    default:
                        newState.Unset(p);
                        break;
                }
            }

            state = newState;
        }

        return state.Count.ToString();
    }
}