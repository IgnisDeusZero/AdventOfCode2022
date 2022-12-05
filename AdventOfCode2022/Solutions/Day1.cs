using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day1 : SolutionBase
    {
        public Day1() : base("./Inputs/Day1.txt")
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
