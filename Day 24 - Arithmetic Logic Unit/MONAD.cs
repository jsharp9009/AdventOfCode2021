using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ALU
{
    class MONAD
    {
        public static Queue<int?> computeInput = new Queue<int?>();

        static void Main(string[] args)
        {
            var programs = ReadInput();

            var currentStates = new List<State>() { new State() };

            for (int i = 0; i < 14; i++)
            {
                var nextStates = new List<State>();
                //Console.WriteLine("Digit: " + i + ": " + currentStates.Count());
                foreach (var state in currentStates)
                {
                    for (int n = 9; n > 0; n--)
                    {
                        var nextState = (State)state.Clone();
                        nextState.Commands = programs[i];
                        nextState.Input = n;
                        nextState = ComputeModule.RunProgram((State)nextState);

                        nextState.Previous = state;

                        if (nextState.Commands.Any(c => c.Instruction == "div" && c.Param1 == 'z' && c.Param2 == 26))
                        {
                            if (nextState.Variables['x'] != 0 || nextState.Variables['y'] != 0 || nextState.Variables['z'] >= state.Variables['z'])
                                continue;
                        }

                        nextStates.Add(nextState);
                    }
                }
                currentStates = nextStates;
            }

            var valid = currentStates.Where(s => s.Variables['z'] == 0);

            var ids = valid.Select(s => long.Parse(s.ToString()));

            Console.WriteLine("Largest Model Number is: " + ids.Max());
            Console.WriteLine("Smallest Model Number is: " + ids.Min());
        }
        static List<List<Command>> ReadInput()
        {
            var lines = File.ReadAllLines("input.txt");
            List<List<Command>> program = new List<List<Command>>();
            var commands = new List<Command>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var command = new Command();
                command.Instruction = parts[0];
                command.Param1 = parts[1][0];
                if (parts.Length > 2)
                {
                    if (parts[2].Length > 1)
                    {
                        command.Param2 = int.Parse(parts[2]);
                        command.Param2IsChar = false;
                    }
                    else
                    {
                        if (Char.IsLetter(parts[2][0]))
                        {
                            command.Param2 = parts[2][0];
                            command.Param2IsChar = true;
                        }
                        else
                        {
                            command.Param2 = int.Parse(parts[2]);
                            command.Param2IsChar = false;
                        }
                    }
                }
                if (command.Instruction == "inp")
                {
                    if (commands.Count() > 0)
                    {
                        program.Add(commands);
                        commands = new List<Command>();
                        commands.Add(command);
                    }
                    else
                    {
                        commands.Add(command);
                    }
                }
                else
                {
                    commands.Add(command);
                }
            }

            program.Add(commands);

            return program;
        }
    }
}
