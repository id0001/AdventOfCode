namespace AdventOfCode.Lib.Assembly
{
    public class DefaultMemory<TValue> where TValue : notnull, IParsable<TValue>
    {
        private readonly Dictionary<string, TValue> _registers = new();

        public int Ip { get; set; }

        public TValue Get(string register) => _registers!.GetValueOrDefault(register, default)!;

        public void Set(string register, TValue value) => _registers[register] = value;

        public virtual void Clear()
        {
            Ip = 0;
            _registers.Clear();
        }
    }
}
