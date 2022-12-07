﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day6 : SolutionBase
    {
        public Day6() : base("./Inputs/Day6.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Skip(4)
                .Aggregate(
                (Pos: 5, Window: Input.Substring(0,4), FirstMarkerPos: default(int?)),
                (state, c) => (
                state.Pos + 1,
                state.Window[1..] + c,
                (state.Window[1..] + c).Distinct().Count() == 4
                    ? state.FirstMarkerPos ?? state.Pos
                    : state.FirstMarkerPos)
                    )
                .Item3
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .Skip(14)
                .Aggregate(
                (Pos: 15, Window: Input.Substring(0, 14), FirstMarkerPos: default(int?)),
                (state, c) => (
                state.Pos + 1,
                state.Window[1..] + c,
                (state.Window[1..] + c).Distinct().Count() == 14
                    ? state.FirstMarkerPos ?? state.Pos
                    : state.FirstMarkerPos)
                    )
                .Item3
                .ToString();
        }
    }
}
