using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(7)]
	public class Challenge07
	{
		private readonly IInputReader inputReader;
		private BidirectionalGraph<string, WeightedEdge> bagGraph;

		public Challenge07(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			bagGraph = new BidirectionalGraph<string, WeightedEdge>();

			await foreach (string line in inputReader.ReadLinesAsync(7))
			{
				Match m;
				string[] split = line.Split("contain");
				m = Regex.Match(split[0].Trim(), @"([a-z]+ [a-z]+) bags");

				string parentBag = m.Groups[1].Value;
				bagGraph.AddVertex(parentBag);

				string[] contents = split[1].Split(",");
				foreach (var content in contents)
				{
					m = Regex.Match(content, @"(\d+) ([a-z]+ [a-z]+) bags?");
					if (m.Success)
					{
						int amount = int.Parse(m.Groups[1].Value);
						string childBag = m.Groups[2].Value;

						bagGraph.AddVertex(childBag);
						bagGraph.AddEdge(new WeightedEdge(parentBag, childBag, amount));
					}
				}
			}
		}

		[Part1]
		public string Part1()
		{
			int count = 0;
			ISet<string> closedSet = new HashSet<string>();
			Stack<WeightedEdge> openSet = new Stack<WeightedEdge>(bagGraph.InEdges("shiny gold"));

			while (openSet.Count > 0)
			{
				WeightedEdge edge = openSet.Pop();

				if (!closedSet.Contains(edge.Source))
				{
					closedSet.Add(edge.Source);
					count++;

					foreach (var child in bagGraph.InEdges(edge.Source))
					{
						if (!closedSet.Contains(child.Source))
							openSet.Push(child);
					}
				}
			}

			return count.ToString();
		}

		[Part2]
		public string Part2()
		{
			int count = CountBags("shiny gold");
			return count.ToString();
		}

		private int CountBags(string vertex)
		{
			int count = 0;

			var outEdges = bagGraph.OutEdges(vertex);
			foreach(var edge in outEdges)
			{
				count += edge.Amount + (edge.Amount * CountBags(edge.Target));
			}

			return count;
		}

		private class WeightedEdge : Edge<string>
		{
			public WeightedEdge(string source, string target, int amount)
				: base(source, target)
			{
				Amount = amount;
			}

			public int Amount { get; }
		}
	}
}
