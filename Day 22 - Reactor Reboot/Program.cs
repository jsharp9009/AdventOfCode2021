using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ReactorReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").ToArray();
            var instructions = ReadInput(input);

            Reactor.Intialize(instructions);
            Reactor.Reboot(instructions);
        }

        static Instruction[] ReadInput(string[] input){
            Instruction[] instructions = new Instruction[input.Count()];
            for(int i = 0; i < input.Count(); i++){
                var instruction = new Instruction();
                var lineSplit = input[i].Split(" ");
                instruction.On = lineSplit[0] == "on";
                
                var coordinates = lineSplit[1].Split(",");

                var xCoors = coordinates[0].Replace("x=", "").Split("..");
                var yCoors = coordinates[1].Replace("y=", "").Split("..");
                var zCoors = coordinates[2].Replace("z=", "").Split("..");

                instruction.Cube = new Cube(int.Parse(xCoors[0]), int.Parse(xCoors[1]),
                                            int.Parse(yCoors[0]), int.Parse(yCoors[1]),
                                            int.Parse(zCoors[0]), int.Parse(zCoors[1]));

                instructions[i] = instruction;
            }
            return instructions;
        }
    }
}

