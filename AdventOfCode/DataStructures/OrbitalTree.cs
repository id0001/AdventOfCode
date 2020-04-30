using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.DataStructures
{
	internal class OrbitalTree
	{
		public OrbitalTree(string[] data)
		{
			var stack = new Stack<Node>();

			Root = new Node("COM");
			stack.Push(Root);

			while (stack.Count > 0)
			{
				var parent = stack.Pop();

				var relations = data.Where(e => e.Split(")")[0] == parent.Id).ToList();

				foreach (var rel in relations)
				{
					string[] split = rel.Split(")");
					var child = new Node(split[1]);
					parent.AddChild(child);
					stack.Push(child);
				}
			}
		}

		public Node Root { get; }

		public int Sum(Func<Node, int> selector)
		{
			int result = 0;
			Stack<Node> stack = new Stack<Node>();
			stack.Push(Root);

			while (stack.Count > 0)
			{
				var node = stack.Pop();
				result += selector(node);

				foreach (var child in node.Children)
				{
					stack.Push(child);
				}
			}

			return result;
		}

		public Node FindNode(Func<Node, bool> selector)
		{
			Stack<Node> stack = new Stack<Node>();
			stack.Push(Root);

			while (stack.Count > 0)
			{
				var node = stack.Pop();
				if (selector(node))
				{
					return node;
				}

				foreach (var child in node.Children)
				{
					stack.Push(child);
				}
			}

			return null;
		}

		public int FindTransfers(string from, string to)
		{
			var fromNode = FindNode(n => n.Id == from).Parent;
			var toNode = FindNode(n => n.Id == to).Parent;

			int transfers = 0;

			Stack<Node> stack = new Stack<Node>();
			stack.Push(Root);

			if (fromNode == toNode) return 0;

			var fromPath = fromNode.GetPathToRoot();
			var toPath = toNode.GetPathToRoot();

			foreach (var node in fromPath)
			{
				if (toPath.Contains(node))
				{
					fromNode = node;
					break;
				}

				transfers++;
			}

			foreach (var node in toPath)
			{
				if (node == fromNode)
				{
					break;
				}

				transfers++;
			}

			return transfers;
		}

		public class Node
		{
			private readonly List<Node> _children = new List<Node>();

			public Node(string id)
			{
				Id = id;
			}

			public string Id { get; }

			public Node Parent { get; set; }

			public int Depth
			{
				get
				{
					int depth = 0;
					var p = Parent;
					while (p != null)
					{
						p = p.Parent;
						depth++;
					}

					return depth;
				}
			}

			public IEnumerable<Node> GetPathToRoot()
			{
				List<Node> nodes = new List<Node>();
				nodes.Add(this);
				var p = Parent;
				while (p != null)
				{
					nodes.Add(p);
					p = p.Parent;
				}

				return nodes;
			}

			public void AddChild(Node node)
			{
				if (!_children.Contains(node))
				{
					node.Parent = this;
					_children.Add(node);
				}
			}

			public void RemoveChild(Node node)
			{
				if (_children.Contains(node))
				{
					node.Parent = null;
					_children.Remove(node);
				}
			}

			public IEnumerable<Node> Children => _children;
		}
	}
}
