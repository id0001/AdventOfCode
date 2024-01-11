using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using Microsoft;

namespace AdventOfCode2022.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader InputReader)
{
    private static readonly Regex InputPattern = new(@"\$ (\w+) ?(.+)?");

    [Part1]
    public async Task<string> Part1Async()
    {
        var root = await ParseFileSystemAsync();

        var stack = new Stack<FsNode>();
        stack.Push(root);

        var sum = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current.Size <= 100000)
                sum += current.Size;

            foreach (var child in current.Children.Where(x => !x.IsFile))
                stack.Push(child);
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var root = await ParseFileSystemAsync();

        const int total = 70_000_000;
        var neededSpace = 30_000_000 - (total - root.Size);

        var stack = new Stack<FsNode>();
        stack.Push(root);

        var dirFlatList = new List<(string Name, int Size)>();

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            dirFlatList.Add((current.Name, current.Size));

            foreach (var child in current.Children.Where(x => !x.IsFile))
                stack.Push(child);
        }

        var toDelete = dirFlatList.OrderBy(x => x.Size).SkipWhile(x => x.Size < neededSpace).First();

        return toDelete.Size.ToString();
    }

    private async Task<FsNode> ParseFileSystemAsync()
    {
        var root = FsNode.CreateDirectory("/");
        var dirStack = new Stack<FsNode>(new[] {root});
        await foreach (var line in InputReader.ReadLinesAsync(7))
            if (line.StartsWith("$"))
            {
                var match = InputPattern.Match(line);
                var command = match.Groups[1].Value;
                var parameter = match.Groups.Count == 3 ? match.Groups[2].Value : string.Empty;

                switch (command)
                {
                    case "ls":
                        break;
                    case "cd":
                        switch (parameter)
                        {
                            case "/":
                                dirStack.Clear();
                                dirStack.Push(root);
                                break;
                            case "..":
                                dirStack.Pop();
                                break;
                            default:
                                var node = dirStack.Peek().Children.FirstOrDefault(n => n.Name == parameter);
                                Assumes.Present(node);
                                dirStack.Push(node);
                                break;
                        }

                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                var currentDir = dirStack.Peek();
                if (line.StartsWith("dir"))
                {
                    var split = line.Split(' ');
                    currentDir.AddDirectory(split[1]);
                }
                else
                {
                    var split = line.Split(' ');
                    currentDir.AddFile(split[1], int.Parse(split[0]));
                }
            }

        return root;
    }

    private class FsNode
    {
        private readonly List<FsNode> _children = new();
        private readonly int _size;

        private FsNode(string name, int size, bool isFile)
        {
            Name = name;
            IsFile = isFile;
            _size = size;
        }

        public int Size => IsFile ? _size : Children.Sum(x => x.Size);
        public string Name { get; }
        public bool IsFile { get; }

        public IEnumerable<FsNode> Children => _children;

        public static FsNode CreateDirectory(string name)
        {
            return new FsNode(name, 0, false);
        }

        private static FsNode CreateFile(string name, int size)
        {
            return new FsNode(name, size, true);
        }

        public void AddFile(string name, int size)
        {
            _children.Add(CreateFile(name, size));
        }

        public void AddDirectory(string name)
        {
            _children.Add(CreateDirectory(name));
        }
    }
}