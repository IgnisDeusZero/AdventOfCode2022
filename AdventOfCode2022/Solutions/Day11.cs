using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode2022.Solutions
{
    public class Day11 : SolutionBase
    {
        public Day11() : base("./Inputs/Day11.txt")
        {
        }

        public override string Part1()
        {
            var monkeys = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "`")
                .Split(new[] { '`' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                .Select(inp => new
                {
                    Index = int.Parse(inp[0].Replace(":", "")[7..]),
                    Items = new List<int>(inp[1][18..].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)),
                    Operation = ParseOperation(inp[2]),
                    Divisor = int.Parse(inp[3][20..]),
                    IfDivisible = int.Parse(inp[4][29..]),
                    IfNonDivisible = int.Parse(inp[5][30..]),
                    ItemsInspected = new int[1]
                })
                .ToList();

            Enumerable.Range(0, 20)
                .ToList()
                .ForEach(_ =>
                {
                    monkeys.ForEach(monkey =>
                    {
                        monkey.Items.ForEach(item =>
                        {
                            var newItem = monkey.Operation(item) / 3;
                            var newMonkeyIndex = newItem % monkey.Divisor == 0 ? monkey.IfDivisible : monkey.IfNonDivisible;
                            monkeys[newMonkeyIndex].Items.Add(newItem);
                        });
                        monkey.ItemsInspected[0] += monkey.Items.Count;
                        monkey.Items.Clear();
                    });
                });
            return monkeys
                .Select(x => x.ItemsInspected[0])
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((a, b) => a * b)
                .ToString();
        }

        public override string Part2()
        {
            var monkeys = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Replace("//", "`")
                .Split(new[] { '`' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                .Select(inp => new
                {
                    Index = int.Parse(inp[0].Replace(":", "")[7..]),
                    Items = new List<ulong>(inp[1][18..].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse)),
                    Operation = ParseOperationAsUnsignedLong(inp[2]),
                    Divisor = int.Parse(inp[3][20..]),
                    IfDivisible = int.Parse(inp[4][29..]),
                    IfNonDivisible = int.Parse(inp[5][30..]),
                    ItemsInspected = new int[1]
                })
                .ToList();

            var commonDivisor = monkeys.Select(x => x.Divisor).Aggregate(1UL, (a, b) => a * (ulong)b);
            var rounds = Enumerable.Range(1, 10).Select(x => x * 1000).Concat(new[] { 1, 20 }).ToArray();
            Enumerable.Range(0, 10000)
                .ToList()
                .ForEach(_ =>
                {
                    monkeys.ForEach(monkey =>
                    {
                        monkey.Items.ForEach(item =>
                        {
                            var newItem = monkey.Operation(item);
                            var newMonkeyIndex = newItem % (ulong)monkey.Divisor == 0 ? monkey.IfDivisible : monkey.IfNonDivisible;
                            monkeys[newMonkeyIndex].Items.Add(newItem % commonDivisor);
                        });
                        monkey.ItemsInspected[0] += monkey.Items.Count;
                        monkey.Items.Clear();
                    });
                });
            return monkeys
                .Select(x => x.ItemsInspected[0])
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate(1UL, (a, b) => a * (ulong)b)
                .ToString();
        }

        private Func<int, int> ParseOperation(string s)
        {
            var x = s[19..].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var arg = Expression.Parameter(typeof(int), "old");
            var operand1 = x[0] == "old" ? (Expression)arg : Expression.Constant(int.Parse(x[0]));
            var operand2 = x[2] == "old" ? (Expression)arg : Expression.Constant(int.Parse(x[2]));
            var operation = x[1] switch
            {
                "*" => Expression.Multiply(operand1, operand2),
                "+" => Expression.Add(operand1, operand2),
                _ => throw new NotImplementedException()
            };

            return Expression.Lambda<Func<int, int>>(operation, arg).Compile();
        }

        private Func<ulong, ulong> ParseOperationAsUnsignedLong(string s)
        {
            var x = s[19..].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var arg = Expression.Parameter(typeof(ulong), "old");
            var operand1 = x[0] == "old" ? (Expression)arg : Expression.Constant(ulong.Parse(x[0]));
            var operand2 = x[2] == "old" ? (Expression)arg : Expression.Constant(ulong.Parse(x[2]));
            var operation = x[1] switch
            {
                "*" => Expression.Multiply(operand1, operand2),
                "+" => Expression.Add(operand1, operand2),
                _ => throw new NotImplementedException()
            };

            return Expression.Lambda<Func<ulong, ulong>>(operation, arg).Compile();
        }
    }
}
