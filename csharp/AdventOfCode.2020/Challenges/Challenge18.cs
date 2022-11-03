using AdventOfCode.Lib;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(18)]
public class Challenge18
{
    private readonly IInputReader _inputReader;

    public Challenge18(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async() =>
        await _inputReader.ReadLinesAsync(18).SumAsync(line => EvaluateExpression(line)).ToStringAsync();

    [Part2]
    public async Task<string?> Part2Async() => await _inputReader.ReadLinesAsync(18)
        .SumAsync(line => EvaluateExpression(line, new HashSet<char> { '*' })).ToStringAsync();

    private static long EvaluateExpression(string expr, ICollection<char>? lowestPrecedence = null)
    {
        expr = expr.Trim();

        var bracketCounter = 0;
        var operatorIndex = -1;

        // Search the expr for an operator.
        for (var i = expr.Length - 1; i >= 0; i--)
        {
            var c = expr[i];

            switch (c)
            {
                case '(':
                    bracketCounter--;
                    break;
                case ')':
                    bracketCounter++;
                    break;
                case '+' when bracketCounter == 0:
                    operatorIndex = i;
                    break;
                case '*' when bracketCounter == 0:
                    operatorIndex = i;
                    break;
            }

            if (operatorIndex >= 0 && (lowestPrecedence == null || lowestPrecedence.Contains(expr[operatorIndex])))
                break;
        }

        // Parse expr as value if it does not contain an operator.
        if (operatorIndex >= 0)
            return expr[operatorIndex] switch
            {
                '+' => EvaluateExpression(expr[..operatorIndex], lowestPrecedence) +
                       EvaluateExpression(expr[(operatorIndex + 1)..], lowestPrecedence),
                '*' => EvaluateExpression(expr[..operatorIndex], lowestPrecedence) *
                       EvaluateExpression(expr[(operatorIndex + 1)..], lowestPrecedence),
                _ => throw new NotSupportedException()
            };

        // Evaluate expressions around operators and execute on the values.
        if (expr.StartsWith('(') && expr.EndsWith(')'))
            return EvaluateExpression(expr.Substring(1, expr.Length - 2), lowestPrecedence);

        return long.Parse(expr);
    }
}