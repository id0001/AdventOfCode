namespace AdventOfCode.Lib.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Point3 source) => new Vector3(source.X, source.Y, source.Z);
    }
}
