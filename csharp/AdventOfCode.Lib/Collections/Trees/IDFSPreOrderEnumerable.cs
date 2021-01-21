using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections.Trees
{
	public interface IDFSPreOrderEnumerable<TNode, TKey, TValue> : IEnumerable<TNode> where TNode : ITreeNode<TKey, TValue>
	{
	}
}
