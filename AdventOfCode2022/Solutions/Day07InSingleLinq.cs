using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day07InSingleLinq : SolutionBase
    {
        public Day07InSingleLinq() : base("./Inputs/Day07.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByNewlines()
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
                            "$ cd .." when state.CurrentDirectory[0] != "/" => state.CurrentDirectory[0][0..state.CurrentDirectory[0].LastIndexOf("/")],
                            _ => $"{state.CurrentDirectory[0]}/{cmd.Replace("$ cd ", "")}"
                        };
                        if (!state.DirectoryFileSizes.ContainsKey(state.CurrentDirectory[0]))
                        {
                            state.DirectoryFileSizes[state.CurrentDirectory[0]] = 0;
                        }
                    }

                    if (char.IsDigit(cmd[0]))
                    {
                        var filesize = int.Parse(cmd.Split(' ' , StringSplitOptions.RemoveEmptyEntries)[0]);
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
                .SplitByNewlines()
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
                            "$ cd .." when state.CurrentDirectory[0] != "/" => state.CurrentDirectory[0][0..state.CurrentDirectory[0].LastIndexOf("/")],
                            _ => $"{state.CurrentDirectory[0]}/{cmd.Replace("$ cd ", "")}"
                        };
                        if (!state.DirectoryFileSizes.ContainsKey(state.CurrentDirectory[0]))
                        {
                            state.DirectoryFileSizes[state.CurrentDirectory[0]] = 0;
                        }
                    }

                    if (char.IsDigit(cmd[0]))
                    {
                        var filesize = int.Parse(cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
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
                    (state, size) => new[] 
                    { 
                        Math.Max(state[0], size), 
                        Math.Max(state[0], size) - 40_000_000 < size ? size : state[1] 
                    })[1]
                    .ToString();

             
                
        }
    }
}
