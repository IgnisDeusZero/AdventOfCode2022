using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day05 : SolutionBase
    {
        public Day05() : base("./Inputs/Day05.txt")
        {
        }

        public override string Part1()
        {
            var inputs = Input.SplitByDoubleNewlines();
            var wh = Warehouse.Parse(inputs[0]);
            var moves = inputs[1].SplitByNewlines();
            foreach (var move in moves)
            {
                wh.Move(move);
            }
            return string.Join("", wh.Stacks.Select(x => x.Peek()));
        }

        public override string Part2()
        {
            var inputs = Input.SplitByDoubleNewlines();
            var wh = Warehouse.Parse(inputs[0]);
            var moves = inputs[1].SplitByNewlines();
            foreach (var move in moves)
            {
                wh.Move(move, true);
            }
            return string.Join("", wh.Stacks.Select(x => x.Peek()));
        }

        public class Warehouse
        {
            public Stack<char>[] Stacks;
            private static readonly Regex regex = new Regex(@"move (\d+) from (\d+) to (\d+)");

            public static Warehouse Parse(string input)
            {
                var wh = new Warehouse
                {
                    Stacks = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray()
                };

                foreach (var row in input.SplitByNewlines().Reverse().Skip(1))
                {
                    for (var i = 0; i < 9; i++)
                    {
                        var c = row[i * 4 + 1];
                        if (c == ' ')
                        {
                            continue;
                        }
                        wh.Stacks[i].Push(c);
                    }
                }

                return wh;
            }

            public void MoveV1(int from, int to, int count)
            {
                for (var i = 0; i < count; i++)
                {
                    if (Stacks[from].Count == 0)
                    {
                        break;
                    }
                    Stacks[to].Push(Stacks[from].Pop());
                }
            }

            public void MoveV2(int from, int to, int count)
            {
                var intermediateStack = new Stack<char>();
                for (var i = 0; i < count; i++)
                {
                    if (Stacks[from].Count == 0)
                    {
                        break;
                    }
                    intermediateStack.Push(Stacks[from].Pop());
                }
                for (var i = 0; i < count; i++)
                {
                    if (intermediateStack.Count == 0)
                    {
                        break;
                    }
                    Stacks[to].Push(intermediateStack.Pop());
                }
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
