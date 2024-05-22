namespace AdventOfCode.Lib.Collections.Trees;

public class GeneralTreeNode<T>(T value)
{
    private readonly HashSet<GeneralTreeNode<T>> _children = new();

    public GeneralTreeNode<T>? Parent { get; private set; }

    public T Value { get; } = value;

    public int Depth => Parent?.Depth + 1 ?? 0;

    public int ChildCount => _children.Count;

    public IEnumerable<GeneralTreeNode<T>> Children => _children;

    public IEnumerable<GeneralTreeNode<T>> Siblings
    {
        get
        {
            if (Parent == null)
                yield break;

            foreach (var child in Parent.Children.Where(c => c != this))
                yield return child;
        }
    }

    public bool AddChild(T value) => AddChild(new GeneralTreeNode<T>(value));

    public bool AddChild(GeneralTreeNode<T> node)
    {
        if (node.Parent == this)
            return false;

        node.Parent?.RemoveChild(node);

        node.Parent = this;
        return _children.Add(node);
    }

    public bool RemoveChild(T value) => RemoveChild(new GeneralTreeNode<T>(value) { Parent = this });

    public bool RemoveChild(GeneralTreeNode<T> node)
    {
        if (node.Parent != this)
            return false;

        node.Parent = null;
        return _children.Remove(node);
    }
}