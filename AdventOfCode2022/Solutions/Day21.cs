using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode2022.Solutions
{
    public class Day21 : SolutionBase
    {
        public Day21() : base("./Inputs/Day21.txt")
        {
        }

        public override string Part1()
        {
            var monkeys = Input.SplitByNewlines()
                .Select(x => x.Split(": "))
                .ToDictionary(x => x[0], x => x[1]);

            return Parse(monkeys, monkeys["root"])
                .ToString();
        }

        private long Parse(
            Dictionary<string, string> md,
            string v)
        {
            if (v.All(char.IsDigit))
            {
                return long.Parse(v);
            }
            return v[5] switch
            {
                '+' => Parse(md, md[v[0..4]]) + Parse(md, md[v[7..11]]),
                '-' => Parse(md, md[v[0..4]]) - Parse(md, md[v[7..11]]),
                '*' => Parse(md, md[v[0..4]]) * Parse(md, md[v[7..11]]),
                '/' => Parse(md, md[v[0..4]]) / Parse(md, md[v[7..11]]),
                _ => throw new NotImplementedException()
            };
        }

        public override string Part2()
        {
            var monkeys = Input.SplitByNewlines()
                .Select(x => x.Split(": "))
                .ToDictionary(x => x[0], x => x[1]);
            monkeys["humn"] = "humn";

            var arg = Expression.Parameter(typeof(long), "humn");
            var e1 = Parse2(monkeys, monkeys[monkeys["root"][0..4]], arg);
            var e2 = Parse2(monkeys, monkeys[monkeys["root"][7..11]], arg);

            if (e1 is ConstantExpression ce1)
            {
                return Transfer((long)ce1.Value, e2).ToString();
            }
            else if (e2 is ConstantExpression ce2)
            {
                return Transfer((long)ce2.Value, e1).ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private Expression Parse2(
            Dictionary<string, string> md,
            string v,
            Expression arg)
        {
            if (v.All(char.IsDigit))
            {
                return Expression.Constant(long.Parse(v), typeof(long));
            }

            if (v == "humn")
            {
                return arg;
            }

            var left = Parse2(md, md[v[0..4]], arg);
            var right = Parse2(md, md[v[7..11]], arg);

            if (left is ConstantExpression lce && right is ConstantExpression rce)
            {
                var value = v[5] switch
                {
                    '+' => (long)lce.Value + (long)rce.Value,
                    '-' => (long)lce.Value - (long)rce.Value,
                    '*' => (long)lce.Value * (long)rce.Value,
                    '/' => (long)lce.Value / (long)rce.Value,
                    _ => throw new NotImplementedException()
                };
                return Expression.Constant(value, typeof(long));
            }

            var expression = v[5] switch
            {
                '+' => Expression.Add(left, right),
                '-' => Expression.Subtract(left, right),
                '*' => Expression.Multiply(left, right),
                '/' => Expression.Divide(left, right),
                _ => throw new NotImplementedException()
            };

            return expression;
        }

        private long Transfer(long value, Expression func)
        {
            if (func is ParameterExpression)
            {
                return value;
            }

            if (func is not BinaryExpression be)
            {
                throw new InvalidOperationException();
            }
            if (be.Right is ConstantExpression rce)
            {
                return be.NodeType switch
                {
                    ExpressionType.Add => Transfer(value - (long)rce.Value, be.Left),
                    ExpressionType.Subtract => Transfer(value + (long)rce.Value, be.Left),
                    ExpressionType.Multiply => Transfer(value / (long)rce.Value, be.Left),
                    ExpressionType.Divide => Transfer(value * (long)rce.Value, be.Left),
                    _ => throw new NotImplementedException()
                };
            }
            if (be.Left is ConstantExpression lce)
            {
                return be.NodeType switch
                {
                    ExpressionType.Add => Transfer(value - (long)lce.Value, be.Right),
                    ExpressionType.Subtract => Transfer((long)lce.Value - value, be.Right),
                    ExpressionType.Multiply => Transfer(value / (long)lce.Value, be.Right),
                    ExpressionType.Divide => Transfer((long)lce.Value / value, be.Right),
                    _ => throw new NotImplementedException()
                };
            }
            throw new NotImplementedException();
        }
    }
}
