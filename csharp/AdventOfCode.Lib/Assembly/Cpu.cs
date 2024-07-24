namespace AdventOfCode.Lib.Assembly
{
    public class Cpu<TRegister, TMemory>
        where TRegister : IParsable<TRegister>
        where TMemory : DefaultMemory<TRegister>, new()
    {
        private readonly Dictionary<string, Action<Arguments<TRegister>, TMemory>> _instructionSet = new();
        private readonly List<(string Opcode, Arguments<TRegister> Arguments)> _program = new();

        public TMemory Memory { get; } = new();

        public bool IsHalted => Memory.Ip < 0 || Memory.Ip >= _program.Count;

        public void LoadProgram(IEnumerable<Instruction<TRegister>> instructions)
        {
            Reset();

            foreach (var instruction in instructions)
            {
                var args = new Arguments<TRegister>(instruction.Arguments, Memory);
                _program.Add((instruction.OpCode, args));
            }
        }

        public void Reset()
        {
            _program.Clear();
            Memory.Clear();
        }

        public bool Next()
        {
            if (IsHalted)
                return false;

            var instruction = _program[Memory.Ip];
            var action = _instructionSet[instruction.Opcode];

            action(instruction.Arguments, Memory);
            return true;
        }

        public void AddInstruction(string opcode, Action<Arguments<TRegister>, TMemory> action) => _instructionSet.Add(opcode, action);

        public void RunTillHalted()
        {
            while (!IsHalted)
                Next();
        }
    }
}