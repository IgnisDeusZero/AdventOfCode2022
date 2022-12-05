using System.IO;

namespace AdventOfCode2022.Solutions
{
    public abstract class SolutionBase
    {
        protected SolutionBase(string inputFile)
        {
            Input = File.ReadAllText(inputFile);
        }

        protected string Input { get; private set; }

        public abstract string Part1();
        public abstract string Part2();
    }
}
