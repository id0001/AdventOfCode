namespace AdventOfCode.Lib.Collections.Trees
{
    public interface ITree<TNode, TValue> where TNode : ITreeNode<TNode, TValue>
    {
        TNode Root { get; set; }
    }
}
