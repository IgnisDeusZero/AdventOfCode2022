using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day16 : SolutionBase
    {
        Regex regex = new Regex(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w\s,]+)");

        public Day16() : base("./Inputs/Day16.txt")
        {
        }

        public override string Part1()
        {
            var input = Input.SplitByNewlines()
                .Select(x => regex.Match(x))
                .Select(x => (Valve: x.Groups[1].Value, Flow: int.Parse(x.Groups[2].Value), ConnectedValves: x.Groups[3].Value.Split(", ").ToList()))
                .ToList();
            var valves = input.Select((x, i) => (x.Valve, i, x.Flow)).ToDictionary(x => x.Valve, x => (Index: x.i, Flow: x.Flow));
            var graph = new int[valves.Count, valves.Count];
            input.ForEach(x =>
            {
                x.ConnectedValves.ForEach(cv =>
                {
                    graph[valves[x.Valve].Index, valves[cv].Index] = 1;
                });
            });
            var dists = GitDists(valves, graph);

            int flowed = 0;
            Bruteforce(
                dists,
                valves.OrderBy(x => x.Value.Index).Select(x => x.Value.Flow).ToArray(),
                new bool[valves.Count],
                0,
                30,
                0,
                valves["AA"].Index,
                ref flowed);

            return flowed.ToString();
        }
        public override string Part2()
        {
            var input = Input.SplitByNewlines()
                .Select(x => regex.Match(x))
                .Select(x => (Valve: x.Groups[1].Value, Flow: int.Parse(x.Groups[2].Value), ConnectedValves: x.Groups[3].Value.Split(", ").ToList()))
                .ToList();
            var valves = input.Select((x, i) => (x.Valve, i, x.Flow)).ToDictionary(x => x.Valve, x => (Index: x.i, Flow: x.Flow));
            var graph = new int[valves.Count, valves.Count];
            input.ForEach(x =>
            {
                x.ConnectedValves.ForEach(cv =>
                {
                    graph[valves[x.Valve].Index, valves[cv].Index] = 1;
                });
            });
            var dists = GitDists(valves, graph);

            int flowed = 0;
            Bruteforce2(
                dists,
                valves.OrderBy(x => x.Value.Index).Select(x => x.Value.Flow).ToArray(),
                new bool[valves.Count],
                0,
                0,
                0,
                26,
                0,
                valves["AA"].Index,
                valves["AA"].Index,
                false,
                false,
                ref flowed);

            return flowed.ToString();
        }

        private void Bruteforce2(
            int[,] dists,
            int[] flows,
            bool[] visited,
            int totalFlowInThisMoment,
            int eTimeToOpenValve,
            int mTimeToOpenValve,
            int stepsLeftInThisMoment,
            int flowedInThisMoment,
            int eValveHeadingTo,
            int mValveHeadingTo,
            bool eOpenValve,
            bool mOpenValve,
            ref int maxFlowed)
        {
            if (eTimeToOpenValve == 0 && !eOpenValve)
            {
                totalFlowInThisMoment += flows[eValveHeadingTo];
                eOpenValve = true;
            }
            if (mTimeToOpenValve == 0 && !mOpenValve)
            {
                totalFlowInThisMoment += flows[mValveHeadingTo];
                mOpenValve = true;
            }

            if (mTimeToOpenValve == 0)
            {
                var hasValvesToHeading = false;
                for (var i = 0; i < flows.Length; i++)
                {
                    if (flows[i] == 0 || visited[i] || dists[mValveHeadingTo, i] + 1 > stepsLeftInThisMoment)
                    {
                        continue;
                    }
                    hasValvesToHeading = true;
                    visited[i] = true;
                    var timePass = Math.Min(dists[mValveHeadingTo, i] + 1, eTimeToOpenValve);
                    Bruteforce2(
                        dists,
                        flows,
                        visited,
                        totalFlowInThisMoment,
                        eTimeToOpenValve - timePass,
                        (dists[mValveHeadingTo, i] + 1) - timePass,
                        stepsLeftInThisMoment - timePass,
                        flowedInThisMoment + (timePass) * totalFlowInThisMoment,
                        eValveHeadingTo,
                        i,
                        eOpenValve,
                        false,
                        ref maxFlowed);
                    visited[i] = false;
                }
                if (!hasValvesToHeading)
                {
                    var timePass = eTimeToOpenValve;
                    Bruteforce2(
                        dists,
                        flows,
                        visited,
                        totalFlowInThisMoment,
                        eTimeToOpenValve - timePass,
                        int.MaxValue,
                        stepsLeftInThisMoment - timePass,
                        flowedInThisMoment + (timePass) * totalFlowInThisMoment,
                        eValveHeadingTo,
                        mValveHeadingTo,
                        eOpenValve,
                        mOpenValve,
                        ref maxFlowed);
                }
            }
            else if (eTimeToOpenValve == 0)
            {
                var hasValvesToHeading = false;
                for (var i = 0; i < flows.Length; i++)
                {
                    if (flows[i] == 0 || visited[i] || dists[eValveHeadingTo, i] + 1 > stepsLeftInThisMoment)
                    {
                        continue;
                    }
                    visited[i] = true;
                    hasValvesToHeading = true;
                    var timePass = Math.Min(dists[eValveHeadingTo, i] + 1, mTimeToOpenValve);
                    Bruteforce2(
                        dists,
                        flows,
                        visited,
                        totalFlowInThisMoment,
                        (dists[eValveHeadingTo, i] + 1) - timePass,
                        mTimeToOpenValve - timePass,
                        stepsLeftInThisMoment - timePass,
                        flowedInThisMoment + (timePass) * totalFlowInThisMoment,
                        i,
                        mValveHeadingTo,
                        false,
                        mOpenValve,
                        ref maxFlowed);
                    visited[i] = false;
                }
                if (!hasValvesToHeading)
                {
                    var timePass = eTimeToOpenValve;
                    Bruteforce2(
                        dists,
                        flows,
                        visited,
                        totalFlowInThisMoment,
                        int.MaxValue,
                        mTimeToOpenValve - timePass,
                        stepsLeftInThisMoment - timePass,
                        flowedInThisMoment + (timePass) * totalFlowInThisMoment,
                        eValveHeadingTo,
                        mValveHeadingTo,
                        eOpenValve,
                        mOpenValve,
                        ref maxFlowed);
                }
            }

            maxFlowed = Math.Max(maxFlowed, flowedInThisMoment + totalFlowInThisMoment * stepsLeftInThisMoment);
        }

        private void Bruteforce(
            int[,] dists,
            int[] flows,
            bool[] visited,
            int totalFlow,
            int stepsLeft,
            int flowed,
            int node,
            ref int maxFlowed)
        {
            for (var i = 0; i < flows.Length; i++)
            {
                if (flows[i] == 0 || visited[i] || dists[node, i] + 1 > stepsLeft)
                {
                    continue;
                }
                visited[i] = true;
                Bruteforce(
                    dists,
                    flows,
                    visited,
                    totalFlow + flows[i],
                    stepsLeft - (dists[node, i] + 1),
                    flowed + (dists[node, i] + 1) * totalFlow,
                    i,
                    ref maxFlowed);
                visited[i] = false;
            }

            maxFlowed = Math.Max(maxFlowed, flowed + totalFlow * stepsLeft);
        }

        private static int[,] GitDists(Dictionary<string, (int Index, int Flow)> valves, int[,] graph)
        {
            var dists = new int[valves.Count, valves.Count];
            for (var i = 0; i < valves.Count; i++)
            {
                for (var j = 0; j < valves.Count; j++)
                {
                    if (i == j)
                    {
                        dists[i, j] = 0;
                    }
                    else if (graph[i, j] == 1)
                    {
                        dists[i, j] = 1;
                    }
                    else
                    {
                        dists[i, j] = byte.MaxValue;
                    }
                }
            }

            for (var k = 0; k < valves.Count; k++)
            {
                for (var i = 0; i < valves.Count; i++)
                {
                    for (var j = 0; j < valves.Count; j++)
                    {
                        dists[i, j] = Math.Min(dists[i, j], dists[i, k] + dists[k, j]);
                    }
                }
            }

            return dists;
        }

    }
}
