using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day01 : SolutionBase
    {
        public Day01() : base("./Inputs/Day01.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByDoubleNewlines()
                .Select(x => x.SplitByNewlines().Select(int.Parse).Sum())
                .Max()
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .SplitByDoubleNewlines()
                .Select(x => x.SplitByNewlines().Select(int.Parse).Sum())
                .OrderByDescending(x => x)
                .Take(3)
                .Sum()
                .ToString();
        }
    }
}
