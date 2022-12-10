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
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Sum())
                .Max()
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Sum())
                .OrderByDescending(x => x)
                .Take(3)
                .Sum()
                .ToString();
        }
    }
}
