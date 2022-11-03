namespace AdventOfCode.Lib.Comparers;

public class DoubleEqualityComparer : IEqualityComparer<double>
{
    private readonly int _precision;
    
    public DoubleEqualityComparer(int precision = 6)
    {
        _precision = precision;
    }

    public bool Equals(double x, double y) => x.MarginalEquals(y, _precision);

    public int GetHashCode(double obj) => obj.GetHashCode();
}