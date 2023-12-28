namespace AdventOfCode.Lib.Collections.Trees;

public class GeneralTreeNode<T>(T value) : IEquatable<GeneralTreeNode<T>>
{
    private readonly HashSet<GeneralTreeNode<T>> _children = new();

    public GeneralTreeNode<T>? Parent { get; set; }

    public T Value { get; } = value;

    public int Depth => Parent?.Depth + 1 ?? 0;

    public IReadOnlySet<GeneralTreeNode<T>> Children => _children;

    public bool Equals(GeneralTreeNode<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(Value, other.Value);
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

    public bool RemoveChild(T value) => RemoveChild(new GeneralTreeNode<T>(value) {Parent = this});

    public bool RemoveChild(GeneralTreeNode<T> node)
    {
        if (node.Parent != this)
            return false;

        node.Parent = null;
        return _children.Remove(node);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((GeneralTreeNode<T>) obj);
    }

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(GeneralTreeNode<T>? left, GeneralTreeNode<T>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        return !ReferenceEquals(null, left) && left.Equals(right);
    }

    public static bool operator !=(GeneralTreeNode<T>? left, GeneralTreeNode<T>? right) => !(left == right);
}