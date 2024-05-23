using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;

//await new AdventOfCodeRunner(2017).RunAsync(args);

BenchmarkRunner.Run<BenchmarkJob>();

//var job = new BenchmarkJob();
//await job.Setup();
//await job.Job3();


public class BenchmarkJob
{
    private string[] _data;

    public BenchmarkJob()
    {

    }

    [GlobalSetup]
    public async Task Setup()
    {
        _data = await new InputReader().ReadLinesAsync(0).ToArrayAsync();
    }

    [Benchmark]
    public async Task Job1()
    {
        var program = _data.Select(line => line.SplitBy(" ")).ToArray();

        var registers = new Dictionary<string, int>()
        {
            {"a", 0 },
            {"b", 0 },
            {"c", 1 },
            {"d", 0 },
        };

        var ip = 0;
        while (ip < program.Length)
        {
            _ = program[ip] switch
            {
            ["cpy", var x, var y] when int.TryParse(x, out var ix) => registers[y] = ix,
            ["cpy", var x, var y] => registers[y] = registers[x],
            ["jnz", var x, var y] when int.TryParse(x, out var ix) => ip = ix != 0 ? ip + int.Parse(y) - 1 : ip,
            ["jnz", var x, var y] => ip = registers[x] != 0 ? ip + int.Parse(y) - 1 : ip,
            ["inc", var x] => registers[x] = registers[x] + 1,
            ["dec", var x] => registers[x] = registers[x] - 1,
                _ => throw new NotImplementedException()
            };

            ip++;
        }

        Debug.Assert(registers["a"] == 9227657);
    }

    [Benchmark]
    public async Task Job2()
    {
        var program = _data.Select(line => line.SplitBy(" ")).ToArray();

        var cpu = new Cpu<int>(program);

        var cpy = new Instruction<int>((cpu, ip, args) =>
        {
            cpu.SetValue(args[2], cpu.GetValue(args[1]));
            return ip + 1;
        });

        var jnz = new Instruction<int>((cpu, ip, args) =>
        {
            var x = cpu.GetValue(args[1]);
            var y = args[2].As<int>();

            if (x != 0)
                return ip + y;

            return ip + 1;
        });

        var inc = new Instruction<int>((cpu, ip, args) =>
        {
            cpu.SetValue(args[1], cpu.GetValue(args[1]) + 1);
            return ip + 1;
        });

        var dec = new Instruction<int>((cpu, ip, args) =>
        {
            cpu.SetValue(args[1], cpu.GetValue(args[1]) - 1);
            return ip + 1;
        });

        cpu.AddInstruction("cpy", cpy);
        cpu.AddInstruction("jnz", jnz);
        cpu.AddInstruction("inc", inc);
        cpu.AddInstruction("dec", dec);

        cpu.SetValue("a", 0);
        cpu.SetValue("b", 0);
        cpu.SetValue("c", 1);
        cpu.SetValue("d", 0);

        while (cpu.MoveNext())
        { }

        Debug.Assert(cpu.GetValue("a") == 9227657);
    }

    [Benchmark]
    public async Task Job3()
    {
        var program = _data.Select(line => line.SplitBy(" ")).ToArray();

        var cpu = new Cpu<int>(program);

        var cpy = new Instruction<int>((cpu, ip, args) =>
        {
            if (!int.TryParse(args[1], out var x))
                x = cpu.Registers[args[1]];

            cpu.Registers[args[2]] = x;
            return ip + 1;
        });

        var jnz = new Instruction<int>((cpu, ip, args) =>
        {
            if (!int.TryParse(args[1], out var x))
                x = cpu.Registers[args[1]];

            var y = args[2].As<int>();

            if (x != 0)
                return ip + y;

            return ip + 1;
        });

        var inc = new Instruction<int>((cpu, ip, args) =>
        {
            cpu.Registers[args[1]]++;
            return ip + 1;
        });

        var dec = new Instruction<int>((cpu, ip, args) =>
        {
            cpu.Registers[args[1]]--;
            return ip + 1;
        });

        cpu.AddInstruction("cpy", cpy);
        cpu.AddInstruction("jnz", jnz);
        cpu.AddInstruction("inc", inc);
        cpu.AddInstruction("dec", dec);

        cpu.SetValue("a", 0);
        cpu.SetValue("b", 0);
        cpu.SetValue("c", 1);
        cpu.SetValue("d", 0);

        while (cpu.MoveNext())
        { }

        Debug.Assert(cpu.GetValue("a") == 9227657);
    }
}

public delegate int Instruction<TRegister>(Cpu<TRegister> cpu, int ip, string[] args) where TRegister : IBinaryInteger<TRegister>, IParsable<TRegister>;

public class Cpu<TRegister>(string[][] program)
    where TRegister : IBinaryInteger<TRegister>, IParsable<TRegister>
{
    private readonly Dictionary<string, Instruction<TRegister>> _instructionSet = new();

    private int _ip;

    public bool _halt;

    public Dictionary<string, TRegister> Registers { get; } = new();

    public bool MoveNext()
    {
        if (_halt || _ip < 0 || _ip >= program.Length)
            return false;

        var current = program[_ip];
        if (!_instructionSet.TryGetValue(current.First(), out var instruction))
            throw new InvalidOperationException("Invalid instruction");

        _ip = instruction.Invoke(this, _ip, current);
        return true;
    }

    public void Reset()
    {
        _ip = 0;
        _halt = false;
        Registers.Clear();
    }

    public void AddInstruction(string opCode, Instruction<TRegister> instruction) => _instructionSet.Add(opCode, instruction);

    public TRegister GetValue(string registerOrValue)
    {
        if (!TRegister.TryParse(registerOrValue, CultureInfo.CurrentCulture, out TRegister? result))
            result = Registers!.GetValueOrDefault(registerOrValue, default);

        return result!;
    }

    public void SetValue(string register, TRegister value) => Registers[register] = value;
}