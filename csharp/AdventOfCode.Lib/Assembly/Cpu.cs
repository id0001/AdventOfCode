namespace AdventOfCode.Lib.Assembly;

public class Cpu<TMemory, TOpCode, TArguments>(TMemory memory, IList<Instruction<TOpCode, TArguments>> program)
    where TMemory : IMemory
    where TOpCode : notnull
{
    private readonly Dictionary<TOpCode, Action<TArguments, TMemory>> _instructionSet = new();

    public TMemory Memory { get; } = memory;

    public IList<Instruction<TOpCode, TArguments>> Program { get; } = program;

    public bool IsHalted => Memory.Ip < 0 || Memory.Ip >= Program.Count;

    public void Reset() => Memory.Clear();

    public bool Next()
    {
        if (IsHalted)
            return false;

        var instruction = Program[Memory.Ip];
        var action = _instructionSet[instruction.OpCode];

        action(instruction.Arguments, Memory);
        return true;
    }

    public void AddInstruction(TOpCode opcode, Action<TArguments, TMemory> action) =>
        _instructionSet.Add(opcode, action);

    public void RunTillHalted()
    {
        while (!IsHalted)
            Next();
    }

    public void RunCycles(int cycles)
    {
        for (var i = 0; i < cycles && !IsHalted; i++)
            Next();
    }
}