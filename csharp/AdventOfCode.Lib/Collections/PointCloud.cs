using System.Collections;

namespace AdventOfCode.Lib.Collections
{
    public class PointCloud<T> : IEnumerable<T> where T : IPoint, new()
    {
        private HashSet<T> _points = new HashSet<T>();

        public PointCloud()
        {
        }

        public PointCloud(IEnumerable<T> points)
        {
            foreach (var point in points)
                Set(point);
        }

        public BoundingBox<T> Bounds { get; } = new();

        public int Count => _points.Count;

        public void Set(T point)
        {
            if (_points.Add(point))
                Bounds.Inflate(point);
        }

        public void Unset(T point)
        {
            if (_points.Remove(point))
                Bounds.Deflate(_points, point);
        }

        public bool Contains(T point) => _points.Contains(point);

        public IEnumerator<T> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();
    }
}
