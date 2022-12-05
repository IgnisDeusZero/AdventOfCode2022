using AdventOfCode2022.Solutions;
using System;
using System.Linq;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            var solvers = typeof(Program).Assembly
                .GetTypes()
                .Where(x => typeof(SolutionBase).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => x.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>()) as SolutionBase);

            foreach (var solver in solvers)
            {
                Console.WriteLine(solver.GetType().Name);
                Console.WriteLine(Try(solver.Part1));
                Console.WriteLine(Try(solver.Part2));
                Console.WriteLine();
            }
        }

        private static string Try(Func<string> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return $"ERROR!!! {e.Message}";
            }
        }
    }
}
