using AdventOfCode2022.Models;
using System;

namespace AdventOfCode2022.Solutions.Day22Models
{
    public class CubeMap
    {
        public const int SweepSchemaSize = 5;
        public int[][] Sweep { get; set; }
        public int[,] SweepSchema { get; set; }
        public (int ArriveEdge, int ArriveDir)[][] EdgeTransitions { get; set; }
        public int[][,] Edges { get; set; }
        public int EdgeSize { get; set; }

        public Point EdgePos { get; set; } =  new Point(0, 0, 0);
        public int Direction { get; set; }

        public void Move(int steps)
        {
            var pos = EdgePos;
            while (steps --> 0)
            {
                pos = NextPoint(pos);
            }
            EdgePos = pos;
        }

        private Point NextPoint(Point pos)
        {
            return Direction switch
            {
                0 => NextXPoint(pos, 1),
                1 => NextYPoint(pos, 1),
                2 => NextXPoint(pos, -1),
                3 => NextYPoint(pos, -1),
                _ => throw new NotImplementedException()
            };
        }

        private Point NextYPoint(Point point, int direction)
        {
            var newPoint = new Point(point.X, point.Y + direction, point.Z);
            int newDirection = Direction;
            if (newPoint.Y < 0)
            {
                var gotoDir = 3;
                var arriveDir = EdgeTransitions[point.Z][gotoDir].ArriveDir;
                var arriveEdge = EdgeTransitions[point.Z][gotoDir].ArriveEdge;
                var (y, x) = TransformOnEdgeChanging(gotoDir, arriveDir, point.Y, point.X);
                newDirection = (arriveDir + 2) % 4;
                newPoint = new(x, y, arriveEdge);
            }
            if (newPoint.Y >= EdgeSize)
            {
                var gotoDir = 1;
                var arriveDir = EdgeTransitions[point.Z][gotoDir].ArriveDir;
                var arriveEdge = EdgeTransitions[point.Z][gotoDir].ArriveEdge;
                var (y, x) = TransformOnEdgeChanging(gotoDir, arriveDir, point.Y, point.X);
                newDirection = (arriveDir + 2) % 4;
                newPoint = new(x, y, arriveEdge);
            }
            if (Edges[newPoint.Z][newPoint.Y, newPoint.X] == 1)
            {
                Direction = newDirection;
                return newPoint;
            }
            return point;
        }

        private Point NextXPoint(Point point, int direction)
        {
            var newPoint = new Point(point.X + direction, point.Y, point.Z);
            int newDirection = Direction;
            if (newPoint.X < 0)
            {
                var gotoDir = 2;
                var arriveDir = EdgeTransitions[point.Z][gotoDir].ArriveDir;
                var arriveEdge = EdgeTransitions[point.Z][gotoDir].ArriveEdge;
                var (y, x) = TransformOnEdgeChanging(gotoDir, arriveDir, point.Y, point.X);
                newDirection = (arriveDir + 2) % 4;
                newPoint = new(x, y, arriveEdge);
            }
            if (newPoint.X >= EdgeSize)
            {
                var gotoDir = 0;
                var arriveDir = EdgeTransitions[point.Z][gotoDir].ArriveDir;
                var arriveEdge = EdgeTransitions[point.Z][gotoDir].ArriveEdge;
                var (y, x) = TransformOnEdgeChanging(gotoDir, arriveDir, point.Y, point.X);
                newDirection = (arriveDir + 2) % 4;
                newPoint = new(x, y, arriveEdge);
            }
            if (Edges[newPoint.Z][newPoint.Y, newPoint.X] == 1)
            {
                Direction = newDirection;
                return newPoint;
            }
            return  point;
        }

        private (int Y, int X) TransformOnEdgeChanging(int gotoDir, int arriveDir, int y, int x)
        {
            return (gotoDir, arriveDir) switch
            {
                (0, 0) when x == EdgeSize - 1 => (EdgeSize - 1 - y, EdgeSize - 1),
                (0, 1) when x == EdgeSize - 1 => (EdgeSize - 1, y),
                (0, 2) when x == EdgeSize - 1 => (y, 0),
                (0, 3) when x == EdgeSize - 1 => (0, EdgeSize - 1 - y),

                (1, 0) when y == EdgeSize - 1 => (x, EdgeSize - 1),
                (1, 1) when y == EdgeSize - 1 => (EdgeSize - 1, EdgeSize - 1 - x),
                (1, 2) when y == EdgeSize - 1 => (EdgeSize - 1 - x, 0),
                (1, 3) when y == EdgeSize - 1 => (0, x),

                (2, 0) when x == 0 => (y, EdgeSize - 1),
                (2, 1) when x == 0 => (EdgeSize - 1, EdgeSize - 1 - y),
                (2, 2) when x == 0 => (EdgeSize - 1 - y, 0),
                (2, 3) when x == 0 => (0, y),

                (3, 0) when y == 0 => (EdgeSize - 1 - x, EdgeSize - 1),
                (3, 1) when y == 0 => (EdgeSize - 1, x),
                (3, 2) when y == 0 => (x, 0),
                (3, 3) when y == 0 => (0, EdgeSize - 1 - x),

                _ => throw new NotImplementedException()
            };
        }

        public Point GetAbsolutePosition()
        {
            for (var row = 0; row < SweepSchemaSize; row++)
            {
                for (var col = 0; col < SweepSchemaSize; col++)
                {
                    if (SweepSchema[row,col] == EdgePos.Z + 1)
                    {
                        return new Point(col * EdgeSize + EdgePos.X, row * EdgeSize + EdgePos.Y);
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}
