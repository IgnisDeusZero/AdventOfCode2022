using System.Linq;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day20 : SolutionBase
    {
        public Day20() : base("./Inputs/Day20.txt")
        {
        }

        public override string Part1()
        {
            var nodes = Input.SplitByNewlines()
                .Select(long.Parse)
                .Select(x => new Node { Value = x })
                .ToArray();
            Node zeroNode = null;
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i].Next = nodes[(i + 1) % nodes.Length];
                nodes[i].Prev = nodes[(i - 1 + nodes.Length) % nodes.Length];
                if (nodes[i].Value == 0)
                {
                    zeroNode = nodes[i];
                }
            }
            Shuffle(nodes);
            var curNode = zeroNode;
            var result = 0L;
            for (var moves = 0; moves < 3; moves++)
            {
                for (var i = 0; i < 1000; i++)
                {
                    curNode = curNode.Next;
                }
                result += curNode.Value;
            }

            return result.ToString();
        }

        public override string Part2()
        {
            var nodes = Input.SplitByNewlines()
                .Select(long.Parse)
                .Select(x => new Node { Value = x * 811_589_153L })
                .ToArray();
            Node zeroNode = null;
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i].Next = nodes[(i + 1) % nodes.Length];
                nodes[i].Prev = nodes[(i - 1 + nodes.Length) % nodes.Length];
                if (nodes[i].Value == 0)
                {
                    zeroNode = nodes[i];
                }
            }
            for (var i = 0; i < 10; i++)
            {
                Shuffle(nodes);
            }
            var curNode = zeroNode;
            var result = 0L;
            for (var moves = 0; moves < 3; moves++)
            {
                for (var i = 0; i < 1000; i++)
                {
                    curNode = curNode.Next;
                }
                result += curNode.Value;
            }

            return result.ToString();
        }

        private void Shuffle(Node[] nodes)
        {
            for (var i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Value == 0 || nodes[i].Value % (nodes.Length - 1) == 0)
                {
                    continue;
                }
                nodes[i].Prev.Next = nodes[i].Next;
                nodes[i].Next.Prev = nodes[i].Prev;

                Node nodeInsertAfter = null;

                if (nodes[i].Value < 0)
                {
                    nodeInsertAfter = nodes[i].Prev;
                    for (var m = 0; m < -nodes[i].Value % (nodes.LongLength - 1); m++)
                    {
                        nodeInsertAfter = nodeInsertAfter.Prev;
                    }
                }
                else
                {
                    nodeInsertAfter = nodes[i];
                    for (var m = 0; m < nodes[i].Value % (nodes.LongLength - 1); m++)
                    {
                        nodeInsertAfter = nodeInsertAfter.Next;
                    }
                }

                nodes[i].Prev = nodeInsertAfter;
                nodes[i].Next = nodeInsertAfter.Next;
                nodes[i].Prev.Next = nodes[i];
                nodes[i].Next.Prev = nodes[i];
            }
        }

        public class Node
        {
            public Node Prev { get; set; }
            public Node Next { get; set; }
            public long Value { get; set; }

            public override string ToString()
            {
                var node = Next;
                var sb = new StringBuilder($"{Value}");
                while (node != this)
                {
                    sb.Append($" {node.Value}");
                    node = node.Next;
                }
                return sb.ToString();
            }
        }
    }
}
