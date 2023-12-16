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
        var state = new PointCloud<Point3>();

        var lines = await _inputReader.ReadLinesAsync(17).ToArrayAsync();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == '#')
                state.Set(new Point3(x, y, 0));
        }

        for (var i = 0; i < 6; i++)
        {
            var newState = new PointCloud<Point3>();

            for (var z = state.Bounds.GetMin(2) - 1; z <= state.Bounds.GetMax(2) + 1; z++)
            for (var y = state.Bounds.GetMin(1) - 1; y <= state.Bounds.GetMax(1) + 1; y++)
            for (var x = state.Bounds.GetMin(0) - 1; x <= state.Bounds.GetMax(0) + 1; x++)
            {
                var p = new Point3(x, y, z);
                var neighborCount = p.GetNeighbors(true).Count(state.Contains);
                switch (state.Contains(p))
                {
                    case true when neighborCount is 2 or 3:
                        newState.Set(p);
                        break;
                    case false when neighborCount == 3:
                        newState.Set(p);
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

    [Part2]
    public async Task<string> Part2Async()
    {
        var state = new PointCloud<Point4>();

        var lines = await _inputReader.ReadLinesAsync(17).ToArrayAsync();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == '#')
                state.Set(new Point4(x, y, 0, 0));
        }

        for (var i = 0; i < 6; i++)
        {
            var newState = new PointCloud<Point4>();

            for (var w = state.Bounds.GetMin(3) - 1; w <= state.Bounds.GetMax(3) + 1; w++)
            for (var z = state.Bounds.GetMin(2) - 1; z <= state.Bounds.GetMax(2) + 1; z++)
            for (var y = state.Bounds.GetMin(1) - 1; y <= state.Bounds.GetMax(1) + 1; y++)
            for (var x = state.Bounds.GetMin(0) - 1; x <= state.Bounds.GetMax(0) + 1; x++)
            {
                var p = new Point4(x, y, z, w);
                var neighborCount = p.GetNeighbors(true).Count(state.Contains);
                switch (state.Contains(p))
                {
                    case true when neighborCount is 2 or 3:
                        newState.Set(p);
                        break;
                    case false when neighborCount == 3:
                        newState.Set(p);
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