namespace AdventOfCode2019.IntCode.Core
{
    public partial class Cpu
    {
        private void Add()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            memory.Write(dest, a + b);

            ip += 4;
        }

        private void Multiply()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            memory.Write(dest, a * b);

            ip += 4;
        }

        private void Input()
        {
            waitingForInput = false;

            var dest = GetAddress(0);
            long input;

            if (!inputBuffer.TryDequeue(out input))
            {
                waitingForInput = true;
                return;
            }

            memory.Write(dest, input);

            ip += 2;
        }

        private void Output()
        {
            var value = GetValue(0);
            outputCallback.Invoke(value);

            ip += 2;
        }

        private void JumpIfTrue()
        {
            var a = GetValue(0);
            var b = GetValue(1);

            ip = a != 0 ? b : ip + 3;

        }

        private void JumpIfFalse()
        {
            var a = GetValue(0);
            var b = GetValue(1);

            ip = a == 0 ? b : ip + 3;

        }

        private void LessThan()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            memory.Write(dest, a < b ? 1 : 0);

            ip += 4;
        }

        private void Equals()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            memory.Write(dest, a == b ? 1 : 0);

            ip += 4;
        }

        private void AjustRelativeBase()
        {
            var a = GetValue(0);
            relativeBase += a;

            ip += 2;
        }
    }
}
