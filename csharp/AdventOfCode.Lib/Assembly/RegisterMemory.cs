using System.Globalization;

namespace AdventOfCode.Lib.Assembly
{
    public class RegisterMemory<TRegister> : IMemory
        where TRegister : notnull, IParsable<TRegister>
    {
        private readonly Dictionary<string, TRegister> _registers = new();

        public IReadOnlyDictionary<string, TRegister> Registers => _registers;

        public int Ip { get; set; }

        public TRegister Get(string registerOrValue) => TRegister.TryParse(registerOrValue, CultureInfo.InvariantCulture, out var value)
            ? value
            : _registers!.GetValueOrDefault(registerOrValue, default)!;

        public void Set(string register, TRegister value) => _registers[register] = value;

        public virtual void Clear()
        {
            Ip = 0;
            _registers.Clear();
        }
    }
}
