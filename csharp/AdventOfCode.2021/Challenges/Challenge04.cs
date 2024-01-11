using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(4)]
public class Challenge04(IInputReader InputReader)
{
    private const int BoardLength = 5;
    private readonly List<int[]> _boards = new();

    private readonly Queue<int> _numbers = new();

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await InputReader.ReadLinesAsync(4).ToArrayAsync();
        foreach (var item in lines[0].Split(',').Select(int.Parse))
            _numbers.Enqueue(item);

        var board = new List<int>();
        for (var i = 2; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                _boards.Add(board.ToArray());
                board.Clear();
                continue;
            }

            board.AddRange(lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse));
        }

        _boards.Add(board.ToArray());
    }

    [Part1]
    public string? Part1()
    {
        var score = Enumerable.Range(0, _boards.Count).Select(_ => new int[BoardLength * 2]).ToList();
        var history = new List<int>();

        while (_numbers.Count > 0)
        {
            var number = _numbers.Dequeue();
            history.Add(number);

            for (var b = 0; b < _boards.Count; b++)
            {
                var index = Array.IndexOf(_boards[b], number);
                if (index == -1)
                    continue;

                var y = index / BoardLength;
                var x = index - y * BoardLength;

                score[b][x]++;
                score[b][y + BoardLength]++;

                if (score[b].Any(t => t == 5))
                    // Winner winner chicken dinner
                    return CalculateScore(_boards[b], history, number).ToString();
            }
        }

        return null;
    }

    [Part2]
    public string? Part2()
    {
        var score = Enumerable.Range(0, _boards.Count).Select(_ => new int[BoardLength * 2]).ToList();
        var history = new List<int>();

        while (_numbers.Count > 0)
        {
            var number = _numbers.Dequeue();
            history.Add(number);

            for (var b = 0; b < _boards.Count; b++)
            {
                var index = Array.IndexOf(_boards[b], number);
                if (index == -1)
                    continue;

                var y = index / BoardLength;
                var x = index - y * BoardLength;

                score[b][x]++;
                score[b][y + BoardLength]++;

                if (score.All(s => s.Any(t => t == 5)))
                    // Winner winner chicken dinner
                    return CalculateScore(_boards[b], history, number).ToString();
            }
        }

        return null;
    }

    private static int CalculateScore(IEnumerable<int> board, IEnumerable<int> numbers, int lastNumberCalled)
    {
        var unmarked = board.Except(numbers).Sum();
        return unmarked * lastNumberCalled;
    }
}