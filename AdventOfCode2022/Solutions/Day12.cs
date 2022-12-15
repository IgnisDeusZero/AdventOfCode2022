using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode2022.Solutions
{
    public class Day12 : SolutionBase
    {
        public Day12() : base("./Inputs/Day12.txt")
        {
        }

        public override string Part1()
        {
            var start = (0, 0, 0);
            var finish = (0, 0);
            var map = Input
                .SplitByNewlines()
                .Select((row, rowIndex) => row
                    .Select<char, int>((ch, colIndex) => (ch switch
                    {
                        'S' => (Func<int>)(() => { start = (rowIndex, colIndex, 0); return 0; }),
                        'E' => (Func<int>)(() => { finish = (rowIndex, colIndex); return 27; }),
                        char x => (Func<int>) (() => x - 'a' + 1)
                    })())
                    .ToArray())
                .ToArray();

            var availableNodes = new List<(int Row, int Col, int Len)>() { start };
            var shortestPathToNode = map.Select(row => row.Select(_ => int.MaxValue).ToArray()).ToArray();
            var neighborOffsets = new (int Row, int Col)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            while (availableNodes.Count > 0)
            {
                var curNode = availableNodes.OrderBy(x => x.Len).First();
                availableNodes.Remove(curNode);
                neighborOffsets
                    .Select(n => (n.Row + curNode.Row, n.Col + curNode.Col, curNode.Len + 1))
                    .Where(n => n.Item1 >= 0 && n.Item1 < map.Length && n.Item2 >= 0 && n.Item2 < map[n.Item1].Length)
                    .Where(n => map[n.Item1][n.Item2] - map[curNode.Row][curNode.Col] <= 1)
                    .Where(n => shortestPathToNode[n.Item1][n.Item2] > n.Item3)
                    .ToList()
                    .ForEach(n =>
                    {
                        shortestPathToNode[n.Item1][n.Item2] = n.Item3;
                        availableNodes.Add(n);
                    });

            }
            return shortestPathToNode[finish.Item1][finish.Item2].ToString();
        }

        public override string Part2()
        {
            var starts = new List<(int, int, int)>();
            var finish = (0, 0);
            var map = Input
                .SplitByNewlines()
                .Select((row, rowIndex) => row
                    .Select<char, int>((ch, colIndex) => (ch switch
                    {
                        'S' => (Func<int>)(() => { return 0; }),
                        'E' => (Func<int>)(() => { finish = (rowIndex, colIndex); return 27; }),
                        'a' => (Func<int>)(() => { starts.Add((rowIndex, colIndex, 0)); return 1; }),
                        char x => (Func<int>)(() => x - 'a' + 1)
                    })())
                    .ToArray())
                .ToArray();


            return starts.Select(start =>
                {
                    var availableNodes = new List<(int Row, int Col, int Len)>() { start };
                    var shortestPathToNode = map.Select(row => row.Select(_ => int.MaxValue).ToArray()).ToArray();
                    var neighborOffsets = new (int Row, int Col)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
                    while (availableNodes.Count > 0)
                    {
                        var curNode = availableNodes.OrderBy(x => x.Len).First();
                        availableNodes.Remove(curNode);
                        neighborOffsets
                            .Select(n => (n.Row + curNode.Row, n.Col + curNode.Col, curNode.Len + 1))
                            .Where(n => n.Item1 >= 0 && n.Item1 < map.Length && n.Item2 >= 0 && n.Item2 < map[n.Item1].Length)
                            .Where(n => map[n.Item1][n.Item2] - map[curNode.Row][curNode.Col] <= 1)
                            .Where(n => shortestPathToNode[n.Item1][n.Item2] > n.Item3)
                            .ToList()
                            .ForEach(n =>
                            {
                                shortestPathToNode[n.Item1][n.Item2] = n.Item3;
                                availableNodes.Add(n);
                            });

                    }
                    return shortestPathToNode[finish.Item1][finish.Item2];
                })
                .Min()
                .ToString();
        }
    }
}
