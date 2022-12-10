using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Solutions
{
    public class Day7 : SolutionBase
    {
        public Day7() : base("./Inputs/Day7.txt")
        {
        }

        public override string Part1()
        {
            return Input
                .Replace("\r\n", "*")
                .Replace("\n", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(
                new Tree("/", null),
                (t, s) => Apply(t, s))
                .Move("/")
                .GetDirSizes()
                .Where(x => x.Item2 < 100000)
                .Sum(x => x.Item2)
                .ToString();
        }

        public override string Part2()
        {
            var dirSizes = Input
                .Replace("\r\n", "*")
                .Replace("\n", "*")
                .Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(
                new Tree("/", null),
                (t, s) => Apply(t, s))
                .Move("/")
                .GetDirSizes()
                .ToArray();
            var volume = 70000000;
            var freeSpaceNeed = 30000000;
            var spaceUsed = dirSizes.Single(ds => ds.Item1 == "/").Item2;
            var freeSpace = volume - spaceUsed;
            var needToFree = freeSpaceNeed - freeSpace;
            return dirSizes.Where(x => x.Item2 > needToFree).Select(x=>x.Item2).Min().ToString();
        }

        private Tree Apply(Tree t, string s)
        {
            if (s.StartsWith("$ cd "))
            {
                return t.Move(s.Remove(0, 5));
            }
            if (s.StartsWith("dir "))
            {
                t.AddDirectory(s.Remove(0, 4));
                return t;
            }
            if (char.IsDigit(s[0]))
            {
                var fileMeta = s.Split(new[] { ' ' });

                t.AddFile(fileMeta[1], long.Parse(fileMeta[0]));
                return t;
            }
            return t;
        }
    }

    public class Tree
    {
        public Tree Parent;
        public string DirName;
        public List<(string FileName, long Size)> Files = new List<(string FileName, long Size)>();
        public List<Tree> SubDirs = new List<Tree>();

        public Tree(string dirName, Tree parent)
        {
            Parent = parent;
            DirName = dirName;
        }

        public void AddFile(string filename, long filesize)
        {
            if (Files.Any(x => x.FileName == filename))
            {
                return;
            }
            Files.Add((filename, filesize));
        }

        public void AddDirectory(string dirName)
        {
            if (SubDirs.Any(x=>x.DirName == dirName))
            {
                return;
            }
            SubDirs.Add(new Tree(dirName, this));
        }

        public Tree Move(string dirName)
        {
            if (dirName == "..")
            {
                if (Parent == null)
                {
                    throw new Exception("Move above root");
                }
                return Parent;
            }   
            if (dirName == "/")
            {
                var t = this;
                while (t.Parent != null)
                {
                    t = t.Parent;
                }
                return t;
            }
            var directory = SubDirs.SingleOrDefault(sd => sd.DirName == dirName);
            if (directory != null)
            {
                return directory;
            }
            AddDirectory(dirName);
            return Move(dirName);
        }

        public IEnumerable<(string, long)> GetDirSizes()
        {
            var subDirsSizes = SubDirs.SelectMany(x => x.GetDirSizes()).ToArray();
            var subDirsSizesSum = SubDirs.Sum(sd => subDirsSizes.Single(sds => sds.Item1 == sd.GetAbsoluteName()).Item2);
            var filesSizesSum = Files.Sum(x => x.Size);
            yield return (GetAbsoluteName(), subDirsSizesSum + filesSizesSum);
            foreach (var subDirsSize in subDirsSizes)
            {
                yield return subDirsSize;
            }
        }

        private string GetAbsoluteName()
        {
            var t = this;
            var absoluteName = DirName;
            while (t.Parent != null)
            {
                t = t.Parent;
                absoluteName = $"{t.DirName}/{absoluteName}";
            }
            return absoluteName;
        }
    }
}
