using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day13 : SolutionBase, IComparer<object>
    {
        public Day13() : base("./Inputs/Day13.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByDoubleNewlines()
                .Select(pair => pair.SplitByNewlines().Select(Parse).ToArray())
                .Select((pair, index) => (index + 1, Check(pair[0], pair[1])))
                .Where(x => x.Item2.Value)
                .Select(x => x.Item1)
                .Aggregate(0, (a, b) => a + b)
                .ToString();
        }

        public override string Part2()
        {
            var delims = new object[]
            {
                new object[] {new object[] {2}},
                new object[] {new object[] {6}},
            };
            return Input
                .SplitByDoubleNewlines()
                .SelectMany(pair => pair.SplitByNewlines().Select(Parse))
                .Concat(delims)
                .OrderBy(x => x, this)
                .Select((obj, i) => (obj, ind: i+1))
                .Where(x => delims.Contains(x.obj))
                .Select(x => x.ind)
                .Aggregate((a, b) => a * b)
                .ToString();
        }

        private object Parse(string s)
        {
            var objects = new List<object>();
            for (var i = 1; i < s.Length; i++)
            {
                if (s[i] == '[')
                {
                    var mbi = i;
                    var depth = 1;
                    while (depth != 0)
                    {
                        mbi++;
                        depth += s[mbi] switch
                        {
                            '[' => 1,
                            ']' => -1,
                            _ => 0
                        };
                    }
                    objects.Add(Parse(s[i..(mbi + 1)]));
                    i = mbi;
                }
                else if (char.IsDigit(s[i]))
                {
                    var ci = i + 1;
                    while (char.IsDigit(s[ci]))
                    {
                        ci++;
                    }
                    objects.Add(int.Parse(s[i..ci]));
                    i = ci;
                }
            }
            return objects.ToArray();
        }

        private bool? Check(object obj1, object obj2)
        {
            if (obj1 is int v1 && obj2 is int v2)
            {
                if (v1 == v2)
                {
                    return null;
                }
                return v1 < v2;
            }
            if (obj1 is object[] arr1 && obj2 is object[] arr2)
            {
                var cmp = arr1.Zip(
                            arr2,
                            (item1, item2) => Check(item1, item2))
                        .FirstOrDefault(x => x != null);
                if (cmp != null)
                {
                    return cmp;
                }
                return (arr1.Length - arr2.Length) switch
                {
                    int diff when diff < 0 => true,
                    int diff when diff > 0 => false,
                    _ => (bool?)null
                };
            }
            if (obj1 is object[])
            {
                return Check(obj1, new object[] { obj2 });
            }
            if (obj2 is object[])
            {
                return Check(new object[] { obj1 }, obj2);
            }
            return null;
        }

        public int Compare(object x, object y)
        {
            return Check(x, y) switch
            {
                true => -1,
                false => 1,
                _ => 0
            };
        }
    }
}
