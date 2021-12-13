using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PassagePathing
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();
            var caves = LoadInput(lines);

            var paths = new List<List<Cave>>();

            MapPaths(caves.FirstOrDefault(c => c.Name.Equals("start", StringComparison.InvariantCultureIgnoreCase)),
                new List<Cave>(), paths, new Cave());

            Console.WriteLine("Number of Paths: " + paths.Count());

            paths.Clear();

            MapPaths2(caves.FirstOrDefault(c => c.Name.Equals("start", StringComparison.InvariantCultureIgnoreCase)),
                new List<Cave>(), paths, new Cave());

            Console.WriteLine("Number of Paths for Part 2: " + paths.Count());

            // foreach (var path in paths){
            //     Console.WriteLine(string.Join(',', path.Select(c => c.Name).ToArray()));
            // }
        }

        static void MapPaths(Cave current, List<Cave> currentPath, List<List<Cave>> fullPaths, Cave Previous){
            currentPath.Add(current);

            if(current.Name.Equals("end", StringComparison.InvariantCultureIgnoreCase)){
                fullPaths.Add(currentPath);
                return;
            }

            for (int i = 0; i < current.CanAccess.Count(); i++){
                var next = current.CanAccess[i];

                if(next.IsSmallCave && currentPath.Contains(next)) continue;
                //if(next.Name.Equals(Previous.Name, StringComparison.InvariantCultureIgnoreCase)) continue;
                if(next.Name.Equals("start", StringComparison.InvariantCultureIgnoreCase)) continue;
                MapPaths(next, ClonePath(currentPath), fullPaths, current);

            }

        }

        static void MapPaths2(Cave current, List<Cave> currentPath, List<List<Cave>> fullPaths, Cave Previous){
            currentPath.Add(current);

            if(current.Name.Equals("end", StringComparison.InvariantCultureIgnoreCase)){
                fullPaths.Add(currentPath);
                return;
            }

            for (int i = 0; i < current.CanAccess.Count(); i++){
                var next = current.CanAccess[i];

                if(next.IsSmallCave && currentPath.Contains(next) && currentPath.Any(b => currentPath.Count(c => c == b) > 1 && b.IsSmallCave)) continue;
                //if(next.Name.Equals(Previous.Name, StringComparison.InvariantCultureIgnoreCase)) continue;
                if(next.Name.Equals("start", StringComparison.InvariantCultureIgnoreCase)) continue;
                MapPaths2(next, ClonePath(currentPath), fullPaths, current);

            }

        }

        static List<Cave> LoadInput(string[] lines){
            var caves = new List<Cave>();
            foreach(string path in lines){
                var parts = path.Split('-');
                if(!caves.Any(c => c.Name.Equals(parts[0], StringComparison.InvariantCultureIgnoreCase))){
                    caves.Add(new Cave(){Name = parts[0]});
                }

                if(!caves.Any(c => c.Name.Equals(parts[1], StringComparison.InvariantCultureIgnoreCase))){
                    caves.Add(new Cave(){Name = parts[1]});
                }

                var cave1 = caves.FirstOrDefault(c => c.Name.Equals(parts[0], StringComparison.InvariantCultureIgnoreCase));
                var cave2 = caves.FirstOrDefault(c => c.Name.Equals(parts[1], StringComparison.InvariantCultureIgnoreCase));

                cave1.CanAccess.Add(cave2);
                cave2.CanAccess.Add(cave1);
            }
            return caves;
        }

        static List<Cave> ClonePath(List<Cave> path){
            return path.Select<Cave, Cave>(c => c).ToList();
        }
    }

    class Cave {
        public string Name {get;set;}
        public bool IsSmallCave{
            get{
                return !char.IsUpper(Name[0]);    
            }}
        public List<Cave> CanAccess{get;set;} = new List<Cave>();

        // public object Clone()
        // {
        //     return new Cave(){
        //         Name = this.Name,
        //         CanAccess = this.CanAccess
        //     };
        // }
    }

    class Path : List<Path>, ICloneable
    {
        public object Clone()
        {
            return this.Select(p => p);
        }
    }
}
