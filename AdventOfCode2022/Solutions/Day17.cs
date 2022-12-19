using AdventOfCode2022.Models;
using AdventOfCode2022.Solutions.Day17Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day17 : SolutionBase
    {
        private readonly Stone[] stones = new[]
        {
            new Stone(new (0,0), new (1,0), new (2,0), new (3,0)),
            new Stone(new (1,0), new (0,1), new (1,1), new (2,1), new (1,2)),
            new Stone(new (0,0), new (1,0), new (2,0), new (2,1), new (2,2)),
            new Stone(new (0,0), new (0,1), new (0,2), new (0,3)),
            new Stone(new (0,0), new (1,0), new (0,1), new (1,1)),
        };

        public Day17() : base("./Inputs/Day17.txt")
        {
        }

        public override string Part1()
        {
            var inputIndex = 0;
            var cupContent = new ExpandReducableCupContent(7, 100);
            var cup = new Cup(cupContent);
            var reducableHeight = 0L;
            for (var i = 0; i <= 2022; i++)
            {
                var movable = true;
                cup.Add(stones[i % stones.Length]);
                while (movable)
                {
                    _ = Input[inputIndex] switch
                    {
                        '>' => cup.TryMoveRight(),
                        '<' => cup.TryMoveLeft(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    movable = cup.TryMoveDown();
                    inputIndex++;
                    inputIndex %= Input.Length;
                }
                TryUpdateReducableHeight(cupContent, cup, ref reducableHeight);
                if (cupContent.Height - cup.HighestStone < 20)
                {
                    cupContent.ExpandReduce(reducableHeight, 100);
                }
            }
            return cup.HighestStone.ToString();
        }

        private static void TryUpdateReducableHeight(ExpandReducableCupContent cupContent, Cup cup, ref long reducableHeight)
        {
            var start = Math.Max(0, cup.lastStoneCoord.Y - 2);
            var end = cup.lastStoneCoord.Y + 10;
            for (var y = start; y < end; y++)
            {
                if (cupContent.CanReduceAt(y))
                {
                    reducableHeight = y;
                }
            }
        }

        public override string Part2()
        {
            var expandCapacity = 100;
            var inputIndex = 0;
            var cupContent = new ExpandReducableCupContent(7, expandCapacity);
            var cup = new Cup(cupContent);
            var reducableHeight = 0L;
            var contents = new Dictionary<long, (ExpandReducableCupContent Content, long HighestStone, LongPoint LastStoneCoord)>();
            var skippedCycles = false;
            var stonesToFall = 1_000_000_000_000;
            for (var i = 0L; i <= stonesToFall; i++)
            {
                var movable = true;
                cup.Add(stones[i % stones.Length]);
                while (movable)
                {
                    _ = Input[inputIndex] switch
                    {
                        '>' => cup.TryMoveRight(),
                        '<' => cup.TryMoveLeft(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    movable = cup.TryMoveDown();
                    inputIndex++;
                    inputIndex %= Input.Length;
                }
                if (!skippedCycles && i % (Input.Length * stones.Length) == 0)
                {
                    contents[i] = (cupContent.Copy(), cup.HighestStone, cup.lastStoneCoord);
                    var matchedContents = contents.Where(x => x.Value.Content.ContentEquals(cupContent)).ToArray();
                    if (matchedContents.Length > 1)
                    {
                        var cycleLength = Math.Abs(matchedContents[0].Key - matchedContents[1].Key);
                        var stonesToFallLeft = stonesToFall - i;
                        var skipCycles = stonesToFallLeft / cycleLength;
                        i += skipCycles * cycleLength;
                        var highestStoneToTopDiff = cup.CupHight - cup.HighestStone;
                        cup.SkipCycles(
                            skipCycles,
                            Math.Abs(matchedContents[0].Value.HighestStone - matchedContents[1].Value.HighestStone),
                            Math.Abs(matchedContents[0].Value.LastStoneCoord.Y - matchedContents[1].Value.LastStoneCoord.Y));

                        cupContent.NewHeight(cup.HighestStone + highestStoneToTopDiff);
                        skippedCycles = true;
                    }
                }
                TryUpdateReducableHeight(cupContent, cup, ref reducableHeight);
                if (cupContent.Height - cup.HighestStone < 200)
                {
                    cupContent.ExpandReduce(reducableHeight - 10, expandCapacity);
                }
            }
            return cup.HighestStone.ToString();
        }
    }
}
