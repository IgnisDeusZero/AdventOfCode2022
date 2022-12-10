using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day05UsingList : SolutionBase
    {
        public Day05UsingList() : base("./Inputs/Day05.txt")
        {
        }

        public override string Part1()
        {
            var inputs = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            var wh = Warehouse.Parse(inputs[0]);
            var moves = inputs[1].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var move in moves)
            {
                wh.Move(move);
            }
            return string.Join("", wh.Lists.Select(x => x.Last()));
        }

        public override string Part2()
        {
            var inputs = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            var wh = Warehouse.Parse(inputs[0]);
            var moves = inputs[1].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var move in moves)
            {
                wh.Move(move, true);
            }
            return string.Join("", wh.Lists.Select(x => x.Last()));
        }

        public class Warehouse
        {
            public List<char>[] Lists;
            private static readonly Regex regex = new Regex(@"move (\d+) from (\d+) to (\d+)");

            public static Warehouse Parse(string input)
            {
                return new Warehouse
                {
                    Lists = input
                    .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Reverse()
                    .Skip(1)
                    .SelectMany((row, rowIndex) => Enumerable.Range(0, 9).Select(i => (rowIndex, i, row[i * 4 + 1])))
                    .GroupBy(x => x.i)
                    .Select(x => x.OrderBy(x => x.rowIndex).Select(x => x.Item3).Where(x => !char.IsWhiteSpace(x)).ToList())
                    .ToArray()
                };
            }

            public void MoveV1(int from, int to, int count)
            {
                var fromIndex = Math.Max(0, Lists[from].Count - count);
                count = Lists[from].Count - fromIndex;
                Lists[to].AddRange(Lists[from].GetRange(fromIndex, count).Reverse<char>());
                Lists[from].RemoveRange(fromIndex, count);
            }

            public void MoveV2(int from, int to, int count)
            {
                var fromIndex = Math.Max(0, Lists[from].Count - count);
                count = Lists[from].Count - fromIndex;
                Lists[to].AddRange(Lists[from].GetRange(fromIndex, count));
                Lists[from].RemoveRange(fromIndex, count);
            }

            public void Move(string s, bool isV2 = false)
            {
                var match = regex.Match(s);
                var from = int.Parse(match.Groups[2].Value) - 1;
                var to = int.Parse(match.Groups[3].Value) - 1;
                var count = int.Parse(match.Groups[1].Value);
                if (isV2)
                {
                    MoveV2(from, to, count);
                }
                else
                {
                    MoveV1(from, to, count);
                }
            }
        }
    }
}
