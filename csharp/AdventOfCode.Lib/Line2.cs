namespace AdventOfCode.Lib;

public readonly record struct Line2
{
    public Line2(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        A = start.Y - end.Y;
        B = end.X - start.X;
        C = (start.X - end.X) * start.Y + (end.Y - start.Y) * start.X;
        IsVertical = start.X.IsSimilarTo(end.X);
        IsHorizontal = start.Y.IsSimilarTo(end.Y);
        Slope = (end.Y - start.Y) / (end.X - start.X);
        Intercept = start.Y - (end.Y - start.Y) / (end.X - start.X) * start.X;
    }

    public double A { get; }

    public double B { get; }

    public double C { get; }

    public bool IsVertical { get; }

    public bool IsHorizontal { get; }

    public double Slope { get; }

    public double Intercept { get; }
    
    public Vector2 Start { get; init; }
    
    public Vector2 End { get; init; }

    public static bool TryIntersect(Line2 line1, Line2 line2, out Vector2 intersection)
    {
        intersection = Vector2.Zero;

        var d = line1.A * line2.B - line2.A * line1.B;
        if (d == 0)
            return false;

        var x = (line1.B * line2.C - line2.B * line1.C) / d;
        var y = (line2.A * line1.C - line1.A * line2.C) / d;
        intersection = new Vector2(x, y);
        return true;
    }

    public void Deconstruct(out Vector2 start, out Vector2 end)
    {
        start = Start;
        end = End;
    }
}