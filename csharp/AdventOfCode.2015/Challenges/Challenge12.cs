using System.Text.Json;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(12)]
public class Challenge12
{
    private readonly IInputReader _inputReader;

    public Challenge12(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var json = await _inputReader.ReadAllTextAsync(12);
        var doc = JsonDocument.Parse(json);

        var sum = 0;
        var stack = new Stack<JsonElement>(new[] { doc.RootElement });
        while (stack.Count > 0)
        {
            var current = stack.Pop();

            switch (current.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var child in current.EnumerateObject())
                    {
                        stack.Push(child.Value);
                    }

                    break;
                case JsonValueKind.Array:
                    foreach (var child in current.EnumerateArray())
                    {
                        stack.Push(child);
                    }
                    break;
                case JsonValueKind.Number:
                    sum += current.GetInt32();
                    break;
                case JsonValueKind.Undefined:
                case JsonValueKind.String:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var json = await _inputReader.ReadAllTextAsync(12);
        var doc = JsonDocument.Parse(json);
        
        var queue = new Queue<JsonElement>();
        queue.Enqueue(doc.RootElement);

        var sum = 0;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            switch (current.ValueKind)
            {
                case JsonValueKind.Object:
                    var children = current.EnumerateObject();
                    if (children.Any(c => c.Value.ValueKind == JsonValueKind.String && c.Value.GetString() == "red"))
                        break;
                    
                    foreach(var child in children)
                        queue.Enqueue(child.Value);
                    
                    break;
                case JsonValueKind.Array:
                    foreach(var child in current.EnumerateArray())
                        queue.Enqueue(child);
                    break;
                case JsonValueKind.Number:
                    sum += current.GetInt32();
                    break;
                case JsonValueKind.Undefined:
                case JsonValueKind.String:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return sum.ToString();
    }
}