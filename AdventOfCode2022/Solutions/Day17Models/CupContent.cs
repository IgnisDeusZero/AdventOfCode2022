using AdventOfCode2022.Models;
using System;

namespace AdventOfCode2022.Solutions.Day17Models
{
    public class CupContent : ICupContent
    {
        private byte[,] content;

        public CupContent(long width, long height)
        {
            content = new byte[width, height];
            Width = width;
            Height = height;
        }

        public long Width { get; }
        public long Height { get; }

        public byte this[LongPoint point]
        {
            get => content[point.X, point.Y];
            set => content[point.X, point.Y] = value;
        }

        public byte this[long x, long y]
        {
            get => content[x, y];
            set => content[x, y] = value;
        }
    }
}
