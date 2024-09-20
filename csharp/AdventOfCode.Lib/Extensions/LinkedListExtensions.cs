namespace AdventOfCode.Lib;

public static class LinkedListExtensions
{
    public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
    {
        if (current.List is null)
            throw new ArgumentException("Node is unlinked");

        return current.Next ?? current.List.First!;
    }

    public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
    {
        if (current.List is null)
            throw new ArgumentException("Node is unlinked");

        return current.Previous ?? current.List!.Last!;
    }
}