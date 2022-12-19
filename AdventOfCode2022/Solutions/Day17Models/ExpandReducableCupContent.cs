using AdventOfCode2022.Models;
using System;

namespace AdventOfCode2022.Solutions.Day17Models
{
    public class ExpandReducableCupContent : ICupContent
    {
        private byte[,] content;

        public ExpandReducableCupContent(long width, long height)
        {
            content = new byte[width, height];
            Width = width;
            Height = height;
            LocalHeight = height;
        }

        private ExpandReducableCupContent(byte[,] content, long height, long reducedAt)
        {
            this.content = content;
            Width = content.GetLongLength(0);
            Height = height;
            LocalHeight = content.GetLongLength(1);
            this.ReducedAt = reducedAt;
        }

        public long Width { get; }
        public long Height { get; private set; }
        public long LocalHeight { get; private set; }
        public long ReducedAt { get; private set; } = 0;

        public byte this[LongPoint point]
        {
            get => content[point.X, point.Y - ReducedAt];
            set => content[point.X, point.Y - ReducedAt] = value;
        }

        public byte this[long x, long y]
        {
            get => content[x, y - ReducedAt];
            set => content[x, y - ReducedAt] = value;
        }

        public ExpandReducableCupContent Copy()
        {
            var contentCopy = new byte[Width, LocalHeight];
            Array.Copy(this.content, contentCopy, Width * LocalHeight * sizeof(byte));
            return new ExpandReducableCupContent(contentCopy, Height, ReducedAt);
        }

        public bool CanReduceAt(long reduceAt)
        {
            for (var x = 0; x < Width; x++)
            {
                if(this[x, reduceAt] == 0 && this[x, reduceAt+1] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void ExpandReduce(long reduceAt = 0, long expandOn = 0)
        {
            reduceAt = Math.Max(reduceAt, ReducedAt);
            var localStartHeightsDiff = reduceAt - ReducedAt;
            var newLocalHeight = (LocalHeight - localStartHeightsDiff) + expandOn;
            var newContent = new byte[Width, newLocalHeight];

            for (var y = 0; y < newLocalHeight; y++)
            {
                if (y + localStartHeightsDiff >= LocalHeight)
                {
                    break;
                }
                for (var x = 0; x < Width; x++)
                {
                    newContent[x, y] = content[x, y + localStartHeightsDiff];
                }
            }

            content = newContent;
            LocalHeight = newLocalHeight;
            Height += expandOn;
            ReducedAt = reduceAt;
        }

        public bool ContentEquals(ExpandReducableCupContent other)
        {
            var c1 = content;
            var c2 = other.content;
            var c1y = GetHighestStone(c1);
            var c2y = GetHighestStone(c2);
            while (c1y >= 0 && c2y >= 0)
            {
                for (var x = 0; x < 7; x++)
                {
                    if (c1[x, c1y] != c2[x, c2y])
                    {
                        return false;
                    }
                }
                c1y--;
                c2y--;
            }
            return true;
        }

        public void NewHeight(long height)
        {
            Height = height;
            ReducedAt = Height - LocalHeight;
        }

        private long GetHighestStone(byte[,] c)
        {
            for (var y = c.GetLongLength(1) - 1; y > 0; y--)
            {
                for (var x = 0; x < 7; x++)
                {
                    if (c[x, y] != 0)
                    {
                        return y;
                    }
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return $"({Width}x{LocalHeight}) {Height} - {ReducedAt}";
        }
    }
}
