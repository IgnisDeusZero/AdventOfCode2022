using AdventOfCode2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day15 : SolutionBase
    {
        private Regex regex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");
        private const int depth = 2000000;
        public Day15() : base("./Inputs/Day15.txt")
        {
        }

        public override string Part1()
        {
            var reachable = new HashSet<int>();
            var beacons = new HashSet<int>();
            Input.SplitByNewlines()
                .ToList()
                .ForEach(line =>
                {
                    var match = regex.Match(line);
                    var p1 = new Point(
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value));
                    var p2 = new Point(
                        int.Parse(match.Groups[3].Value),
                        int.Parse(match.Groups[4].Value));
                    var rad = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
                    var dist = Math.Abs(depth - p1.Y);
                    if (p2.Y == depth)
                    {
                        beacons.Add(p2.X);
                    }
                    if (rad < dist)
                    { 
                        return;
                    }
                    Enumerable.Range(0, rad - dist + 1)
                     .ToList()
                     .ForEach(xOffset =>
                     {
                         reachable.Add(p1.X + xOffset);
                         reachable.Add(p1.X - xOffset);
                     });
                });
            return reachable.Except(beacons).Count().ToString();
        }

        public override string Part2()
        {
            var segments = Enumerable
                .Range(0, depth * 2 + 1)
                .Select(x => new Segments(x))
                .ToArray();


            Input.SplitByNewlines()
                .ToList()
                .ForEach(line =>
                {
                    var match = regex.Match(line);
                    var p1 = new Point(
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value));
                    var p2 = new Point(
                        int.Parse(match.Groups[3].Value),
                        int.Parse(match.Groups[4].Value));
                    var rad = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
                    Enumerable.Range(0, rad)
                     .ToList()
                     .ForEach(yOffset =>
                     {
                         var r = p1.Y + yOffset;
                         var xRange = rad - yOffset;
                         if (r <= depth * 2)
                         {
                             segments[r].Add((Math.Max(0, p1.X - xRange), Math.Min(depth * 2, p1.X + xRange)));
                         }
                         r = p1.Y - yOffset;
                         if (r >= 0)
                         {
                             segments[r].Add((Math.Max(0, p1.X - xRange), Math.Min(depth * 2, p1.X + xRange)));
                         }
                     });
                });

            var nonFullSegment = segments
                .Single(x => x.segments.Count != 1 || !(x.segments[0].F == 0 && x.segments[0].T == depth * 2));
            if (nonFullSegment.segments.Count == 2)
            {
                long x = nonFullSegment.segments.OrderBy(x => x.F).First().T + 1;
                long y = nonFullSegment.row;
                return ((x * 4000000) + y).ToString();
            }
            throw new NotImplementedException();
        }
    }

    public class Segments
    {
        public List<(int F, int T)> segments = new List<(int, int)>();
        public readonly int row;

        public Segments(int row)
        {
            this.row = row;
        }

        public void Add((int F, int T) segment)
        {
            if (segment.F > segment.T)
            {
                throw new InvalidOperationException();
            }
            var segs = segments.Where(seg => IntersectOrConnect(segment, seg)).Append(segment).ToArray();
            foreach (var seg in segs)
            {
                segments.Remove(seg);
            }
            segments.Add(Join(segs));
        }

        private bool IntersectOrConnect((int F, int T) seg1, (int F, int T) seg2)
        {
            return !(seg1.T < seg2.F - 1 || seg2.T < seg1.F - 1);
        }

        private (int, int) Join(params (int F, int T)[] segs)
        {
            return (segs.Min(x => x.F), segs.Max(x => x.T));
        }

        public override string ToString()
        {
            return string.Join(",", segments.OrderBy(x => x.F).Select(x => $"({x.F}:{x.T})"));
        }
    }
}
