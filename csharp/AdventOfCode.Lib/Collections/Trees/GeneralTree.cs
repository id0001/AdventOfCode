namespace AdventOfCode.Lib.Collections.Trees;

public class GeneralTree<T>
{
    private GeneralTreeNode<T>? _root;

    public GeneralTreeNode<T>? Root
    {
        get => _root;
        set
        {
            if (value != null)
                MakeRoot(value);
            _root = value;
        }
    }

    public IEnumerable<GeneralTreeNode<T>> PreOrder()
    {
        if (Root == null)
            yield break;

        var stack = new Stack<GeneralTreeNode<T>>();
        stack.Push(Root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            yield return current;

            foreach (var child in current.Children.Reverse())
                stack.Push(child);
        }
    }

    private static void MakeRoot(GeneralTreeNode<T> node)
    {
        if (node.Parent == null)
            return;

        var parent = node.Parent;
        parent.RemoveChild(node);
        MakeRoot(parent);
        node.AddChild(parent);
    }
}