using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day09 : SolutionBase
    {
        public Day09() : base("Inputs/Day09.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => (
                Direction: x[0] switch
                {
                    "R" => (Row: 0, Col: 1),
                    "L" => (Row: 0, Col: -1),
                    "U" => (Row: -1, Col: 0),
                    "D" => (Row: 1, Col: 0),
                    _ => throw new NotImplementedException()
                },
                Moves: int.Parse(x[1])))
                .Aggregate(
                    new
                    {
                        Positions = new[] { 0, 0, 0, 0 },
                        TailHistory = new HashSet<(int Row, int Col)>() { (0, 0) }
                    },
                    (state, command) =>
                    {
                        for (var i = 0; i < command.Moves; i++)
                        {
                            state.Positions[0] = state.Positions[0] + command.Direction.Row;
                            state.Positions[1] = state.Positions[1] + command.Direction.Col;

                            var offsetRow = state.Positions[0] - state.Positions[2];
                            var offsetCol = state.Positions[1] - state.Positions[3];
                            if (Math.Abs(offsetCol) <= 1 && Math.Abs(offsetRow) <= 1)
                            {
                                continue;
                            }
                            state.Positions[2] += (int)Math.Round(offsetRow / 2.0, MidpointRounding.AwayFromZero);
                            state.Positions[3] += (int)Math.Round(offsetCol / 2.0, MidpointRounding.AwayFromZero);
                            state.TailHistory.Add((state.Positions[2], state.Positions[3]));
                        }
                        return state;
                    }
                )
                .TailHistory
                .Count
                .ToString();
        }

        public override string Part2()
        {
            const int ropeLength = 10;
            return Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => 
                    (
                        Direction: x[0] switch
                        {
                            "R" => (Row: 0, Col: 1),
                            "L" => (Row: 0, Col: -1),
                            "U" => (Row: -1, Col: 0),
                            "D" => (Row: 1, Col: 0),
                            _ => throw new NotImplementedException()
                        },
                        Moves: int.Parse(x[1])
                    ))
                .Aggregate(
                    new
                    {
                        Positions = new int[20],
                        TailHistory = new HashSet<(int Row, int Col)>() { (0, 0) }
                    },
                    (state, command) =>
                    {
                        Enumerable
                            .Range(0, command.Moves)
                            .ToList()
                            .ForEach(_ =>
                            {

                                state.Positions[0] = state.Positions[0] + command.Direction.Row;
                                state.Positions[1] = state.Positions[1] + command.Direction.Col;
                                Enumerable
                                    .Range(1, ropeLength - 1)
                                    .ToList()
                                    .ForEach(ropePart =>
                                        {
                                            var headRowIndex = ropePart * 2 - 2;
                                            var headColIndex = ropePart * 2 - 1;
                                            var tailRowIndex = ropePart * 2 + 0;
                                            var tailColIndex = ropePart * 2 + 1;
                                            var offsetRow = state.Positions[headRowIndex] - state.Positions[tailRowIndex];
                                            var offsetCol = state.Positions[headColIndex] - state.Positions[tailColIndex];
                                            if (Math.Abs(offsetCol) > 1 || Math.Abs(offsetRow) > 1)
                                            {
                                                state.Positions[tailRowIndex] += (int)Math.Round(offsetRow / 2.0, MidpointRounding.AwayFromZero);
                                                state.Positions[tailColIndex] += (int)Math.Round(offsetCol / 2.0, MidpointRounding.AwayFromZero);
                                            }
                                        });
                                state.TailHistory.Add((state.Positions[ropeLength * 2 - 2], state.Positions[ropeLength * 2 - 1]));
                            });
                        return state;
                    }
                )
                .TailHistory
                .Count
                .ToString();
        }
    }
}
