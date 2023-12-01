using System.Collections.Immutable;

namespace AdventOfCode.Lib.Misc
{
    public class CubeWalker
    {
        private readonly int _size;

        private readonly ImmutableDictionary<(Side, Side), Func<Point2, Point2>> _operations;
        private readonly ImmutableDictionary<(Face, Side), Edge> _edges;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">Size of a face</param>
        /// <param name="edges">All edges that define how each face is connected to another face (12 edges)</param>
        public CubeWalker(int size, Edge[] edges)
        {
            if (edges.Length != 12)
                throw new ArgumentOutOfRangeException(nameof(edges), "A cube has 12 edges");

            _size = size;
            _edges = InitializeEdges(edges);
            _operations = InitializeOperations();
        }

        public (CubeCoord, Point2) Move(CubeCoord coord, Point2 direction)
        {
            return direction switch
            {
                (0, -1) => MoveUp(coord, direction),
                (0, 1) => MoveDown(coord, direction),
                (-1, 0) => MoveLeft(coord, direction),
                (1, 0) => MoveRight(coord, direction),
                _ => throw new NotImplementedException()
            };
        }

        private (CubeCoord, Point2) MoveRight(CubeCoord coord, Point2 direction)
        {
            if (coord.Position.X + 1 < _size)
                return (new CubeCoord(coord.Face, new Point2(coord.Position.X + 1, coord.Position.Y)), direction);

            return MoveToNewFace(coord, direction, Side.Right);
        }

        private (CubeCoord, Point2) MoveLeft(CubeCoord coord, Point2 direction)
        {
            if (coord.Position.X - 1 >= 0)
                return (new CubeCoord(coord.Face, new Point2(coord.Position.X - 1, coord.Position.Y)), direction);

            return MoveToNewFace(coord, direction, Side.Left);
        }

        private (CubeCoord, Point2) MoveDown(CubeCoord coord, Point2 direction)
        {
            if (coord.Position.Y + 1 < _size)
                return (new CubeCoord(coord.Face, new Point2(coord.Position.X, coord.Position.Y + 1)), direction);

            return MoveToNewFace(coord, direction, Side.Bottom);
        }

        private (CubeCoord, Point2) MoveUp(CubeCoord coord, Point2 direction)
        {
            if (coord.Position.Y - 1 >= 0)
                return (new CubeCoord(coord.Face, new Point2(coord.Position.X, coord.Position.Y - 1)), direction);

            return MoveToNewFace(coord, direction, Side.Top);
        }

        private (CubeCoord, Point2) MoveToNewFace(CubeCoord coord, Point2 direction, Side fromSide)
        {
            var edge = _edges[(coord.Face, fromSide)];
            return (new CubeCoord(edge.ToFace, _operations[(edge.FromSide, edge.ToSide)](coord.Position)), UpdateDirection(edge.FromSide, edge.ToSide, direction));
        }

        private Point2 UpdateDirection(Side from, Side to, Point2 direction)
        {
            if (from == to)
                return Point2.Turn(direction, Point2.Zero, System.Math.PI);

            if (from == Side.Top && to == Side.Left || from == Side.Bottom && to == Side.Right)
                return Point2.Turn(direction, Point2.Zero, System.Math.PI / 2d);

            if (from == Side.Top && to == Side.Right || from == Side.Bottom && to == Side.Left)
                return Point2.Turn(direction, Point2.Zero, -(System.Math.PI / 2d));

            if (from == Side.Right && to == Side.Top || from == Side.Left && to == Side.Bottom)
                return Point2.Turn(direction, Point2.Zero, System.Math.PI / 2d);

            if (from == Side.Right && to == Side.Bottom || from == Side.Left && to == Side.Top)
                return Point2.Turn(direction, Point2.Zero, -(System.Math.PI / 2d));

            return direction;
        }

        private Point2 HorizontalFlip(Point2 p) => new Point2(Flip(p.X), p.Y);

        private Point2 Swap(Point2 p) => new Point2(p.Y, p.X);

        private Point2 FlipSwap(Point2 p) => new Point2(Flip(p.Y), Flip(p.X));

        private Point2 VerticalFlip(Point2 p) => new Point2(p.X, Flip(p.Y));

        private int Flip(int v) => _size - 1 - v;

        private ImmutableDictionary<(Side, Side), Func<Point2, Point2>> InitializeOperations()
        {
            return new Dictionary<(Side, Side), Func<Point2, Point2>>
            {
                { (Side.Left, Side.Left), VerticalFlip },
                { (Side.Left, Side.Right), HorizontalFlip },
                { (Side.Left, Side.Top), Swap },
                { (Side.Left, Side.Bottom), FlipSwap },
                { (Side.Right, Side.Left), HorizontalFlip },
                { (Side.Right, Side.Right), VerticalFlip },
                { (Side.Right, Side.Top), FlipSwap },
                { (Side.Right, Side.Bottom), Swap },
                { (Side.Top, Side.Left), Swap },
                { (Side.Top, Side.Right), FlipSwap },
                { (Side.Top, Side.Top), HorizontalFlip },
                { (Side.Top, Side.Bottom), VerticalFlip },
                { (Side.Bottom, Side.Left), FlipSwap },
                { (Side.Bottom, Side.Right), Swap },
                { (Side.Bottom, Side.Top), VerticalFlip },
                { (Side.Bottom, Side.Bottom), HorizontalFlip }
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<(Face, Side), Edge> InitializeEdges(IEnumerable<Edge> edges)
        {
            var tmp = new HashSet<Edge>();
            foreach (var edge in edges)
            {
                tmp.Add(edge);
                tmp.Add(new Edge(edge.ToFace, edge.FromFace, edge.ToSide, edge.FromSide));
            }

            return tmp.ToImmutableDictionary(kv => (kv.FromFace, kv.FromSide));
        }

        public enum Face
        {
            Top = 0,
            Back = 1,
            Left = 2,
            Front = 3,
            Bottom = 4,
            Right = 5
        }

        public enum Side
        {
            Top = 0,
            Right = 1,
            Bottom = 2,
            Left = 3
        }

        public record CubeCoord(Face Face, Point2 Position);

        public record Edge(Face FromFace, Face ToFace, Side FromSide, Side ToSide);
    }
}
