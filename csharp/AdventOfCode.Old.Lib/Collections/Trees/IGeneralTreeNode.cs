using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections.Trees
{
    public interface IGeneralTreeNode<TNode, TValue> where TNode : ITreeNode<TNode, TValue>
    {
        IReadOnlySet<TNode> Children { get; }

        bool AddChild(TValue value);

        bool AddChild(TNode node);

        bool RemoveChild(TValue value);

        bool RemoveChild(TNode node);
    }
}
