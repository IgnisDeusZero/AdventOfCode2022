using AdventOfCode2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions.Day17Models
{
    public class Cup
    {
        private readonly LongPoint Left = new(-1, 0);
        private readonly LongPoint Right = new(1, 0);
        private readonly LongPoint Down = new(0, -1);

        private readonly ICupContent cupContent;

        public LongPoint lastStoneCoord { get; private set; } = new(0, 0);
        private Stone lastStone = new(
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(4, 0),
                new(5, 0),
                new(6, 0));

        public long CupWidth => cupContent.Width;
        public long CupHight => cupContent.Height;
        public long HighestStone { get; private set; } = 0;

        public Cup(ICupContent cupContent)
        {
            this.cupContent = cupContent;
        }

        public void Add(Stone stone)
        {
            SaveLastStone();
            lastStone = Normalize(stone);
            lastStoneCoord = new LongPoint(2, HighestStone + 4);
        }

        public bool TryMoveLeft()
        {
            if (lastStoneCoord.X == 0)
            {
                return false;
            }
            return TryMove(Left);
        }

        public bool TryMoveRight()
        {
            if (lastStoneCoord.X + lastStone.Width >= CupWidth)
            {
                return false;
            }
            return TryMove(Right);
        }

        public bool TryMoveDown()
        {
            return TryMove(Down);
        }

        public void SkipCycles(long cycles, long highestStoneDiff, long lastStoneCoordYDiff)
        {
            HighestStone += cycles * highestStoneDiff;
            lastStoneCoord = new LongPoint(lastStoneCoord.X, lastStoneCoord.Y + cycles * lastStoneCoordYDiff);
        }

        private bool TryMove(LongPoint offset)
        {
            var lastStoneCoordsLocalNew = lastStoneCoord + offset;
            var stonePoints = ConvertStoneToCupCoordinates(lastStone, lastStoneCoordsLocalNew);

            foreach (var point in stonePoints)
            {
                if (cupContent[point] != 0)
                {
                    return false;
                }
            }

            lastStoneCoord = new LongPoint(lastStoneCoordsLocalNew.X, lastStoneCoordsLocalNew.Y);
            return true;
        }

        private void SaveLastStone()
        {
            var points = ConvertStoneToCupCoordinates(lastStone, lastStoneCoord);
            foreach (var point in points)
            {
                cupContent[point] = 1;
                HighestStone = Math.Max(HighestStone, point.Y);
            }
        }

        private static IEnumerable<LongPoint> ConvertStoneToCupCoordinates(Stone stone, LongPoint coord)
        {
            return stone.Points.Select(x => coord + x);
        }

        private static Stone Normalize(Stone stone)
        {
            var offset = new LongPoint(
                   -stone.Points.Min(x => x.X),
                   -stone.Points.Min(x => x.Y));
            if (offset.X == 0 && offset.Y == 0)
            {
                return stone;
            }

            var points = stone.Points.Select(x => x + offset).ToArray();
            return new Stone(points);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var maxHeight = Math.Max(HighestStone, lastStoneCoord.Y + lastStone.Height) + 1;
            var lastStonePoints = ConvertStoneToCupCoordinates(lastStone, lastStoneCoord);
            for (var y = maxHeight; y >= 0; y--)
            {
                for (var x = 0; x < CupWidth; x++)
                {
                    if (y >= lastStoneCoord.Y && lastStonePoints.Contains(new LongPoint(x, y)))
                    {
                        stringBuilder.Append('@');
                        continue;
                    }
                    stringBuilder.Append(cupContent[x, y] == 0 ? '.' : '#');
                }

                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }
    }
}
