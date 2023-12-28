using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode2020.Challenges;

[Challenge(23)]
public class Challenge23
{
    private readonly int[] _input = {2, 1, 9, 3, 4, 7, 8, 6, 5};

    [Part1]
    public string Part1()
    {
        var list = new LinkedList<int>(_input);

        var current = list.First;

        for (var move = 0; move < 100; move++)
        {
            var nextNodes = RemoveItemsAfter(current!, 3);

            var lowest = list.Min();
            var highest = list.Max();
            LinkedListNode<int>? destination = null;
            for (var n = current!.Value - 1; n >= lowest; n--)
            {
                destination = list.Find(n);
                if (destination != null)
                    break;
            }

            destination ??= list.Find(highest);

            foreach (var nextNode in nextNodes)
            {
                list.AddAfter(destination!, nextNode);
                destination = nextNode;
            }

            current = current.Next ?? list.First;
        }

        var node = list.Find(1)!.Next;
        var sb = new StringBuilder();
        do
        {
            sb.Append(node!.Value);
            node = node.Next ?? list.First;
        } while (node!.Value != 1);

        return sb.ToString();
    }

    [Part2]
    public string Part2()
    {
        var max = _input.Max();
        var rest = Enumerable.Range(max + 1, 1000000 - _input.Length).ToArray();
        var list = new LinkedList<int>(_input.Concat(rest));

        var current = list.First;

        // Optimize node loopup by creating a lookup table from value to linkedlist node.
        var dict = new Dictionary<int, LinkedListNode<int>>();
        var node = list.First;
        do
        {
            dict.Add(node!.Value, node);
            node = node.Next;
        } while (node != null);

        for (var i = 0; i < 10000000; i++)
        {
            // Pick 3 and remove from list
            var p1 = current!.Next ?? list.First;
            var p2 = p1!.Next ?? list.First;
            var p3 = p2!.Next ?? list.First;

            list.Remove(p1);
            list.Remove(p2);
            list.Remove(p3!);

            // Determine insert position
            var set = new[] {p1.Value, p2.Value, p3!.Value};

            var n = 1000000;
            if (current.Value - 1 >= 1 && !set.Contains(current.Value - 1))
                n = current.Value - 1;
            else if (current.Value - 2 >= 1 && !set.Contains(current.Value - 2))
                n = current.Value - 2;
            else if (current.Value - 3 >= 1 && !set.Contains(current.Value - 3)) n = current.Value - 3;


            list.AddAfter(dict[n], p1);
            list.AddAfter(p1, p2);
            list.AddAfter(p2, p3);

            current = current.Next ?? list.First;
        }

        var node1 = dict[1].Next;
        var node2 = node1!.Next;

        return ((long) node1.Value * node2!.Value).ToString();
    }

    private static IEnumerable<LinkedListNode<int>> RemoveItemsAfter(LinkedListNode<int> node, int amount)
    {
        var list = new List<LinkedListNode<int>>();
        for (var i = 0; i < amount; i++)
        {
            var next = node.Next ?? node.List!.First; // get next or first (cyclic)

            if (next == null) // empty list
                break;

            list.Add(next); // add node to list
            node = next;
        }

        list.ForEach(n => n.List!.Remove(n));

        return list;
    }
}