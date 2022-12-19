using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2022.Models
{
    public struct Point
    {
        public Point(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }

    public struct LongPoint
    {
        public LongPoint(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X { get; }
        public long Y { get; }

        public static LongPoint operator +(LongPoint p1, LongPoint p2)
        {
            return new LongPoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static LongPoint operator +(Point p1, LongPoint p2)
        {
            return new LongPoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static LongPoint operator +(LongPoint p1, Point p2)
        {
            return new LongPoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
