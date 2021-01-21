using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections.Trees
{
    public interface IDFSPreOrderEnumerable<TNode, TValue> where TNode : ITreeNode<TNode, TValue>
    {
        IEnumerable<TNode> EnumerateDFSPreOrder();
    }
}
