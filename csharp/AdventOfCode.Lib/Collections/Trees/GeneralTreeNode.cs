using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Collections.Trees
{
	public class GeneralTreeNode<TKey, TValue> :
		ITreeNode<TKey, TValue>,
		IGeneralTree<GeneralTreeNode<TKey, TValue>, TKey, TValue>,
		IDFSPreOrderEnumerable<GeneralTreeNode<TKey, TValue>, TKey, TValue>
	{
		private readonly IDictionary<TKey, GeneralTreeNode<TKey, TValue>> children = new Dictionary<TKey, GeneralTreeNode<TKey, TValue>>();

		public GeneralTreeNode(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public TKey Key { get; }

		public TValue Value { get; }

		public GeneralTreeNode<TKey, TValue> Parent { get; private set; }

		public IReadOnlySet<GeneralTreeNode<TKey, TValue>> Children => children.Values.ToHashSet();

		ITreeNode<TKey, TValue> ITreeNode<TKey, TValue>.Parent => Parent;

		public int Depth { get; private set; }

		public void AddChild(TKey key, TValue value) => AddChild(new GeneralTreeNode<TKey, TValue>(key, value));

		public void AddChild(GeneralTreeNode<TKey, TValue> node)
		{
			if (node.Parent != null)
				throw new ArgumentException("Node is already part of a tree.");

			node.Parent = this;
			node.UpdateDepth();
			children.Add(node.Key, node);
		}

		public void RemoveChild(TKey key)
		{
			if (children.ContainsKey(key))
			{
				var child = children[key];
				children.Remove(key);
				child.Parent = null;
				child.UpdateDepth();
			}
		}

		public IEnumerable<GeneralTreeNode<TKey, TValue>> EnumeratePreOrder()
		{
			yield return this;

			foreach (var child in Children)
			{
				foreach (var item in child.EnumeratePreOrder())
				{
					yield return item;
				}
			}
		}

		public bool Equals(ITreeNode<TKey, TValue> other) => other != null && EqualityComparer<TKey>.Default.Equals(Key, other.Key);

		private void UpdateDepth()
		{
			Depth = (Parent?.Depth ?? 0) + 1;
			foreach (var child in Children)
			{
				child.UpdateDepth();
			}
		}

		IEnumerator<GeneralTreeNode<TKey, TValue>> IEnumerable<GeneralTreeNode<TKey, TValue>>.GetEnumerator() => EnumeratePreOrder().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => EnumeratePreOrder().GetEnumerator();
	}
}
