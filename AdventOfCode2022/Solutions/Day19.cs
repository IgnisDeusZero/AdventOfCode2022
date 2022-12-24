using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    public class Day19 : SolutionBase
    {
        public Day19() : base("./Inputs/Day19.txt")
        {
        }

        public override string Part1()
        {
            return Input.SplitByNewlines()
                .Select(Blueprint.Parse)
                .Select(b =>
                {
                    var geodes = MaximumGeodesCanGetByBlueprint(b, 24);
                    return geodes * b.Id;
                })
                .Sum()
                .ToString();
        }

        private int MaximumGeodesCanGetByBlueprint(Blueprint blueprint, int time)
        {
            var resources = new int[5];
            var robots = new int[5];
            robots[1] = 1;
            var maxGeodes = new int[time + 1];
            var builtRobots = new int[time + 1];
            BruteforceV2(blueprint, resources, robots, time, maxGeodes, builtRobots);
            Console.WriteLine(maxGeodes[0]);
            return maxGeodes[0];
        }

        private void Bruteforce(
            Blueprint blueprint, 
            int[] resources,
            int[] robots,
            int buildingRobot,
            int timeLeft,
            int[] maxGeodes)
        {
            if (timeLeft == 0)
            {
                maxGeodes[0] = Math.Max(maxGeodes[0], resources[4]);
                //variants++;
                return;
            }
            if (maxGeodes[timeLeft-1] > resources[4] + robots[4] + 1)
            {
                //variants += (ulong)Math.Pow(5, timeLeft);
                return;
            }

            robots[buildingRobot]++;

            var canAffordRobot = new bool[5]
            {
                true,
                blueprint.Robots[1][1] <= resources[1],
                blueprint.Robots[2][1] <= resources[1],
                blueprint.Robots[3][1] <= resources[1] && blueprint.Robots[3][2] <= resources[2],
                blueprint.Robots[4][1] <= resources[1] && blueprint.Robots[4][3] <= resources[3],

            };

            for (var i = 1; i <= 4; i++)
            {
                resources[i] += robots[i];
            }


            for (var rob = 4; rob >= 0; rob--)
            {
                if (!canAffordRobot[rob])
                {
                    continue;
                }
                var robotCost = blueprint.Robots[rob];
                for (var res = 1; res <= 4; res++)
                {
                    resources[res] -= robotCost[res];
                }

                Bruteforce(blueprint, resources, robots, rob, timeLeft - 1, maxGeodes);


                for (var res = 1; res <= 4; res++)
                {
                    resources[res] += robotCost[res];
                }
            }

            for (var i = 1; i <= 4; i++)
            {
                resources[i] -= robots[i];
            }

            maxGeodes[timeLeft] = Math.Max(maxGeodes[timeLeft], resources[4]);

            robots[buildingRobot]--;
        }


        private void BruteforceV2(
            Blueprint blueprint,
            int[] resources,
            int[] robots,
            int timeLeft,
            int[] maxGeodes,
            int[] builtRobots)
        {
            for (var t = timeLeft - 1; t >= 0; t--)
            {
                maxGeodes[t] = Math.Max(maxGeodes[t], resources[4] + robots[4] * (timeLeft - t));
            }

            if (maxGeodes[timeLeft - 1] > resources[4] + robots[4] + 1)
            {
                return;
            }

            for (var rob = 4; rob > 0; rob--)
            {

                var resLack = new[]
                {
                    0,
                    resources[1] - blueprint.Robots[rob][1],
                    resources[2] - blueprint.Robots[rob][2],
                    resources[3] - blueprint.Robots[rob][3]
                };
                var canBuild = !(resLack[2] < 0 && robots[2] == 0) && !(resLack[3] < 0 && robots[3] == 0);
                if (!canBuild)
                {
                    continue;
                }

                var timeWaitForBuild = TimeWaitForResources(robots, resLack) + 1;

                if (timeWaitForBuild >= timeLeft)
                {
                    continue;
                }

                for (var res = 1; res <= 4; res++)
                {
                    resources[res] += robots[res] * timeWaitForBuild;
                    resources[res] -= blueprint.Robots[rob][res];
                }
                robots[rob]++;
                builtRobots[timeLeft - timeWaitForBuild] = rob;
                BruteforceV2(
                    blueprint,
                    resources,
                    robots,
                    timeLeft - timeWaitForBuild,
                    maxGeodes,
                    builtRobots);


                builtRobots[timeLeft - timeWaitForBuild] = 0;
                robots[rob]--;
                for (var res = 1; res <= 4; res++)
                {
                    resources[res] -= robots[res] * timeWaitForBuild;
                    resources[res] += blueprint.Robots[rob][res];
                }
            }
        }

        private static int TimeWaitForResources(int[] robots, int[] resLack)
        {
            var timeWaitForRes = 0;
            if (resLack[1] < 0)
            {
                timeWaitForRes = Math.Max(timeWaitForRes, (int)Math.Ceiling((double)-resLack[1] / robots[1]));
            }
            if (resLack[2] < 0)
            {
                timeWaitForRes = Math.Max(timeWaitForRes, (int)Math.Ceiling((double)-resLack[2] / robots[2]));
            }
            if (resLack[3] < 0)
            {
                timeWaitForRes = Math.Max(timeWaitForRes, (int)Math.Ceiling((double)-resLack[3] / robots[3]));
            }

            return timeWaitForRes;
        }

        public override string Part2()
        {
            return Input.SplitByNewlines()
                .Take(3)
                .Select(Blueprint.Parse)
                .Select(b => MaximumGeodesCanGetByBlueprint(b, 32))
                .Aggregate((a,b) => a*b)
                .ToString();
        }

        public class Blueprint
        {
            private static readonly Regex regex = 
                new Regex(@"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");

            public int Id { get; private set; }
            public int[][] Robots { get; private set; }

            public static Blueprint Parse(string x)
            {
                var match = regex.Match(x);
                var id = int.Parse(match.Groups[1].Value);
                var oreCostOre = int.Parse(match.Groups[2].Value);
                var clayCostOre = int.Parse(match.Groups[3].Value);
                var obsidianCostOre = int.Parse(match.Groups[4].Value);
                var obsidianCostClay = int.Parse(match.Groups[5].Value);
                var geodeCostOre = int.Parse(match.Groups[6].Value);
                var geodeCostObsidian = int.Parse(match.Groups[7].Value);

                return new Blueprint()
                {
                    Id = id,
                    Robots = new[]
                    {
                        new[] { 0, 0, 0, 0, 0},
                        new[] { 0, oreCostOre, 0, 0 ,0 },
                        new[] { 0, clayCostOre, 0, 0 ,0 },
                        new[] { 0, obsidianCostOre, obsidianCostClay, 0 ,0 },
                        new[] { 0, geodeCostOre, 0, geodeCostObsidian, 0 }
                    }
                };
            }
        }
    }
}
