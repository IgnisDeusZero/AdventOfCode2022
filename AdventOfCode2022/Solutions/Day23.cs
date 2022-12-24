using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day23 : SolutionBase
    {
        public Day23() : base("./Inputs/Day23.txt")
        {
        }

        private List<(int xOffset, int yOffset)[]> nbgs = new()
        {
            // North
            new[] { (-1, 1), (0, 1), (1, 1) },
            // South
            new[] { (-1, -1), (0, -1), (1, -1) },
            // West
            new[] { (-1, -1), (-1, 0), (-1, 1) },
            // East
            new[] { (1, -1), (1, 0), (1, 1) },
        };

        public override string Part1()
        {
            var elfPositions = ParseInitPositions();

            for (var round = 0; round < 10; round++)
            {
                ExecuteMovingRound(elfPositions, round);
            }

            var minX = elfPositions.Min(x => x.X);
            var maxX = elfPositions.Max(x => x.X);
            var minY = elfPositions.Min(x => x.Y);
            var maxY = elfPositions.Max(x => x.Y);
            var sq = (maxX - minX + 1) * (maxY - minY + 1);
            return (sq - elfPositions.Count).ToString();
        }

        public override string Part2()
        {
            var elfPositions = ParseInitPositions();

            var round = 0;
            while (true)
            {
                var anyMoved = ExecuteMovingRound(elfPositions, round);
                round++;
                if (!anyMoved)
                {
                    return round.ToString();
                }
            }
        }

        private HashSet<(int X, int Y)> ParseInitPositions()
        {
            return Input.SplitByNewlines()
                .Reverse()
                .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
                .Where(x => x.c == '#')
                .Select(x => (X: x.x, Y: x.y))
                .ToHashSet();
        }

        private bool ExecuteMovingRound(
            HashSet<(int X, int Y)> elfPositions,
            int round)
        {
            var anyMoved = false;
            var movingProposals = new Dictionary<(int X, int Y), List<(int X, int Y)>>();
            foreach (var ep in elfPositions)
            {
                if (!AnyNbgAround(elfPositions, ep))
                {
                    continue;
                }

                for (var i = 0; i < 4; i++)
                {
                    var nbg = nbgs[(i + round) % 4];
                    var nbgPositions = nbg.Select(n => (n.xOffset + ep.X, n.yOffset + ep.Y)).ToArray();
                    var ocuppiedDirection = nbgPositions
                        .Select(n => elfPositions.Contains(n))
                        .Any(x => x);
                    if (ocuppiedDirection)
                    {
                        continue;
                    }
                    Add(movingProposals, nbgPositions[1], ep);
                    break;
                }
            }

            foreach (var mp in movingProposals)
            {
                if (mp.Value.Count > 1)
                {
                    continue;
                }
                anyMoved = true;
                elfPositions.Remove(mp.Value.Single());
                elfPositions.Add(mp.Key);
            }

            return anyMoved;
        }

        private bool AnyNbgAround(HashSet<(int X, int Y)> elfPositions, (int X, int Y) ep)
        {
            return nbgs.SelectMany(ns => ns.Select(n => (n.xOffset + ep.X, n.yOffset + ep.Y)))
                                .Any(pos => elfPositions.Contains(pos));
        }

        private void Add(
            Dictionary<(int X, int Y), List<(int, int)>> dict,
            (int X, int Y) key,
            (int X, int Y) value)
        {

            if (!dict.ContainsKey(key))
            {
                dict[key] = new List<(int, int)>();
            }
            dict[key].Add(value);
        }
    }
}
