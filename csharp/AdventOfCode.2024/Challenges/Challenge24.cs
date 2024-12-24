using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using System.Text;
using TextCopy;

namespace AdventOfCode2024.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var gates = await inputReader.ParseTextAsync(24, ParseInput);

        var outputs = gates.Where(g => g.Key.StartsWith("z")).OrderByDescending(g => g.Key).Select(g => g.Value).ToList();

        string output = "";
        foreach (var o in outputs)
            output += o.GetOutput() ? "1" : "0";

        return Convert.ToInt64(output, 2).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        /*
         * Adder GraphViz:
            digraph G {
               in1[label="IN2 (TRUE)"]
               in2[label="IN2 (TRUE)"]
               prev1[label="PREV1 (AND)"]
               prev2[label="PREV2 (AND)"]
   
               xor1[label="XOR"]
               or1[label="OR"]
               and1[label="AND"]
   
               out[label="OUT (XOR)"]
               next1[label="NEXT1 (AND)"]
               next2[label="NEXT2 (OR)"]
   
               in1 -> xor1
               in1 -> and1
               in2 -> xor1
               in2 -> and1
   
               prev1 -> or1
               prev2 -> or1
   
               or1 -> out
               or1 -> next1
               xor1 -> out
               xor1 -> next1
   
               and1 -> next2
               next1 -> next2
            }
         */

        var gates = await inputReader.ParseTextAsync(24, ParseInput);
        var gv = ToGraphViz(gates);
        await ClipboardService.SetTextAsync(gv);

        List<string> wires = ["z06", "fkp", "z11", "ngr", "z31", "mfm", "bpt", "krj"];
        return string.Join(",", wires.Order());
    }

    private static Dictionary<string, Gate> ParseInput(string input)
    {
        var gates = new Dictionary<string, Gate>();

        var paragraphs = input.SelectParagraphs();

        foreach (var (o, v) in paragraphs[0].SelectLines().Select(line => line.Extract<string, int>(@"(.+): (\d)")))
        {
            var gate = new Gate(v == 1 ? "TRUE" : "FALSE", string.Empty, string.Empty, gates);
            gates.Add(o, gate);
        }

        foreach (var (a, op, b, o) in paragraphs[1].SelectLines().Select(line => line.Extract<string, string, string, string>("(.+) (OR|AND|XOR) (.+) -> (.+)")))
        {
            var gate = new Gate(op, a, b, gates);
            gates.Add(o, gate);
        }

        return gates;
    }

    private static string ToGraphViz(Dictionary<string, Gate> gates)
    {
        var sb = new StringBuilder();
        sb.AppendLine("digraph G {");

        foreach (var key in gates.Keys)
        {
            sb.AppendLine($"{key}[label=\"{key}({gates[key].Type})\"]");
        }

        foreach (var key in gates.Keys)
        {
            var gate = gates[key];
            if (string.IsNullOrEmpty(gate.GateA) || string.IsNullOrEmpty(gate.GateB))
                continue;

            sb.AppendLine($"{gate.GateA} -> {key}");
            sb.AppendLine($"{gate.GateB} -> {key}");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }


    private class Gate(string type, string gateA, string gateB, Dictionary<string, Gate> gates)
    {
        public string Type { get; } = type;

        public string GateA { get; } = gateA;

        public string GateB { get; } = gateB;

        public bool GetOutput()
        {
            if (type == "TRUE")
                return true;

            if (type == "FALSE")
                return false;

            var a = gates[gateA];
            var b = gates[gateB];

            return type switch
            {
                "AND" => a.GetOutput() && b.GetOutput(),
                "OR" => a.GetOutput() || b.GetOutput(),
                "XOR" => a.GetOutput() ^ b.GetOutput(),
                _ => throw new NotImplementedException()
            };
        }
    }

}
