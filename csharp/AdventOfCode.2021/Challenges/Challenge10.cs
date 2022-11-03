using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(10)]
public class Challenge10
{
    private readonly IInputReader _inputReader;

    public Challenge10(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await _inputReader.ReadLinesAsync(10).ToArrayAsync();

        return input
            .Select(line => Validate(line, new Stack<char>()) switch
            {
                Fail fail => Score1(fail.Char),
                _ => 0
            })
            .Sum()
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await _inputReader.ReadLinesAsync(10).ToArrayAsync();

        var arr = input
            .Select(line =>
            {
                var stack = new Stack<char>();
                return Validate(line, stack) switch
                {
                    Success => Score2(stack),
                    _ => 0L
                };
            })
            .Where(x => x > 0)
            .OrderBy(x => x)
            .ToArray();

        return arr[arr.Length / 2].ToString();
    }

    private static int Score1(char c) => c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new NotSupportedException()
    };

    private static long Score2(IEnumerable<char> stack) => stack.Aggregate(0L, (acc, c) => c switch
    {
        '(' => acc * 5L + 1L,
        '[' => acc * 5L + 2L,
        '{' => acc * 5L + 3L,
        '<' => acc * 5L + 4L,
        _ => throw new NotSupportedException()
    });

    private static Result Validate(string list, Stack<char> stack)
    {
        foreach (var c in list)
        {
            var result = AddChar(c, stack);
            if (result is Fail)
                return result;
        }

        return new Success();
    }

    private static Result AddChar(char c, Stack<char> stack)
    {
        switch (c)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                stack.Push(c);
                return new Success();
            case ')':
            case ']':
            case '}':
            case '>':
                var l = GetMatchingLeft(c);
                if (stack.Peek() == l)
                {
                    stack.Pop();
                    return new Success();
                }

                return new Fail(c);
            default:
                throw new NotSupportedException();
        }
    }

    private static char GetMatchingLeft(char c) => c switch
    {
        ')' => '(',
        ']' => '[',
        '}' => '{',
        '>' => '<',
        _ => throw new NotSupportedException()
    };

    private record Result;

    private record Success : Result;

    private record Fail(char Char) : Result;
}