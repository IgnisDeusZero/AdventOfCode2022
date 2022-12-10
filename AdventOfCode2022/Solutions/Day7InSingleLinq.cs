using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day7InSingleLinq : SolutionBase
    {
        public Day7InSingleLinq() : base("./Inputs/Day7.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "*")
                .Replace("\n", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(
                new
                {
                    CurrentDirectory = new[] { "/" },
                    DirectoryFileSizes = new Dictionary<string, long>() { ["/"] = 0 }
                },
                (state, cmd) =>
                {
                    if (cmd.StartsWith("$ cd "))
                    {
                        state.CurrentDirectory[0] = cmd switch
                        {
                            "$ cd /" => "/",
                            "$ cd .." when state.CurrentDirectory[0] != "/" => state.CurrentDirectory[0].Substring(0, state.CurrentDirectory[0].LastIndexOf("/")),
                            _ => $"{state.CurrentDirectory[0]}/{cmd.Replace("$ cd ", "")}"
                        };
                        if (!state.DirectoryFileSizes.ContainsKey(state.CurrentDirectory[0]))
                        {
                            state.DirectoryFileSizes[state.CurrentDirectory[0]] = 0;
                        }
                    }

                    if (char.IsDigit(cmd[0]))
                    {
                        var filesize = int.Parse(cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                        state.DirectoryFileSizes
                        .Where(x => state.CurrentDirectory[0].StartsWith(x.Key))
                        .ToArray()
                        .Select(x => state.DirectoryFileSizes[x.Key] += filesize)
                        .ToArray();
                    }
                    return state;
                })
                .DirectoryFileSizes
                .Where(x => x.Value < 100000)
                .Sum(x => x.Value)
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .Replace("\r\n", "*")
                .Replace("\n", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(
                new
                {
                    CurrentDirectory = new[] { "/" },
                    DirectoryFileSizes = new Dictionary<string, long>() { ["/"] = 0 }
                },
                (state, cmd) =>
                {
                    if (cmd.StartsWith("$ cd "))
                    {
                        state.CurrentDirectory[0] = cmd switch
                        {
                            "$ cd /" => "/",
                            "$ cd .." when state.CurrentDirectory[0] != "/" => state.CurrentDirectory[0].Substring(0, state.CurrentDirectory[0].LastIndexOf("/")),
                            _ => $"{state.CurrentDirectory[0]}/{cmd.Replace("$ cd ", "")}"
                        };
                        if (!state.DirectoryFileSizes.ContainsKey(state.CurrentDirectory[0]))
                        {
                            state.DirectoryFileSizes[state.CurrentDirectory[0]] = 0;
                        }
                    }

                    if (char.IsDigit(cmd[0]))
                    {
                        var filesize = int.Parse(cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                        state.DirectoryFileSizes
                        .Where(x => state.CurrentDirectory[0].StartsWith(x.Key))
                        .ToArray()
                        .Select(x => state.DirectoryFileSizes[x.Key] += filesize)
                        .ToArray();
                    }
                    return state;
                })
                .DirectoryFileSizes
                .Select(x => x.Value)
                .OrderByDescending(x => x)
                .Aggregate(
                    new[] { 0L, 70_000_000L },
                    (state, size) => new[] { Math.Max(state[0], size), Math.Max(state[0], size) - 40_000_000 < size ? size : state[1] }
                    //{
                    //    if (state[0] == 0)
                    //    {
                    //        state[0] = size;
                    //    }
                    //    if (state[0] - 40_000_000 < size)
                    //    {
                    //        state[1] = size;
                    //    }
                    //    return state;
                    //}
                    )[1]
                    .ToString();

             
                
        }
    }
}
