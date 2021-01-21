
namespace AdventOfCode.Lib.Collections.Trees
{
	public interface ITree<TNode, TKey, TValue> where TNode : ITreeNode<TKey, TValue>
	{
		TNode Root { get; }
	}
}
