using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections.Trees;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(6)]
	public class Challenge06
	{
		private readonly IInputReader inputReader;
		private GeneralTree<string, string> tree;

		public Challenge06(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			var nodeDict = new Dictionary<string, GeneralTreeNode<string, string>>();

			tree = new GeneralTree<string, string>();

			await foreach(var line in inputReader.ReadLinesAsync(6))
			{
				var parent = line.Substring(0, line.IndexOf(")"));
				var child = line.Substring(line.IndexOf(")") + 1);

				if (!nodeDict.ContainsKey(parent))
					nodeDict.Add(parent, new GeneralTreeNode<string, string>(parent, parent));

				if (!nodeDict.ContainsKey(child))
					nodeDict.Add(child, new GeneralTreeNode<string, string>(child, child));

				nodeDict[parent].AddChild(nodeDict[child]);
			}

			tree.Root = nodeDict["COM"];
		}

		[Part1]
		public string Part1()
		{
			return tree.EnumeratePreOrder().Sum(n => n.Depth).ToString();
		}

		[Part2]
		public string Part2()
		{
			return null;
		}
	}
}
