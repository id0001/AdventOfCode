namespace AdventOfCode.Lib.Comparers;

public class DoubleEqualityComparer(int precision = 6) : IEqualityComparer<double>
{
    public bool Equals(double x, double y) => x.IsSimilarTo(y, precision);

    public int GetHashCode(double obj) => obj.GetHashCode();
}