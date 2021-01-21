using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.Collections.Trees
{
	public interface ITreeNode<TKey, TValue> : IEquatable<ITreeNode<TKey, TValue>>
	{
		TKey Key { get; }

		TValue Value { get; }

		ITreeNode<TKey, TValue> Parent { get; }

		int Depth { get; }
	}
}
