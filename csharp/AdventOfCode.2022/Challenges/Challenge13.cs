using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var sumInOrder = 0;
        var n = 1;
        await foreach (var (left, right) in ParseInputAsync(13))
        {
            if (CompareLine(left, right) == State.RightOrder)
                sumInOrder += n;

            n++;
        }

        return sumInOrder.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var list = new List<string>();
        await foreach (var (left, right) in ParseInputAsync(13))
        {
            list.Add(left);
            list.Add(right);
        }

        list.Add("[2]");
        list.Add("[6]");

        var cmp = new PacketComparer();

        var ordered = list.OrderBy(x => x, cmp).ToList();
        var p1 = ordered.IndexOf("[2]") + 1;
        var p2 = ordered.IndexOf("[6]") + 1;

        return (p1 * p2).ToString();
    }

    private async IAsyncEnumerable<(string, string)> ParseInputAsync(int challenge)
    {
        var parts = new string[2];
        var i = 0;
        await foreach (var line in InputReader.ReadLinesAsync(challenge))
        {
            if (string.IsNullOrEmpty(line))
            {
                yield return (parts[0], parts[1]);
                continue;
            }

            parts[i] = line;
            i = Euclid.Modulus(i + 1, 2);
        }

        yield return (parts[0], parts[1]);
    }

    private static State CompareLine(string left, string right)
    {
        // Compare if both are numbers
        if (IsNumber(left, out var ln) && IsNumber(right, out var rn))
        {
            if (ln == rn)
                return State.None;

            return ln < rn ? State.RightOrder : State.WrongOrder;
        }

        // Convert both to a list
        left = IsNumber(left, out _) ? $"[{left}]" : left;
        right = IsNumber(right, out _) ? $"[{right}]" : right;

        // Extract children and recursively compare each item
        var leftList = ExtractFromList(left).ToList();
        var rightList = ExtractFromList(right).ToList();

        for (var i = 0; i < Math.Min(leftList.Count, rightList.Count); i++)
        {
            var substate = CompareLine(leftList[i], rightList[i]);
            if (substate == State.None)
                continue;

            return substate;
        }

        // Check array length when the loop has run out of items
        if (leftList.Count == rightList.Count)
            return State.None;

        return leftList.Count < rightList.Count ? State.RightOrder : State.WrongOrder;
    }

    private static bool IsNumber(string input, out int number)
    {
        return int.TryParse(input, out number);
    }

    private static IEnumerable<string> ExtractFromList(string listInput)
    {
        var sb = new StringBuilder();
        var listDepth = 0;
        for (var i = 1; i < listInput.Length - 1; i++)
        {
            if (listInput[i] == ',' && listDepth == 0)
            {
                yield return sb.ToString();
                sb.Clear();
                continue;
            }

            sb.Append(listInput[i]);

            if (listInput[i] == '[')
            {
                listDepth++;
                continue;
            }

            if (listInput[i] == ']') listDepth--;
        }

        if (sb.Length > 0)
            yield return sb.ToString();
    }

    private enum State
    {
        None,
        RightOrder,
        WrongOrder
    }

    private class PacketComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            return CompareLine(x!, y!) switch
            {
                State.None => 0,
                State.RightOrder => -1,
                State.WrongOrder => 1,
                _ => throw new NotImplementedException()
            };
        }
    }
}