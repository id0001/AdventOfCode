namespace AdventOfCode.Lib.Math;

public static class Trigonometry
{
    /// <summary>
    /// PI / 180: To convert between degree and radian
    /// </summary>
    public const double PiOver180 = 0.0174532925199432957692369076848861271344287188854172545609719144d;

    public static double DegreeToRadian(double degree) => degree * PiOver180;

    public static double RadianToDegree(double radian) => radian / PiOver180;
}