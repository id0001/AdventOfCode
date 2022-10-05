using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2015.Challenges;

[Challenge(11)]
public class Challenge11
{
    private const string OriginalInput = "hepxcrrq";

    [Part1]
    public string Part1()
    {
        var input = OriginalInput;
        do
        {
            input = Increase(input);
        } while (!(Validate3AscendingCharactersRule(input) && ValidateNoIloRule(input) && Validate2DifferentPairsRule(input)));

        return input;
    }
    
    [Part2]
    public string Part2()
    {
        var input = Part1();
        do
        {
            input = Increase(input);
        } while (!(Validate3AscendingCharactersRule(input) && ValidateNoIloRule(input) && Validate2DifferentPairsRule(input)));

        return input;
    }

    private static bool Validate3AscendingCharactersRule(string input)
    {
        for (var i = 0; i < input.Length - 2; i++)
        {
            if (input[i + 2] - input[i + 1] == 1 && input[i + 1] - input[i] == 1)
                return true;
        }

        return false;
    }

    private static bool ValidateNoIloRule(string input) =>
        !(input.Contains('i') || input.Contains('l') || input.Contains('o'));
    private static bool Validate2DifferentPairsRule(string input)
    {
        var firstType = '-';
        for (var i = 0; i < input.Length - 1; i++)
        {
            if (input[i] == input[i + 1] && (i + 2 == input.Length || input[i] != input[+2]))
            {
                firstType = input[i];
            }
        }

        if (firstType == '-')
            return false;
        
        for (var i = 0; i < input.Length - 1; i++)
        {
            if (input[i] != firstType && input[i] == input[i + 1] && (i + 2 == input.Length || input[i] != input[+2]))
            {
                return true;
            }
        }

        return false;
    }

    private static string Increase(string input)
    {
        var sb = new StringBuilder();
        var stack = new Stack<char>();
        for (var n = input.Length-1; n >= 0; n--)
        {
            var newChar = NextChar(input[n]);
            stack.Push(newChar);
            if (newChar == 'a') continue;
            
            sb.Append(input[0..n]);
            break;
        }
        
        while (stack.Any())
        {
            sb.Append(stack.Pop());
        }

        return sb.ToString();
    }

    private static char NextChar(char input) => (char)('a' + (input - 'a' + 1) % 26);
}