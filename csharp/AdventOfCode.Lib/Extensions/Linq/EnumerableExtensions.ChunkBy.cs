namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, T separator)
        {
            var list = new List<T>();
            foreach (var item in source)
                if (!EqualityComparer<T>.Default.Equals(item, separator))
                {
                    list.Add(item);
                }
                else
                {
                    yield return list;
                    list = new List<T>();
                }
        }
    }
}
