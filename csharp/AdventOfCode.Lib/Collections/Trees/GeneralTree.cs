using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections.Trees
{
	public class GeneralTree<TKey, TValue> : ITree<GeneralTreeNode<TKey, TValue>, TKey, TValue>, IDFSPreOrderEnumerable<GeneralTreeNode<TKey, TValue>, TKey, TValue>
	{
		private GeneralTreeNode<TKey, TValue> root;

		public GeneralTreeNode<TKey, TValue> Root
		{
			get => root;
			set
			{
				MakeRoot(value);
				root = value;
			}
		}
		public IEnumerable<GeneralTreeNode<TKey, TValue>> EnumeratePreOrder()
		{
			if (Root == null)
				yield break;

			foreach (var item in Root.EnumeratePreOrder())
				yield return item;
		}

		private void MakeRoot(GeneralTreeNode<TKey, TValue> node)
		{
			if (node.Parent == null)
				return;

			var parent = node.Parent;
			parent.RemoveChild(node.Key);
			MakeRoot(parent);
			node.AddChild(parent);
		}

		IEnumerator<GeneralTreeNode<TKey, TValue>> IEnumerable<GeneralTreeNode<TKey, TValue>>.GetEnumerator() => EnumeratePreOrder().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => EnumeratePreOrder().GetEnumerator();



	}

	public class MyTree<TKey, TNode>
	{
		private Func<TNode, TKey> keySelector;
		private IDictionary<TKey, TNode> nodes;
		private IDictionary<TKey, TKey> parents;
		private IDictionary<TKey, ISet<TKey>> children;

		private TKey root;

		public MyTree(Func<TNode, TKey> keySelector)
		{
			this.keySelector = keySelector;
			this.nodes = new Dictionary<TKey, TNode>();
		}

		public void SetRoot(TKey key)
		{
			root = key;
			// TODO root fuckery
		}

		public void AddNode(TKey parent, TNode value)
		{
			var key = keySelector(value);
			if (nodes.ContainsKey(key))
				return;

			nodes.Add(key, value);
			parents.Add(key, parent);
			children.Add(key, new HashSet<TKey>());
		}

		public void RemoveNode(TKey key)
		{
			// Remove from parent
			RemoveFromParent(key);

			// Remove children
			foreach (var child in children[key])
			{
				RemoveNode(child);
			}

			// Remove node
			nodes.Remove(key);

		}

		public IEnumerable<TNode> EnumerateDFSPreOrder() => EnumerateDFSPreOrderInternal(root);

		public IEnumerable<TNode> EnumerateDFSPreOrderInternal(TKey node)
		{
			yield return nodes[node];

			foreach (var child in children[node])
				foreach (var item in EnumerateDFSPreOrderInternal(child))
					yield return item;
		}

		private void MakeRoot(TKey key)
		{
			if (!parents.TryGetValue(key, out var parent))
				return;

			RemoveFromParent(key);
			MakeRoot(parent);
			AddChild(key, parent);
		}

		private void RemoveFromParent(TKey key)
		{
			if (parents.TryGetValue(key, out var parent))
			{
				children[parent].Remove(key);
				parents.Remove(key);
			}
		}

		private void AddChild(TKey parent, TKey child)
		{
			parents[child]
		}
		
		private void SetParent(TKey child, TKey parent)
		{
			if (parents.ContainsKey(child))
				RemoveFromParent(child);

			parents.Add(child, parent);
		}
	}
}
