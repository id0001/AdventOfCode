using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.Collections.Trees
{
	public interface IGeneralTree<TNode, TKey, TValue> where TNode : ITreeNode<TKey,TValue>
	{
		IReadOnlySet<TNode> Children { get; }

		void AddChild(TNode node);

		void RemoveChild(TKey key);
	}
}
