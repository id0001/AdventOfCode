namespace AdventOfCode.Lib.Assembly;

public class Assembler<TMemory, TOpCode, TArguments>(TMemory memory)
    : Cpu<TMemory, Instruction<TOpCode, TArguments>>(memory)
    where TMemory : IMemory<Instruction<TOpCode, TArguments>>
    where TOpCode : notnull
{
    private readonly Dictionary<TOpCode, Action<TArguments, TMemory>> _instructionSet = new();

    public override bool Next()
    {
        if (IsHalted)
            return false;

        var (opCode, args) = Memory.Program[Memory.Ip];
        var action = _instructionSet[opCode];
        action(args, Memory);
        return true;
    }

    public void AddInstruction(TOpCode opcode, Action<TArguments, TMemory> action)
        => _instructionSet.Add(opcode, action);
}