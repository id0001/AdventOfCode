using AdventOfCode.Lib.Assembly;

namespace AdventOfCode.Lib.Tests;

[TestClass]
public class IntCodeTests
{
    [TestMethod]
    public async Task Do_The_IntCode()
    {
        var input = await InputReader.ReadLineAsync<int>("2019.2.1.txt", ',').ToListAsync();

        var cpu = new IntCode(new Memory(input));

        var result = string.Empty;
        for (var noun = 0; noun < 100; noun++)
        for (var verb = 0; verb < 100; verb++)
        {
            cpu.Reset();
            cpu.Memory[1] = noun;
            cpu.Memory[2] = verb;
            cpu.RunTillHalted();
            if (cpu.Memory[0] == 19690720)
            {
                result = (100 * noun + verb).ToString();
                goto End;
            }
        }

        End:

        cpu.RunTillHalted();
        Assert.AreEqual("7264", result);
    }

    public class IntCode : Cpu<Memory, int>
    {
        private readonly Dictionary<int, Action<Memory>> _instructionSet = new();

        public IntCode(Memory memory) : base(memory)
        {
            AddInstruction(1, Add);
            AddInstruction(2, Mul);
            AddInstruction(99, Halt);
        }

        public override bool Next()
        {
            if (IsHalted)
                return false;

            var opCode = Memory.Program[Memory.Ip];
            var action = _instructionSet[opCode];
            action(Memory);
            return true;
        }

        private void AddInstruction(int opCode, Action<Memory> action) => _instructionSet.Add(opCode, action);

        private static void Add(Memory memory)
        {
            var a = GetValue(memory, 0);
            var b = GetValue(memory, 1);
            var addr = GetAddress(memory, 2);

            memory[addr] = a + b;
            memory.Ip += 4;
        }

        private static void Mul(Memory memory)
        {
            var a = GetValue(memory, 0);
            var b = GetValue(memory, 1);
            var addr = GetAddress(memory, 2);

            memory[addr] = a * b;
            memory.Ip += 4;
        }

        private static void Halt(Memory memory)
        {
            memory.Ip = -1;
        }

        private static int GetValue(Memory memory, int offset)
        {
            return memory[memory[memory.Ip + offset + 1]];
        }

        private static int GetAddress(Memory memory, int offset)
        {
            return memory[memory.Ip + offset + 1];
        }
    }

    public sealed class Memory(IList<int> program) : IMemory<int>
    {
        public int this[int index]
        {
            get => Program[index];
            set => Program[index] = value;
        }

        public int Ip { get; set; }

        public IList<int> Program { get; private set; } = program.ToList();

        public void Clear()
        {
            Ip = 0;
            Program = program.ToList(); // Reset program
        }
    }
}