namespace AdventOfCode.Lib;

public static class Face
{
    /// <summary>
    ///     Face into the negative Y axis
    /// </summary>
    public static readonly Point2 Up = new(0, -1);

    /// <summary>
    ///     Face into the positive X axis
    /// </summary>
    public static readonly Point2 Right = new(1, 0);

    /// <summary>
    ///     Face into the positive Y axis
    /// </summary>
    public static readonly Point2 Down = new(0, 1);

    /// <summary>
    ///     Face into the negative X axis
    /// </summary>
    public static readonly Point2 Left = new(-1, 0);

    /// <summary>
    ///     Face into the positive Z axis
    /// </summary>
    public static readonly Point3 Backward = new(0, 0, 1);

    /// <summary>
    ///     Face into the negative Z axis
    /// </summary>
    public static readonly Point3 Forward = new(0, 0, -1);
}