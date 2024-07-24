namespace AdventOfCode.Lib.Assembly
{
    public class Cpu<TMemory, TArguments>(TMemory memory, IList<Instruction<TArguments>> program)
        where TMemory : notnull, IMemory
    {
        private readonly Dictionary<string, Action<TArguments, TMemory>> _instructionSet = new();

        public TMemory Memory { get; } = memory;

        public IList<Instruction<TArguments>> Program { get; } = program;

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

        public void AddInstruction(string opcode, Action<TArguments, TMemory> action) => _instructionSet.Add(opcode, action);

        public void RunTillHalted()
        {
            while (!IsHalted)
                Next();
        }
    }
}