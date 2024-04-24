using AdventOfCode.Core;

namespace AdventOfCode2016.Challenges;

[Challenge(19)]
public class Challenge19()
{
    private const int Input = 3005290;

    [Part1]
    public string Part1()
    {
        int count = Input;

        var start = new Node(1);
        var current = start;

        for (var i = 1; i < Input; i++)
        {
            var node = new Node(i + 1) { Previous = current };
            current.Next = node;
            current = node;
        }

        current.Next = start;
        start.Previous = current;

        current = start;
        while (count > 1)
        {
            var target = current.Next;

            target.Previous.Next = target.Next;
            target.Next.Previous = target.Previous;

            current = current.Next;
            count--;
        }

        return current.Id.ToString();
    }

    [Part2]
    public string Part2()
    {
        var count = Input;
        var start = new Node(1);
        var current = start;
        Node target = null!;

        for (var i = 1; i < Input; i++)
        {
            var node = new Node(i + 1) { Previous = current };
            current.Next = node;
            current = node;

            if (i == count / 2)
                target = current;
        }

        current.Next = start;
        start.Previous = current;

        while (count > 1)
        {
            target.Previous.Next = target.Next;
            target.Next.Previous = target.Previous;
            target = count % 2 == 1 ? target.Next.Next : target.Next;

            current = current.Next;
            count--;
        }

        return current!.Id.ToString();
    }

    private class Node(int id)
    {
        public int Id => id;

        public Node Previous { get; set; } = null!;
        public Node Next { get; set; } = null!;
    }
}