namespace AdventOfCode.Lib.Assembly;

public class RegisterMemory<TKey, TValue> : IMemory
    where TKey : notnull
    where TValue : IParsable<TValue>
{
    private readonly Dictionary<TKey, TValue> _registers = new();

    public IReadOnlyDictionary<TKey, TValue> Registers => _registers;

    public int Ip { get; set; }

    public virtual void Clear()
    {
        Ip = 0;
        _registers.Clear();
    }

    public TValue Get(TKey register, TValue defaultValue = default!) =>
        _registers.GetValueOrDefault(register, defaultValue);

    public void Set(TKey register, TValue value) => _registers[register] = value;
}