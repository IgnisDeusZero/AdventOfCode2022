using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day5InSingleLinq : SolutionBase
    {
        public Day5InSingleLinq() : base("./Inputs/Day5.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(x => char.IsDigit(x[0])).Select(int.Parse).ToArray())
                .Aggregate(
                    Input
                        .Replace("\r\n", "/")
                        .Replace("\n", "/")
                        .Replace("//", "*")
                        .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)[0]
                        .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                        .Reverse()
                        .Skip(1)
                        .SelectMany((row, rowIndex) => Enumerable.Range(0, 9).Select(i => (rowIndex, i, row[i * 4 + 1])))
                        .GroupBy(x => x.i)
                        .Select(x => x.OrderBy(x => x.rowIndex).Select(x => x.Item3).Where(x => !char.IsWhiteSpace(x)).ToList())
                        .ToArray(),
                    (lists, arr) =>
                    {
                        var fromIndex = Math.Max(0, lists[arr[1] - 1].Count - arr[0]);
                        var count = lists[arr[1] - 1].Count - fromIndex;
                        lists[arr[2] - 1].AddRange(lists[arr[1] - 1].GetRange(fromIndex, count).Reverse<char>());
                        lists[arr[1] - 1].RemoveRange(fromIndex, count);
                        return lists;
                    })
                .ToArray()
                .Select(x => x.Last())
                .Aggregate("", (s, c) => s + c);
        }

        public override string Part2()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(x => char.IsDigit(x[0])).Select(int.Parse).ToArray())
                .Aggregate(
                    Input
                        .Replace("\r\n", "/")
                        .Replace("\n", "/")
                        .Replace("//", "*")
                        .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)[0]
                        .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                        .Reverse()
                        .Skip(1)
                        .SelectMany((row, rowIndex) => Enumerable.Range(0, 9).Select(i => (rowIndex, i, row[i * 4 + 1])))
                        .GroupBy(x => x.i)
                        .Select(x => x.OrderBy(x => x.rowIndex).Select(x => x.Item3).Where(x => !char.IsWhiteSpace(x)).ToList())
                        .ToArray(),
                    (lists, arr) =>
                    {
                        var fromIndex = Math.Max(0, lists[arr[1] - 1].Count - arr[0]);
                        var count = lists[arr[1] - 1].Count - fromIndex;
                        lists[arr[2] - 1].AddRange(lists[arr[1] - 1].GetRange(fromIndex, count));
                        lists[arr[1] - 1].RemoveRange(fromIndex, count);
                        return lists;
                    })
                .ToArray()
                .Select(x => x.Last())
                .Aggregate("", (s, c) => s + c);
        }
    }
}
