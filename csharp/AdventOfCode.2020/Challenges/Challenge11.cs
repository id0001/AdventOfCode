using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(11)]
public class Challenge11
{
    private readonly IInputReader _inputReader;
    private char[] _input = Array.Empty<char>();
    private int _width;
    private int _height;

    public Challenge11(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await _inputReader.ReadLinesAsync(11).ToArrayAsync();
        _height = lines.Length;
        _width = lines[0].Length;
        _input = lines.SelectMany(line => line.ToCharArray()).ToArray();
    }

    [Part1]
    public string Part1()
    {
        var state = new char[_input.Length];
        Array.Copy(_input, state, state.Length);

        bool stateChanged;
        do
        {
            stateChanged = false;
            var newState = new char[state.Length];
            for (var y = 0; y < _height; y++)
            for (var x = 0; x < _width; x++)
                stateChanged |= state[Index(x, y)] switch
                {
                    '.' => Ignore(newState, x, y),
                    'L' => Occupy1(state, newState, x, y),
                    '#' => Empty1(state, newState, x, y),
                    _ => throw new NotSupportedException()
                };

            state = newState;
        } while (stateChanged);

        return state.Count(e => e == '#').ToString();
    }

    [Part2]
    public string Part2()
    {
        var state = new char[_input.Length];
        Array.Copy(_input, state, state.Length);

        bool stateChanged;
        do
        {
            stateChanged = false;
            var newState = new char[state.Length];
            for (var y = 0; y < _height; y++)
            for (var x = 0; x < _width; x++)
                stateChanged |= state[Index(x, y)] switch
                {
                    '.' => Ignore(newState, x, y),
                    'L' => Occupy2(state, newState, x, y),
                    '#' => Empty2(state, newState, x, y),
                    _ => throw new NotSupportedException()
                };

            state = newState;
        } while (stateChanged);

        return state.Count(e => e == '#').ToString();
    }

    private bool Ignore(IList<char> newState, int px, int py)
    {
        newState[Index(px, py)] = '.';
        return false;
    }

    private bool Occupy1(IReadOnlyList<char> oldState, char[] newState, int px, int py)
    {
        for (var y = py - 1; y <= py + 1; y++)
        for (var x = px - 1; x <= px + 1; x++)
            if (!(x == px && y == py) && WithinBoundaries(x, y) && oldState[Index(x, y)] == '#')
            {
                newState[Index(px, py)] = 'L';
                return false;
            }

        newState[Index(px, py)] = '#';
        return true;
    }

    private bool Occupy2(char[] oldState, IList<char> newState, int px, int py)
    {
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
            if (!(x == 0 && y == 0) && CastRay(oldState, px, py, x, y) == '#')
            {
                newState[Index(px, py)] = 'L';
                return false;
            }

        newState[Index(px, py)] = '#';
        return true;
    }

    private bool Empty1(IReadOnlyList<char> oldState, IList<char> newState, int px, int py)
    {
        var count = 0;
        for (var y = py - 1; y <= py + 1; y++)
        for (var x = px - 1; x <= px + 1; x++)
            if (!(x == px && y == py) && WithinBoundaries(x, y) && oldState[Index(x, y)] == '#' && ++count >= 4)
            {
                newState[Index(px, py)] = 'L';
                return true;
            }

        newState[Index(px, py)] = '#';
        return false;
    }

    private bool Empty2(char[] oldState, IList<char> newState, int px, int py)
    {
        var count = 0;
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
            if (!(x == 0 && y == 0) && CastRay(oldState, px, py, x, y) == '#' && ++count >= 5)
            {
                newState[Index(px, py)] = 'L';
                return true;
            }

        newState[Index(px, py)] = '#';
        return false;
    }

    private int Index(int x, int y) => y * _width + x;

    private bool WithinBoundaries(int x, int y) => x >= 0 && y >= 0 && x < _width && y < _height;

    private char CastRay(IReadOnlyList<char> map, int sx, int sy, int dx, int dy)
    {
        var c = '.';
        var x = sx + dx;
        var y = sy + dy;
        while (WithinBoundaries(x, y))
        {
            if (map[Index(x, y)] != '.')
            {
                c = map[Index(x, y)];
                break;
            }

            x += dx;
            y += dy;
        }

        return c;
    }
}