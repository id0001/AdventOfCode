namespace AdventOfCode.Lib.Assembly;

public abstract class Cpu<TMemory, TProgram>(TMemory memory)
    where TMemory : IMemory<TProgram>
{
    public TMemory Memory { get; } = memory;

    public bool IsHalted => Memory.Ip < 0 || Memory.Ip >= Memory.Program.Count;

    public virtual void Reset() => Memory.Clear();

    public abstract bool Next();

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