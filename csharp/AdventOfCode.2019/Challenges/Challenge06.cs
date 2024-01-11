using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Collections.Trees;

namespace AdventOfCode2019.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader InputReader)
{
    private readonly GeneralTree<string> _tree = new();

    [Setup]
    public async Task SetupAsync()
    {
        var nodeDict = new Dictionary<string, GeneralTreeNode<string>>();
        await foreach (var line in InputReader.ReadLinesAsync(6))
        {
            var parent = line[..line.IndexOf(")", StringComparison.Ordinal)];
            var child = line[(line.IndexOf(")", StringComparison.Ordinal) + 1)..];

            if (!nodeDict.ContainsKey(parent))
                nodeDict.Add(parent, new GeneralTreeNode<string>(parent));

            if (!nodeDict.ContainsKey(child))
                nodeDict.Add(child, new GeneralTreeNode<string>(child));

            nodeDict[parent].AddChild(nodeDict[child]);
        }

        _tree.Root = nodeDict["COM"];
    }

    [Part1]
    public string Part1()
    {
        return _tree.PreOrder().Sum(n => n.Depth).ToString();
    }

    [Part2]
    public string Part2()
    {
        var you = _tree.PreOrder().First(x => x.Value == "YOU");
        _tree.Root = you;

        return (_tree.PreOrder().First(x => x.Value == "SAN").Depth - 2).ToString();
    }
}