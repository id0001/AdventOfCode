using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Collections.Trees;
using AdventOfCode.Lib.Extensions;
using JetBrains.Annotations;
using System.Xml.Linq;

namespace AdventOfCode2018.Challenges;

[Challenge(8)]
public class Challenge08(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadLineAsync<int>(8, ' ').ToArrayAsync();

        var (root, _) = CreateTreeNode(input, 0);
        return root.EnumeratePreOrder().Sum(x => x.Value.Sum()).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadLineAsync<int>(8, ' ').ToArrayAsync();

        var (root, _) = CreateTreeNode(input, 0);

        var stack = new Stack<GeneralTreeNode<int[]>>();
        stack.Push(root);

        int sum = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current.ChildCount == 0)
            {
                sum += current.Value.Sum();
                continue;
            }

            var children = current.Children.ToList();
            foreach (var i in current.Value)
            {
                if (i - 1 < children.Count && i - 1 >= 0)
                    stack.Push(children[i - 1]);
            }
        }

        return sum.ToString();
    }

    private (GeneralTreeNode<int[]>, int) CreateTreeNode(int[] input, int left)
    {
        var childCount = input[left];
        var lengthMetadata = input[left + 1];


        var children = new List<GeneralTreeNode<int[]>>();

        var totalChildrenLength = 0;
        left += 2; // start of first child
        for (var i = 0; i < childCount; i++)
        {
            var (child, len) = CreateTreeNode(input, left + totalChildrenLength);
            children.Add(child);
            totalChildrenLength += len;
        }

        left += totalChildrenLength;
        var node = new GeneralTreeNode<int[]>(input[left..(left + lengthMetadata)]);

        foreach (var child in children)
            node.AddChild(child);

        return (node, totalChildrenLength + 2 + lengthMetadata);
    }
}