using AdventOfCode2022.Models;
using System.Linq;

namespace AdventOfCode2022.Solutions.Day17Models
{
    public class Stone
    {
        public LongPoint[] Points { get; }
        public long Width { get; }
        public long Height { get; }
        public Stone(params LongPoint[] points)
        {
            Points = points;
            Width = points.Max(x => x.X) - points.Min(x => x.X) + 1;
            Height = points.Max(x => x.Y) - points.Min(x => x.Y) + 1;
        }
    }
}
