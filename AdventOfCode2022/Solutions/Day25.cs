using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day25 : SolutionBase
    {
        private const long Base = 5L;
        private readonly Dictionary<char, long> Symbols = new Dictionary<char, long>
        {
            ['2'] = 2L,
            ['1'] = 1L,
            ['0'] = 0L,
            ['-'] = -1L,
            ['='] = -2L,
        };

        public Day25() : base("./Inputs/Day25.txt")
        {
        }

        public override string Part1()
        {
            var res = Input.SplitByNewlines()
                .Select(FromSnafu)
                .Sum();
            var resS = ToSnafu(res);
            return resS.TrimStart('0');
        }

        private long FromSnafu(string s)
        {
            return s.Reverse()
                .Aggregate(
                (val: 0L, rank: 1L),
                (aggr, c) => (aggr.val + aggr.rank * Symbols[c], aggr.rank * Base))
                .val;
        }

        private string ToSnafu(long x, long? _rank = null)
        {
            var rank = _rank ?? (long)(Math.Pow(Base, Math.Ceiling(Math.Log(x, Base))));
            var sb = new StringBuilder();
            while (rank > 0)
            {
                if (x > rank * 2.5)
                {
                    throw new NotImplementedException();
                }
                else if (x > rank * 1.5)
                {
                    sb.Append('2');
                    x -= rank * 2;
                }
                else if (x >= rank * 0.5)
                {
                    sb.Append('1');
                    x -= rank;
                }
                else if (x > 0)
                {
                    sb.Append("0");
                }
                else
                {
                    sb.Append(Inverse(ToSnafu(-x, rank)));
                    break;
                }
                rank /= Base;
            }
            return sb.ToString();
        }

        private string Inverse(string s)
        {
            return string.Join("",
            s.Select(
                x => x switch
                {

                    '2' => '=',
                    '1' => '-',
                    '0' => '0',
                    '-' => '1',
                    '=' => '2',
                    _ => throw new NotImplementedException()
                }));
        }

        public override string Part2()
        {
            return "";
        }
    }
}
