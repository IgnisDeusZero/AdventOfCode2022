using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day24 : SolutionBase
    {
        public Day24() : base("./Inputs/Day24.txt")
        {
        }
        private List<char> Winds = new List<char> { '>', 'v', '<', '^' };

        public override string Part1()
        {
            var inputLines = Input.SplitByNewlines();

            var width = inputLines[0].Length;
            var height = inputLines.Length;
            var depth = (width - 2) * (height - 2);

            var initState = inputLines
                .Reverse()
                .SelectMany((line, y) => line.Select((c, x) => (x, y, w: Winds.IndexOf(c))).Where(x => x.w != -1))
                .ToArray();

            if (initState.Any(w => (w.x == 1 || w.x == width - 2) && (w.w == 1 || w.w == 3)))
            {
                throw new NotImplementedException();
            }

            var states = CalcStates(width, height, depth, initState).ToArray();
            int t = BFS(states, (1, height - 1, 0), (width - 2, 0), width, height);

            return t.ToString();
        }

        public override string Part2()
        {
            var inputLines = Input.SplitByNewlines();

            var width = inputLines[0].Length;
            var height = inputLines.Length;
            var depth = (width - 2) * (height - 2);

            var initState = inputLines
                .Reverse()
                .SelectMany((line, y) => line.Select((c, x) => (x, y, w: Winds.IndexOf(c))).Where(x => x.w != -1))
                .ToArray();

            if (initState.Any(w => (w.x == 1 || w.x == width - 2) && (w.w == 1 || w.w == 3)))
            {
                throw new NotImplementedException();
            }

            var states = CalcStates(width, height, depth, initState).ToArray();
            var start = (x: 1, y: height - 1);
            var end = (x: width - 2, y: 0);
            int t1 = BFS(states, (start.x, start.y, 0), end, width, height);
            var t2 = BFS(states, (end.x, end.y, t1), start, width, height);
            int t3 = BFS(states, (start.x, start.y, t2), end, width, height);

            return t3.ToString();
        }

        private (int x, int y, int d)[] nbgs = new[]
        {
            (0, 0, 1),
            (1, 0, 1),
            (0, 1, 1),
            (-1, 0, 1),
            (0, -1, 1)
        };

        private int BFS(
            (int x, int y, int w)[][] states, 
            (int x, int y, int d) start, 
            (int x, int y) target, 
            int width, 
            int height)
        {
            var queue = new Queue<(int x, int y, int d)>();
            queue.Enqueue(start);
            var enqueued = new HashSet<(int x, int y, int d)>();
            while (queue.Count > 0)
            {
                var (x, y, d) = queue.Dequeue();
                if (x == target.x && y == target.y)
                {
                    return d;
                }
                var nextPoints = nbgs.Select(nbg => (x: x + nbg.x, y: y + nbg.y, d: d + nbg.d))
                    .Where(nextPos => ValidPosition(nextPos.x, nextPos.y, width, height))
                    .Where(nextPos => FreeSpace(nextPos, states))
                    .Where(nextPos => !enqueued.Contains(nextPos));
                foreach (var np in nextPoints)
                {
                    queue.Enqueue(np);
                    enqueued.Add(np);
                }
            }
            throw new NotImplementedException();
        }

        private bool FreeSpace((int x, int y, int d) nextPos, (int x, int y, int w)[][] states)
        {
            return !states[nextPos.d % states.Length].Any(x => x.x == nextPos.x && x.y == nextPos.y);
        }

        private bool ValidPosition(int x, int y, int width, int height)
        {
            if ((x == 1 && y == height - 1) || (x == width - 2 && y == 0))
            {
                return true;
            }
            if (x <= 0 || y <= 0 || x >= width - 1 || y >= height - 1)
            {
                return false;
            }
            return true;
        }

        private IEnumerable<(int x, int y, int w)[]> CalcStates(int width, int height, int depth, (int x, int y, int w)[] state)
        {
            yield return state;
            for (var d = 0; d < depth; d++)
            {
                state = state.Select(x => NextPosition(x, width, height)).ToArray();
                yield return state;
            }
        }

        private (int x, int y, int w) NextPosition((int x, int y, int w) wind, int width, int height)
        {
            var x = wind.w switch
            {
                0 => wind.x == width - 2 ? 1 : wind.x + 1,
                2 => wind.x == 1 ? width - 2 : wind.x - 1,
                _ => wind.x
            };
            var y = wind.w switch
            {
                1 => wind.y == 1 ? height - 2 : wind.y - 1,
                3 => wind.y == height - 2 ? 1 : wind.y + 1,
                _ => wind.y
            };

            return (x, y, wind.w);
        }
    }
}
