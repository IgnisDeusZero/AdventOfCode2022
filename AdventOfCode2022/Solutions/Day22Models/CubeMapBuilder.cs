using AdventOfCode2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions.Day22Models
{
    public class CubeMapBuilder
    {

        public static CubeMap Parse(string s)
        {
            var map = new CubeMap();
            map.Sweep = s.SplitByNewlines()
                .Select(row => row.Select(x => x switch { ' ' => 0, '.' => 1, '#' => 2, _ => throw new NotImplementedException() }).ToArray())
                .ToArray();

            map.EdgeSize = map.Sweep.Select(line => line.Length)
                .Distinct()
                .Append(map.Sweep.Length)
                .Aggregate(GCD);

            map.SweepSchema = CalculateSweepSchema(map);

            map.Edges = CalculateEdges(map);

            map.EdgeTransitions = CalculateTransitions(map)
              .Select(x => x.Cast<(int ArriveEdge, int ArriveDir)>().ToArray())
              .ToArray();

            return map;
        }

        private static int[][,] CalculateEdges(CubeMap map)
        {
            var edges = new Dictionary<int, int[,]>();
            for (var row = 0; row < CubeMap.SweepSchemaSize; row++)
            {
                for (var col = 0; col < CubeMap.SweepSchemaSize; col++)
                {
                    if (map.SweepSchema[row,col] == 0)
                    {
                        continue;
                    }
                    var edge = new int[map.EdgeSize, map.EdgeSize];

                    for (var eRow = 0; eRow < map.EdgeSize; eRow++)
                    {
                        for (var eCol = 0; eCol < map.EdgeSize; eCol++)
                        {
                            edge[eRow, eCol] = map.Sweep[row * map.EdgeSize + eRow][col * map.EdgeSize + eCol];
                        }
                    }

                    edges[map.SweepSchema[row, col] - 1] = edge;
                }
            }
            return edges.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
        }

        private static int[,] CalculateSweepSchema(CubeMap map)
        {
            var sweepSchema = new int[CubeMap.SweepSchemaSize, CubeMap.SweepSchemaSize];
            var edgeNum = 1;
            for (var row = 0; row < CubeMap.SweepSchemaSize; row++)
            {
                for (var col = 0; col < CubeMap.SweepSchemaSize; col++)
                {
                    if (map.Sweep.Length <= row * map.EdgeSize || map.Sweep[row * map.EdgeSize].Length <= col * map.EdgeSize)
                    {
                        continue;
                    }
                    sweepSchema[row, col] = map.Sweep[row * map.EdgeSize][col * map.EdgeSize] != 0 ? edgeNum++ : 0;
                }
            }

            return sweepSchema;
        }

        private static (int ArriveEdge, int ArriveDir)?[][] CalculateTransitions(CubeMap map)
        {
            (int ArriveEdge, int ArriveDir)?[][] transitions = Enumerable.Range(0, 6)
                .Select(edge => Enumerable.Range(0, 4).Select(gotodir => ((int, int)?)null).ToArray())
                .ToArray();

            var valuesToFill = 6 * 4;
            var valuesFilled = 0;

            for (var row = 0; row < CubeMap.SweepSchemaSize; row++)
            {
                for (var col = 0; col < CubeMap.SweepSchemaSize; col++)
                {
                    if (map.SweepSchema[row, col] == 0)
                    {
                        continue;
                    }
                    var nbgs = new (int rowOffset, int colOffset, int dir)[] { (0, 1, 0), (1, 0, 1), (0, -1, 2), (-1, 0, 3) }
                        .Select(x => (row: x.rowOffset + row, col: x.colOffset + col, x.dir))
                        .Where(x => x.col >= 0 && x.row >= 0)
                        .Where(x => x.col < CubeMap.SweepSchemaSize && x.row < CubeMap.SweepSchemaSize)
                        .Where(x => map.SweepSchema[x.row, x.col] != 0)
                        .ToArray();
                    foreach (var nbg in nbgs)
                    {
                        transitions[map.SweepSchema[row, col] - 1][nbg.dir] = (map.SweepSchema[nbg.row, nbg.col] - 1, (nbg.dir + 2) % 4);
                        valuesFilled++;
                    }
                }
            }
            while (valuesFilled != valuesToFill)
            {
                for (var fromEdge = 0; fromEdge < 6; fromEdge++)
                {
                    for (var gotoDir = 0; gotoDir < 4; gotoDir++)
                    {
                        var curEdgeCurDir = transitions[fromEdge][gotoDir];
                        var curEdgeNextDir = transitions[fromEdge][(gotoDir + 1) % 4];
                        var curEdgePrevDir = transitions[fromEdge][(gotoDir + 3) % 4];

                        if (curEdgeCurDir.HasValue && curEdgeNextDir.HasValue)
                        {
                            var arriveDirOffset = (curEdgeCurDir.Value.ArriveDir - gotoDir + 6) % 4;
                            var newEdge = curEdgeCurDir.Value.ArriveEdge;
                            var newGotoDir = (gotoDir + 1 + arriveDirOffset) % 4;
                            var newArriveEdge = curEdgeNextDir.Value.ArriveEdge;
                            var newArriveDir = (curEdgeNextDir.Value.ArriveDir + 1) % 4;
                            if (!transitions[newEdge][newGotoDir].HasValue)
                            {
                                valuesFilled++;
                                transitions[newEdge][newGotoDir] = (newArriveEdge, newArriveDir);
                            }
                        }

                        if (curEdgeCurDir.HasValue && curEdgePrevDir.HasValue)
                        {
                            var arriveDirOffset = (curEdgeCurDir.Value.ArriveDir - gotoDir + 6) % 4;
                            var newEdge = curEdgeCurDir.Value.ArriveEdge;
                            var newGotoDir = (gotoDir + 3 + arriveDirOffset) % 4;
                            var newArriveEdge = curEdgePrevDir.Value.ArriveEdge;
                            var newArriveDir = (curEdgePrevDir.Value.ArriveDir + 3) % 4;
                            if (!transitions[newEdge][newGotoDir].HasValue)
                            {
                                valuesFilled++;
                                transitions[newEdge][newGotoDir] = (newArriveEdge, newArriveDir);
                            }
                        }
                    }
                }
            }


            return transitions;
        }

        private static int GCD(int a, int b)
        {
            if (a % b == 0)
            {
                return b;
            }
            return GCD(b, a % b);
        }
    }
}
