namespace AdventOfCode.Lib;

public readonly record struct Line3(Vector3 Intercept, Vector3 Slope)
{
    public Line2 ToLine2() => new(Intercept.ToVector2(), Slope.ToVector2());
}