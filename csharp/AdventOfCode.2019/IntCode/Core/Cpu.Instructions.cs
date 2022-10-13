namespace AdventOfCode2019.IntCode.Core
{
    public partial class Cpu
    {
        private void Add()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            _memory.Write(dest, a + b);

            _ip += 4;
        }

        private void Multiply()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            _memory.Write(dest, a * b);

            _ip += 4;
        }

        private void Input()
        {
            _waitingForInput = false;

            var dest = GetAddress(0);
            long input;

            if (_inputBuffer.IsEmpty)
                _inputCallback?.Invoke();

            if (!_inputBuffer.TryDequeue(out input))
            {
                _waitingForInput = true;
                return;
            }

            _memory.Write(dest, input);

            _ip += 2;
        }

        private void Output()
        {
            var value = GetValue(0);
            _outputCallback?.Invoke(value);

            _ip += 2;
        }

        private void JumpIfTrue()
        {
            var a = GetValue(0);
            var b = GetValue(1);

            _ip = a != 0 ? b : _ip + 3;

        }

        private void JumpIfFalse()
        {
            var a = GetValue(0);
            var b = GetValue(1);

            _ip = a == 0 ? b : _ip + 3;

        }

        private void LessThan()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            _memory.Write(dest, a < b ? 1 : 0);

            _ip += 4;
        }

        private void Equals()
        {
            var a = GetValue(0);
            var b = GetValue(1);
            var dest = GetAddress(2);

            _memory.Write(dest, a == b ? 1 : 0);

            _ip += 4;
        }

        private void AjustRelativeBase()
        {
            var a = GetValue(0);
            _relativeBase += a;

            _ip += 2;
        }
    }
}
