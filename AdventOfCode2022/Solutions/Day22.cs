using AdventOfCode2022.Models;
using AdventOfCode2022.Solutions.Day22Models;
using System;
using System.Data;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day22 : SolutionBase
    {
        public Day22() : base("./Inputs/Day22.txt")
        {
        }

        public override string Part1()
        {
            var input = Input.SplitByDoubleNewlines();
            var map = input[0].SplitByNewlines()
                .Select(row => row.Select(x => x switch { ' ' => 0, '.' => 1, '#' => 2, _ => throw new NotImplementedException() }).ToArray())
                .ToArray();
            var pos = map[0].Select((x, i) => (x, i))
                .Where(x => x.x == 1)
                .Select(x => new Point(x.i, 0))
                .First();
            var dir = 0;
            var steps = 0;
            foreach (var i in input[1] + 'E')
            {
                if (char.IsDigit(i))
                {
                    steps *= 10;
                    steps += int.Parse(i.ToString());
                }
                else
                {
                    pos = Move(map, dir, steps, pos);
                    steps = 0;
                }
                dir = i switch
                {
                    'R' => (dir + 1) % 4,
                    'L' => (dir + 3) % 4,
                    _ => dir
                };
            }
            return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + dir).ToString();
        }

        private Point Move(int[][] map, int dir, int steps, Point pos)
        {
            Func<Point, Point> nextPoint = dir switch
            {
                0 => (Func<Point, Point>)(point => NextXPoint(map, point, 1)),
                1 => (Func<Point, Point>)(point => NextYPoint(map, point, 1)),
                2 => (Func<Point, Point>)(point => NextXPoint(map, point, -1)),
                3 => (Func<Point, Point>)(point => NextYPoint(map, point, -1)),
                _ => throw new NotImplementedException()
            };
            while (steps-- > 0)
            {
                pos = nextPoint(pos);
            }
            return pos;
        }

        private Point NextYPoint(int[][] map, Point point, int direction)
        {
            var y = point.Y;
            do
            {
                y = (y + direction + map.Length) % map.Length;
            } while (map[y].Length <= point.X || map[y][point.X] == 0);
            return map[y][point.X] == 1 ? new Point(point.X, y) : point;
        }

        private Point NextXPoint(int[][] map, Point point, int direction)
        {
            var x = point.X;
            do
            {
                x = (x + direction + map[point.Y].Length) % map[point.Y].Length;
            } while (map[point.Y][x] == 0);
            return map[point.Y][x] == 1 ? new Point(x, point.Y) : point;
        }

        public override string Part2()
        {
            var input = Input.SplitByDoubleNewlines();
            var map = CubeMapBuilder.Parse(input[0]);
            var steps = 0;

            foreach (var i in input[1] + 'E')
            {
                if (char.IsDigit(i))
                {
                    steps *= 10;
                    steps += int.Parse(i.ToString());
                }
                else
                {
                    map.Move(steps);
                    steps = 0;
                }
                map.Direction = i switch
                {
                    'R' => (map.Direction + 1) % 4,
                    'L' => (map.Direction + 3) % 4,
                    _ => map.Direction
                };
            }
            var pos = map.GetAbsolutePosition();
            return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + map.Direction).ToString();
        }
    }
}
