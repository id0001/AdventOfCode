namespace AdventOfCode.Lib.Collections.Trees
{
    public interface ITreeNode<TNode, TValue> where TNode : ITreeNode<TNode, TValue>
    {
        TValue Value { get; }

        TNode Parent { get; }

        int Depth { get; }
    }
}
