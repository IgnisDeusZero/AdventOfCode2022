using AdventOfCode2022.Models;
using AdventOfCode2022.Solutions.Day17Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day18 : SolutionBase
    {
        private Point[] neighborsOffsets = new Point[]
        {
            new(0,0,1),
            new(0,1,0),
            new(1,0,0),
            new(0,0,-1),
            new(0,-1,0),
            new(-1,0,0),

        };

        public Day18() : base("./Inputs/Day18.txt")
        {
        }

        public override string Part1()
        {
            var drops = Input.SplitByNewlines()
                .Select(x => x.Split(",").Select(int.Parse).ToArray())
                .Select(x => new Point(x[0], x[1], x[2]))
                .ToHashSet();

            return drops.Select(d => neighborsOffsets.Count(o => !drops.Contains(d + o))).Sum().ToString();
        }

        public override string Part2()
        {
            var drops = Input.SplitByNewlines()
                .Select(x => x.Split(",").Select(int.Parse).ToArray())
                .Select(x => new Point(x[0], x[1], x[2]))
                .ToHashSet();

            var totalSurfacesCount = drops.Select(p => neighborsOffsets.Count(o => !drops.Contains(p + o))).Sum();
            var minPoint = new Point(drops.Min(x => x.X) - 1, drops.Min(x => x.Y) - 1, drops.Min(x => x.Z) - 1);
            var maxPoint = new Point(drops.Max(x => x.X) + 1, drops.Max(x => x.Y) + 1, drops.Max(x => x.Z) + 1);
            
            HashSet<Point> outerSpace = GetOuterSpace(drops, minPoint, maxPoint);

            var innerSurfacesCount =
            (
                from x in Enumerable.Range(minPoint.X, maxPoint.X - minPoint.X + 1)
                from y in Enumerable.Range(minPoint.Y, maxPoint.Y - minPoint.Y + 1)
                from z in Enumerable.Range(minPoint.Z, maxPoint.Z - minPoint.Z + 1)
                select new Point(x, y, z)
            )
            .Where(x => !drops.Contains(x) && !outerSpace.Contains(x))
            .Select(x => neighborsOffsets.Count(o => drops.Contains(o + x)))
            .Sum();

            return (totalSurfacesCount - innerSurfacesCount).ToString();
        }

        private HashSet<Point> GetOuterSpace(HashSet<Point> drops, Point minPoint, Point maxPoint)
        {
            var outerSpace = new HashSet<Point> { minPoint };
            var queue = new Queue<Point>();
            queue.Enqueue(minPoint);

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                var neighborPoints = neighborsOffsets
                    .Select(x => x + point)
                    .Where(np => np.X >= minPoint.X && np.Y >= minPoint.Y && np.Z >= minPoint.Z
                        && np.X <= maxPoint.X && np.Y <= maxPoint.Y && np.Z <= maxPoint.Z
                        && !drops.Contains(np) && !outerSpace.Contains(np))
                    .ToArray();
                foreach (var n in neighborPoints)
                {
                    outerSpace.Add(n);
                    queue.Enqueue(n);
                }
            }

            return outerSpace;
        }
    }
}
