using AdventOfCode.Lib.Collections.Trees;

namespace AdventOfCode.Lib;

public static class GeneralTreeNodeExtensions
{
    public static void MakeRoot<T>(this GeneralTreeNode<T> source)
    {
        if (source.Parent == null)
            return;

        var parent = source.Parent;
        parent.RemoveChild(source);
        parent.MakeRoot();
        source.AddChild(parent);
    }

    public static IEnumerable<GeneralTreeNode<T>> EnumeratePreOrder<T>(this GeneralTreeNode<T> node)
    {
        var stack = new Stack<GeneralTreeNode<T>>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            yield return current;

            foreach (var child in current.Children.Reverse())
                stack.Push(child);
        }
    }
}