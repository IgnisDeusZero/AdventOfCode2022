using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day14 : SolutionBase
    {
        public Day14() : base("./Inputs/Day14.txt")
        {
        }

        public override string Part1()
        {
            var unmovable = ParseMap();
            var abyssLevel = unmovable.Max(x => x.Y);
            var possibleOffsets = new[] { (X: 0, Y: 1), (X: -1, Y: 1), (X: 1, Y: 1) };
            var belowAbyss = false;
            var stonesCount = unmovable.Count;

            while (!belowAbyss)
            {
                var sand = (X: 500, Y: 0);
                while (true)
                {
                    var move = possibleOffsets
                        .Select(offset => ((int X, int Y)?)(sand.X + offset.X, sand.Y + offset.Y))
                        .FirstOrDefault(move => !unmovable.Contains(move.Value));
                    sand = move ?? sand;
                    belowAbyss = sand.Y > abyssLevel;
                    if (move is null)
                    {
                        unmovable.Add(sand);
                        break;
                    }
                    if (belowAbyss)
                    {
                        break;
                    }
                }
            }
            return (unmovable.Count - stonesCount).ToString();
        }

        public override string Part2()
        {
            var unmovable = ParseMap();
            var floorLevel = unmovable.Max(x => x.Y) + 2;
            var possibleOffsets = new[] { (X: 0, Y: 1), (X: -1, Y: 1), (X: 1, Y: 1) };
            var movableSand = true;
            var stonesCount = unmovable.Count;

            while (movableSand)
            {
                var sand = (X: 500, Y: 0);
                while (true)
                {
                    var move = possibleOffsets
                        .Select(offset => ((int X, int Y)?)(sand.X + offset.X, sand.Y + offset.Y))
                        .Where(move => move.Value.Y < floorLevel)
                        .FirstOrDefault(move => !unmovable.Contains(move.Value));
                    sand = move ?? sand;

                    if (move is null)
                    {
                        if (sand == (500, 0))
                        {
                            movableSand = false;
                        }
                        unmovable.Add(sand);
                        break;
                    }
                }
            }
            return (unmovable.Count - stonesCount).ToString();
        }

        private HashSet<(int X, int Y)> ParseMap()
        {
            var unmovable = new HashSet<(int X, int Y)>();

            Input.SplitByNewlines()
                .ToList()
                .ForEach(line => line
                    .Split(" -> ")
                    .Select(point => point.Split(',').Select(int.Parse).ToArray())
                    .Select(point => (X: point[0], Y: point[1]))
                    .Aggregate((a, b) =>
                    {
                        List<(int, int)> newStones;
                        if (a.X == b.X)
                        {
                            newStones = Enumerable
                                .Range(Math.Min(a.Y, b.Y), Math.Abs(a.Y - b.Y) + 1)
                                .Select(y => (a.X, y))
                                .ToList();
                        }
                        else if (a.Y == b.Y)
                        {
                            newStones = Enumerable
                                .Range(Math.Min(a.X, b.X), Math.Abs(a.X - b.X) + 1)
                                .Select(x => (x, a.Y))
                                .ToList();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        newStones.ForEach(stone => unmovable.Add(stone));
                        return b;
                    }));
            return unmovable;
        }
    }
}
