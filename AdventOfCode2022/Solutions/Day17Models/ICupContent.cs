using AdventOfCode2022.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2022.Solutions.Day17Models
{
    public interface ICupContent
    {
        long Width { get; }
        long Height { get; }
        byte this[LongPoint point] { get; set; }
        byte this[long x, long y] { get; set; }
    }
}
