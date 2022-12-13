using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day10 : SolutionBase
    {
        private readonly int[] cycles = new[] { 20, 60, 100, 140, 180, 220 };
        public Day10() : base("./Inputs/Day10.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .SplitByNewlines()
                .SelectMany(x => x == "noop" ? new[] { 0 } : new[] { 0, int.Parse(x.Remove(0, 5)) })
                .Select((x, i) => (x, i))
                .Aggregate(new[] { 1, 0 }, (sig, c) =>
                {
                    if (cycles.Any(x => x == (c.i + 1)))
                    {
                        sig[1] += sig[0] * (c.i + 1);
                    }
                    sig[0] += c.x;
                    return sig;
                })[1]
                .ToString();
        }

        public override string Part2()
        {
            return Input
                .SplitByNewlines()
                .SelectMany(x => x == "noop" ? new[] { 0 } : new[] { 0, int.Parse(x.Remove(0, 5)) })
                .Select((x, i) => (x, i))
                .Aggregate(new 
                    { 
                        Signal = new[] { 1 }, 
                        Builder = new System.Text.StringBuilder()
                    }, 
                    (state, c) =>
                    {
                        state.Builder.Append(c.i % 40 == 0 ? "\n" : "");
                        state.Builder.Append(Math.Abs(state.Signal[0] - (c.i % 40)) <= 1 ? "#" : ".");
                        state.Signal[0] += c.x;
                        return state;
                    })
                .Builder
                .ToString();
        }
    }
}
