using System.Globalization;

namespace AdventOfCode.Lib.Assembly
{
    public record RegisterOrValueArgument<TValue> where TValue : IParsable<TValue>
    {
        private readonly string _registerOrValue;
        private readonly TValue? _value;

        public RegisterOrValueArgument(string registerOrValue)
        {
            _registerOrValue = registerOrValue;
            IsValue = TValue.TryParse(registerOrValue, CultureInfo.InvariantCulture, out _value);
        }

        public bool IsValue { get; }

        public TValue Value(RegisterMemory<TValue> registers) => IsValue ? _value! : registers.Get(_registerOrValue);

        public static implicit operator RegisterOrValueArgument<TValue>(string value) => new RegisterOrValueArgument<TValue>(value);

        public static implicit operator string(RegisterOrValueArgument<TValue> register) => register._registerOrValue;
    }
}
