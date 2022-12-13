using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day02 : SolutionBase
    {
        Dictionary<string, int> strategy1 = new Dictionary<string, int>
        {
            ["A X"] = 1 + 3,
            ["A Y"] = 2 + 6,
            ["A Z"] = 3 + 0,
            ["B X"] = 1 + 0,
            ["B Y"] = 2 + 3,
            ["B Z"] = 3 + 6,
            ["C X"] = 1 + 6,
            ["C Y"] = 2 + 0,
            ["C Z"] = 3 + 3,
        }; 
        Dictionary<string, string> strategy2 = new Dictionary<string, string>
        {
            ["A X"] = "A Z",
            ["A Y"] = "A X",
            ["A Z"] = "A Y",
            ["B X"] = "B X",
            ["B Y"] = "B Y",
            ["B Z"] = "B Z",
            ["C X"] = "C Y",
            ["C Y"] = "C Z",
            ["C Z"] = "C X",
        };

        public Day02() : base("./Inputs/Day02.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByNewlines()
                .Select(x => strategy1[x])
                .Sum()
                .ToString();

        }

        public override string Part2()
        {
            return Input
                .SplitByNewlines()
                .Select(x => strategy1[strategy2[x]])
                .Sum()
                .ToString();
        }
    }
}
