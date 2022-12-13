using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day03 : SolutionBase
    {
        public Day03() : base("./Inputs/Day03.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByNewlines()
                .Select(x => x.Substring(0, x.Length / 2).Intersect(x.Substring(x.Length / 2)).Single())
                .Select(x => char.IsUpper(x) ? x - 'A' + 27 : x - 'a' + 1)
                .Sum()
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .SplitByNewlines()
                .Select((x, i) => (x.AsEnumerable(), i))
                .GroupBy(x => x.i / 3)
                .Select(x => x.Select(x => x.Item1).Aggregate((acc, s) => acc.Intersect(s)).Single())
                .Select(x => char.IsUpper(x) ? x - 'A' + 27 : x - 'a' + 1)
                .Sum()
                .ToString();
        }
    }
}
