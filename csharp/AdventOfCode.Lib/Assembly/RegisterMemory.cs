namespace AdventOfCode.Lib.Assembly;

public class RegisterMemory<TRegister> : IMemory
    where TRegister : IParsable<TRegister>
{
    private readonly Dictionary<string, TRegister> _registers = new();

    public IReadOnlyDictionary<string, TRegister> Registers => _registers;

    public int Ip { get; set; }

    public virtual void Clear()
    {
        Ip = 0;
        _registers.Clear();
    }

    public TRegister Get(string register, TRegister defaultValue = default!) =>
        _registers.GetValueOrDefault(register, defaultValue);

    public void Set(string register, TRegister value) => _registers[register] = value;
}