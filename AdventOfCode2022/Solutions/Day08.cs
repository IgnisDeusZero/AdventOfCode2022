using System;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day08 : SolutionBase
    {
        public Day08() : base("./Inputs/Day08.txt")
        {
        }

        public override string Part1()
        {
            var forest = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();
            var visibilityFromRight = GetVisibilityFromRight(forest);
            var visibilityFromLeft = GetVisibilityFromLeft(forest);
            var visibilityFromUp = GetVisibilityFromUp(forest);
            var visibilityFromDown = GetVisibilityFromDown(forest);
            var visibleTree = 0;
            for (var i = 0; i < forest.Length; i++)
            {
                for (var j = 0; j < forest[i].Length; j++)
                {
                    if (i == 0 || j == 0 || i == forest.Length - 1 || j == forest[i].Length - 1
                        ||forest[i][j] > visibilityFromRight[i][j]
                        || forest[i][j] > visibilityFromRight[i][j]
                        || forest[i][j] > visibilityFromLeft[i][j]
                        || forest[i][j] > visibilityFromUp[i][j]
                        || forest[i][j] > visibilityFromDown[i][j])
                    {
                        visibleTree++;
                    }
                }
            }

            return visibleTree.ToString();
        }

        private static int[][] GetVisibilityFromUp(int[][] forest)
        {
            var visibilityFromUp = forest
                .Select(x => new int[x.Length])
                .ToArray();
            for (var i = 1; i < forest.Length; i++)
            {
                for (var j = 0; j < forest[i].Length; j++)
                {
                    visibilityFromUp[i][j] = Math.Max(visibilityFromUp[i - 1][j], forest[i - 1][j]);
                }
            }

            return visibilityFromUp;
        }

        private static int[][] GetVisibilityFromDown(int[][] forest)
        {
            var visibilityFromdown = forest
                .Select(x => new int[x.Length])
                .ToArray();
            for (var i = forest.Length - 2; i >= 0; i--)
            {
                for (var j = 0; j < forest[i].Length; j++)
                {
                    visibilityFromdown[i][j] = Math.Max(visibilityFromdown[i + 1][j], forest[i + 1][j]);
                }
            }

            return visibilityFromdown;
        }

        private static int[][] GetVisibilityFromLeft(int[][] forest)
        {
            var visibilityFromLeft = forest
                .Select(x => new int[x.Length])
                .ToArray();
            for (var i = 0; i < forest.Length; i++)
            {
                for (var j = forest[i].Length - 2; j >= 0; j--)
                {
                    visibilityFromLeft[i][j] = Math.Max(visibilityFromLeft[i][j + 1], forest[i][j + 1]);
                }
            }

            return visibilityFromLeft;
        }

        private static int[][] GetVisibilityFromRight(int[][] forest)
        {
            var visibilityFromRight = forest
                            .Select(x => new int[x.Length])
                            .ToArray();
            for (var i = 0; i < forest.Length; i++)
            {
                for (var j = 1; j < forest[i].Length; j++)
                {
                    visibilityFromRight[i][j] = Math.Max(visibilityFromRight[i][j - 1], forest[i][j - 1]);
                }
            }

            return visibilityFromRight;
        }

        public override string Part2()
        {
            var forest = Input
                .Replace("\r\n", "/")
                .Replace("\n", "/")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();
            var scores = forest
                .Select((row, rowIndex) => row.Select((_, colIndex) => ScenicScore(rowIndex, colIndex, forest)).ToArray())
                .ToArray();

            return scores.SelectMany(x => x).Max().ToString();
        }

        private int ScenicScore(int row, int col, int[][] forest)
        {
            var len = forest[row][col];
            var visibles = new int[4];
            for (var i = row - 1; i >= 0; i--)
            {
                visibles[0]++;
                if (forest[i][col] >= len)
                    break;
            }
            for (var i = row + 1; i < forest.Length; i++)
            {
                visibles[1]++;
                if (forest[i][col] >= len)
                    break;
            }
            for (var i = col - 1; i >= 0; i--)
            {
                visibles[2]++;
                if (forest[row][i] >= len)
                    break;
            }
            for (var i = col + 1; i < forest[row].Length; i++)
            {
                visibles[3]++;
                if (forest[row][i] >= len)
                    break;
            }
            return visibles.Aggregate((a, b) => a * b);
        }

    }
}
