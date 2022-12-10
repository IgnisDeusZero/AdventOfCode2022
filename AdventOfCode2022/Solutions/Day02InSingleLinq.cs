using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day02InSingleLinq : SolutionBase
    {
        public Day02InSingleLinq() : base("./Inputs/Day02.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => ((s[2] - 'X' + 'A' - s[0] + 4) % 3 * 3) + (s[2] - 'X' + 1))
                // shizoteric
                //.Select(s => (s[2] - s[0] + 2) % 3 * 3 + (s[2] + 2) % 3 + 1)
                .Sum()
                .ToString();

        }

        public override string Part2()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => (s[2] - 'X') * 3 + ((s[0] - 'A') - (3 - (s[2] - 'X')) + 5) % 3 + 1)
                // shizoteric
                //.Select(s => (s[2] + 2) % 3 * 3 + (s[0] + s[2] + 2) % 3 + 1)
                .Sum()
                .ToString();
        }
    }
}
