using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(10)]
    public class Challenge10
    {
        private readonly IInputReader inputReader;
        private string[] input;

        public Challenge10(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            input = await inputReader.ReadLinesAsync(10).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
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
        public string Part2()
        {
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

        private int Score1(char c) => c switch
        {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => throw new NotSupportedException()
        };

        private long Score2(Stack<char> stack) => stack.Aggregate(0L, (acc, c) => c switch
        {
            '(' => acc * 5L + 1L,
            '[' => acc * 5L + 2L,
            '{' => acc * 5L + 3L,
            '<' => acc * 5L + 4L,
            _ => throw new NotSupportedException()
        });

        private Result Validate(string list, Stack<char> stack)
        {
            foreach (char c in list)
            {
                switch (AddChar(c, stack))
                {
                    case Fail fail:
                        return fail;
                    default:
                        break;
                }
            }

            return new Success();
        }

        private Result AddChar(char c, Stack<char> stack)
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
                    var (l, r) = GetMatchingPair(c);
                    if (stack.Peek() == l)
                    {
                        stack.Pop();
                        return new Success();
                    }
                    else
                    {
                        return new Fail(c);
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        private (char, char) GetMatchingPair(char c) => c switch
        {
            '(' or ')' => ('(', ')'),
            '[' or ']' => ('[', ']'),
            '{' or '}' => ('{', '}'),
            '<' or '>' => ('<', '>'),
            _ => throw new NotSupportedException()
        };

        private record Result(bool success);
        private record Success() : Result(true);
        private record Fail(char Char) : Result(false);
    }
}
