using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ReactorReboot
{
    public static partial class Reactor
    {
        public static void Reboot(Instruction[] instructions){
            var Core = new List<Tuple<Cube, int>>();
            //Core.Add(new Tuple<Cube, int>(instructions[0].Cube, instructions[0].On ? 1 : -1));

            foreach(var instruction in instructions){
                var toAdd = new List<Tuple<Cube, int>>();
                if(instruction.On) toAdd.Add(new Tuple<Cube, int>(instruction.Cube, 1));
                foreach(var piece in Core){
                    var intersect = Intersect(instruction.Cube, piece.Item1);
                    if(intersect != null) toAdd.Add(new Tuple<Cube, int>(intersect, -piece.Item2));
                }
                Core.AddRange(toAdd);
            }

            var on = Core.Sum(t => Volume(t.Item1) * t.Item2);

            Console.WriteLine("Lights on Full Reboot: " + on);
        }

        static Cube Intersect(Cube cube1, Cube cube2){
            var intersect = new Cube(Math.Max(cube1.xMin, cube2.xMin), Math.Min(cube1.xMax, cube2.xMax),
                                        Math.Max(cube1.yMin, cube2.yMin), Math.Min(cube1.yMax, cube2.yMax),
                                        Math.Max(cube1.zMin, cube2.zMin), Math.Min(cube1.zMax, cube2.zMax));

            if(intersect.xMin > intersect.xMax || intersect.yMin > intersect.yMax || intersect.zMin > intersect.zMax)
                return null;

            return intersect;
        }   

        static long Volume(Cube cube){
            var height = cube.xMax - cube.xMin + 1L;
            var width = cube.yMax - cube.yMin + 1L;
            var depth = cube.zMax - cube.zMin + 1L;

            return height * width * depth;
        }
    }
}

