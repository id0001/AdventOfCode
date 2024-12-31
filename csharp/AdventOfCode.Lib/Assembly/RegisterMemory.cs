namespace AdventOfCode.Lib.Assembly;

public interface IRegisterMemory<TKey, TValue>
{
    public TValue Get(TKey register, TValue defaultValue = default!);

    public void Set(TKey register, TValue value);
}

public interface IStringRegisterMemory<TValue> : IRegisterMemory<string, TValue>;

public class RegisterMemory<TKey, TValue, TProgram>(IList<TProgram> program)
    : IMemory<TProgram>, IRegisterMemory<TKey, TValue>
    where TKey : notnull
    where TValue : IParsable<TValue>
{
    private readonly Dictionary<TKey, TValue> _registers = new();

    public IReadOnlyDictionary<TKey, TValue> Registers => _registers;

    public int Ip { get; set; }

    public IList<TProgram> Program { get; } = program;

    public virtual void Clear()
    {
        Ip = 0;
        _registers.Clear();
    }

    public TValue Get(TKey register, TValue defaultValue = default!) =>
        _registers.GetValueOrDefault(register, defaultValue);

    public void Set(TKey register, TValue value) => _registers[register] = value;
}