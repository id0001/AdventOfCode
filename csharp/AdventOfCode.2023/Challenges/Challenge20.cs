using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var modules = await inputReader.ParseLinesAsync(20, ParseLine).ToDictionaryAsync(kv => kv.Name);
        var flipFlops = modules.Values.Where(m => m.Type == "%").ToDictionary(kv => kv.Name, _ => false);
        var conjunctions = modules.Values.Where(m => m.Type == "&")
            .ToDictionary(kv => kv.Name,
                kv => modules.Values.Where(m => m.Outputs.Contains(kv.Name)).ToDictionary(kv2 => kv2.Name, _ => false));

        var rxPresses = conjunctions["bn"].ToDictionary(kv => kv.Key, _ => 0L);

        var lows = 0L;
        var highs = 0L;
        var presses = 0L;
        while (presses < 1000L)
            Process(modules, flipFlops, conjunctions, rxPresses, ref presses, ref lows, ref highs);

        return (lows * highs).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var modules = await inputReader.ParseLinesAsync(20, ParseLine).ToDictionaryAsync(kv => kv.Name);
        var flipFlops = modules.Values.Where(m => m.Type == "%").ToDictionary(kv => kv.Name, _ => false);
        var conjunctions = modules.Values.Where(m => m.Type == "&")
            .ToDictionary(kv => kv.Name,
                kv => modules.Values.Where(m => m.Outputs.Contains(kv.Name)).ToDictionary(kv2 => kv2.Name, _ => false));

        var rxPresses = conjunctions["bn"].ToDictionary(kv => kv.Key, _ => 0L);

        var i = 0L;
        var lows = 0L;
        var highs = 0L;
        while (true)
        {
            Process(modules, flipFlops, conjunctions, rxPresses, ref i, ref lows, ref highs);
            if (rxPresses.Values.All(s => s > 0))
                return rxPresses.Values.Product().ToString();
        }
    }

    private void Process(
        Dictionary<string, Module> modules,
        Dictionary<string, bool> flipflops,
        Dictionary<string, Dictionary<string, bool>> conjunctions,
        Dictionary<string, long> rxPresses,
        ref long presses,
        ref long lows,
        ref long highs)
    {
        presses++;

        var queue = new Queue<Signal>();

        lows++;
        foreach (var d in modules["broadcaster"].Outputs)
            queue.Enqueue(new Signal("broadcaster", d, false));

        while (queue.Count > 0)
        {
            var (source, dest, pulseIn) = queue.Dequeue();
            if (pulseIn)
                highs++;
            else
                lows++;

            if (!modules.ContainsKey(dest))
                continue;

            var (type, _, output) = modules[dest];
            var pulseOut = (type, pulseIn) switch
            {
                ("broadcaster", _) => pulseIn,
                ("%", false) => flipflops[dest] = !flipflops[dest],
                ("&", _) => Conjuction(conjunctions, source, dest, pulseIn),
                _ => (bool?) null
            };

            if (!pulseOut.HasValue)
                continue;

            if (type == "&" && output.Contains("rx"))
                foreach (var kv in conjunctions[dest])
                    if (kv.Value)
                        rxPresses[kv.Key] = presses;

            foreach (var d in output)
                queue.Enqueue(new Signal(dest, d, pulseOut.Value));
        }
    }

    private static bool Conjuction(Dictionary<string, Dictionary<string, bool>> conjunctions, string source,
        string dest, bool pulseIn)
    {
        conjunctions[dest][source] = pulseIn;
        return !conjunctions[dest].Values.All(s => s);
    }

    private static Module ParseLine(string line)
    {
        return line
            .SplitBy("->")
            .Into(parts =>
            {
                var strip = parts.First()[0] is '%' or '&' ? 1 : 0;
                var type = strip > 0 ? parts.First()[0].ToString() : parts.First();
                return new Module(type, parts.First()[strip..], parts.Second().SplitBy(","));
            });
    }

    private record Module(string Type, string Name, string[] Outputs);

    private record Signal(string Source, string Destination, bool PulseIn);
}