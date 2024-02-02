using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var keypad = new[,]
        {
            {'1', '2', '3'},
            {'3', '4', '5'},
            {'7', '8', '9'}
        };

        return string.Join(string.Empty, await inputReader.ReadLinesAsync(2)
            .Select(x => GetNumber(keypad, keypad.Bounds(), x))
            .ToArrayAsync());
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var keypad = new[,]
        {
            {'.', '.', '1', '.', '.'},
            {'.', '2', '3', '4', '.'},
            {'5', '6', '7', '8', '9'},
            {'.', 'A', 'B', 'C', '.'},
            {'.', '.', 'D', '.', '.'}
        };

        return string.Join(string.Empty, await inputReader.ReadLinesAsync(2)
            .Select(x => GetNumber(keypad, keypad.Bounds(), x))
            .ToArrayAsync());
    }

    private static char GetNumber(char[,] keypad, Rectangle bounds, IEnumerable<char> code) => code.Aggregate(
            new Point2(1, 1), (a, b) => b switch
            {
                'U' when bounds.Contains(a.Up) && keypad[a.Up.Y, a.Up.X] != '.' => a.Up,
                'R' when bounds.Contains(a.Right) && keypad[a.Right.Y, a.Right.X] != '.' => a.Right,
                'D' when bounds.Contains(a.Down) && keypad[a.Down.Y, a.Down.X] != '.' => a.Down,
                'L' when bounds.Contains(a.Left) && keypad[a.Left.Y, a.Left.X] != '.' => a.Left,
                _ => a
            })
        .Into(p => keypad[p.Y, p.X]);
}