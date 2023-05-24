namespace AdventOfCode.Lib.Graphs
{
    public static class FloydWarshall
    {
        public static Dictionary<(TVertex, TVertex), int> Calculate<TVertex>(IList<TVertex> vertices, IList<(TVertex, TVertex)> edges, Func<TVertex, TVertex, int> getDistance)
        {
            var matrix = new int?[vertices.Count, vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
                matrix[i, i] = 0;

            foreach (var (a, b) in edges)
                matrix[vertices.IndexOf(a), vertices.IndexOf(b)] = getDistance(a, b);

            for (int k = 0; k < vertices.Count; k++)
                for (int i = 0; i < vertices.Count; i++)
                    for (int j = 0; j < vertices.Count; j++)
                        if (matrix[i, k].HasValue && matrix[k, j].HasValue && (!matrix[i, j].HasValue || matrix[i, j] > matrix[i, k] + matrix[k, j]))
                            matrix[i, j] = matrix[i, k] + matrix[k, j];

            var distances = new Dictionary<(TVertex, TVertex), int>();
            for (int y = 0; y < vertices.Count; y++)
                for (int x = 0; x < vertices.Count; x++)
                {
                    if (matrix[x, y].HasValue)
                        distances.Add((vertices[x], vertices[y]), matrix[x, y]!.Value);
                }

            return distances;
        }
    }
}
