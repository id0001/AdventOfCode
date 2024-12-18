namespace AdventOfCode.Lib
{
    public readonly record struct Point1(int X) : IPoint<int>, INeighbors<int>
    {
        public static readonly Point1 Zero = new();

        public Point1 Left => new(X - 1);

        public Point1 Right => new(X + 1);

        #region Interface implementations

        int IPoint<int>.this[int index] => index switch
        {
            0 => X,
            _ => throw new NotSupportedException()
        };

        int IPoint<int>.Dimensions => 1;

        IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal)
            => this.GetNeighbors().Cast<IPoint<int>>();

        #endregion

        public override int GetHashCode() => X.GetHashCode();

        public override string ToString() => X.ToString();

        public void Deconstruct(out int x) => x = X;

        #region Operators

        public static implicit operator Point1(int x) => new(x);

        public static implicit operator int(Point1 p) => p.X;

        #endregion
    }
}
