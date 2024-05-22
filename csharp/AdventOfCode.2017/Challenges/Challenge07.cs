using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections.Trees;

namespace AdventOfCode2017.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var tree = await CreateTreeAsync(7);
        return tree.Root!.Value.Name;
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var tree = await CreateTreeAsync(7);

        var fatNode = FindFatNode(tree.Root!);
        var difference = GetWeight(fatNode) - GetWeight(fatNode.Siblings.First());

        return (fatNode.Value.Weight - difference).ToString();
    }

    private async Task<GeneralTree<Node>> CreateTreeAsync(int day)
    {
        var relations = new Dictionary<string, string[]>();
        var nodes = new Dictionary<string, GeneralTreeNode<Node>>();

        await foreach (var line in inputReader.ReadLinesAsync(day))
        {
            var split = line.Extract(@"(.+) \((\d+)\)(?: -> (.+))?");
            nodes.Add(split.First(), new GeneralTreeNode<Node>(new Node(split.First(), split.Second().As<int>())));

            if (split.Length == 3)
                relations.Add(split.First(), split.Third().SplitBy(", "));
        }

        foreach (var items in relations)
        {
            var parent = nodes[items.Key];
            foreach (var child in items.Value)
                parent.AddChild(nodes[child]);
        }

        return new GeneralTree<Node>() { Root = nodes.Values.Single(x => x.Parent == null) };
    }

    private GeneralTreeNode<Node> FindFatNode(GeneralTreeNode<Node> root)
    {
        if (root.ChildCount == 0)
            return root;

        var groupByWeight = root.Children.GroupBy(GetWeight);
        if (groupByWeight.Count() == 1)
            return root;

        var fat = groupByWeight.Single(g => g.Count() == 1).Single();
        return FindFatNode(fat);
    }

    private int GetWeight(GeneralTreeNode<Node> node) => node.Value.Weight + node.Children.Sum(GetWeight);

    private sealed record Node(string Name, int Weight);
}