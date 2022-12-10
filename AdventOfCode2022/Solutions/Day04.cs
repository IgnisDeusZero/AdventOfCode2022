using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day04 : SolutionBase
    {
        public Day04() : base("./Inputs/Day04.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .Count(x => (x[0] <= x[2] && x[1] >= x[3]) || (x[2] <= x[0] && x[3] >= x[1]))
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .Count(x => InRange(x[0], x[1], x[2]) || InRange(x[0], x[1], x[3]) || InRange(x[2], x[3], x[0]) || InRange(x[2], x[3], x[1]))
                .ToString();
        }

        private bool InRange(int from, int to, int x)
        {
            return from <= x && x <= to;
        }
    }
}
