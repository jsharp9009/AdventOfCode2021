using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ReactorReboot
{
    public static partial class Reactor
    {
        public static void Intialize(Instruction[] instructions)
        {
            var lightsOn = new HashSet<Vector3>();

            var count = 0;
            foreach(var instruction in instructions){
                count++;
                Console.Write("\rProcessing " + count + " of " + instructions.Count());

                RunInstruction(instruction, lightsOn);
            }
            
            Console.WriteLine("\nLights On: " + lightsOn.Count());
        }    

        static HashSet<Vector3> RunInstruction(Instruction instruction, HashSet<Vector3> lightsOn){
            if(instruction.Cube.xMax < -50 || instruction.Cube.xMin > 50) return lightsOn;
            if(instruction.Cube.yMax < -50 || instruction.Cube.yMin > 50) return lightsOn;
            if(instruction.Cube.zMax < -50 || instruction.Cube.zMin > 50) return lightsOn;

            var xmin = instruction.Cube.xMin < -50 ? -50 : instruction.Cube.xMin;
            var xmax = instruction.Cube.xMax > 50 ? 50 : instruction.Cube.xMax;

            var ymin = instruction.Cube.yMin < -50 ? -50 : instruction.Cube.yMin;
            var ymax = instruction.Cube.yMax > 50 ? 50 : instruction.Cube.yMax;

            var zmin = instruction.Cube.zMin < -50 ? -50 : instruction.Cube.zMin;
            var zmax = instruction.Cube.zMax > 50 ? 50 : instruction.Cube.zMax;

            for(int x = xmin; x <= xmax; x++){
                for(int y = ymin; y <= ymax; y++){
                    for(int z = zmin; z <= zmax; z++){
                        if(instruction.On){
                        var newOn = new Vector3(x, y, z);
                        if(!lightsOn.Contains(newOn)){
                            lightsOn.Add(newOn);
                        }
                        }
                        else{
                            lightsOn.Remove(new Vector3(x,y,z));
                        }
                    }
                }
            }
            return lightsOn;
        }   
    }


}

