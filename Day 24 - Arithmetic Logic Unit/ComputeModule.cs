using System;
using System.Collections.Generic;

namespace ALU
{
    public static class ComputeModule
    {
        private static readonly Dictionary<string, Action<Command, State>> instructions;

        static ComputeModule(){
            instructions = new Dictionary<string, Action<Command, State>>(){
                {"inp", new Action<Command, State>(ReadInput)},
                {"add", new Action<Command, State>(Add)},
                {"mul", new Action<Command, State>(Multiply)},
                {"div", new Action<Command, State>(Divide)},
                {"mod", new Action<Command, State>(Modulus)},
                {"eql", new Action<Command, State>(Equals)}
            };
        }

        public static State RunProgram(State state){
            foreach(var command in state.Commands){
                instructions[command.Instruction].Invoke(command, state);
                //Console.WriteLine(command.Instruction + " " + command.Param1 + " " + (command.Param2IsChar ? ((char)command.Param2).ToString() : command.Param2.ToString())
                //    + "\t\t" + "{" + state.Variables['w'] + ", " + state.Variables['x'] + ", " + state.Variables['y'] + ", " + state.Variables['z'] + "}");
            }
            return state;
        }

        private static void ReadInput(Command command, State state){
            state.Variables[command.Param1] = state.Input;
        }

        private static void Add(Command command, State state){
            var value1 = state.Variables[command.Param1];
            int value2 = 0;
            if(command.Param2IsChar){
                value2 = state.Variables[(char)command.Param2];
            } else {
                value2 = command.Param2;
            }

            state.Variables[command.Param1] = value1 + value2;
        }

        private static void Multiply(Command command, State state){
            var value1 = state.Variables[command.Param1];
            int value2 = 0;
            if(command.Param2IsChar){
                value2 = state.Variables[(char)command.Param2];
            } else {
                value2 = command.Param2;
            }

            state.Variables[command.Param1] = value1 * value2;
        }

        private static void Divide(Command command, State state){
            var value1 = state.Variables[command.Param1];
            int value2 = 0;
            if(command.Param2IsChar){
                value2 = state.Variables[(char)command.Param2];
            } else {
                value2 = command.Param2;
            }

            state.Variables[command.Param1] = (int)Math.Floor(value1 / (double)value2);
        }

        private static void Modulus(Command command, State state){
            var value1 = state.Variables[command.Param1];
            int value2 = 0;
            if(command.Param2IsChar){
                value2 = state.Variables[(char)command.Param2];
            } else {
                value2 = command.Param2;
            }

            state.Variables[command.Param1] = value1 % value2;
        }

        private static void Equals(Command command, State state){
            var value1 = state.Variables[command.Param1];
            int value2 = 0;
            if(command.Param2IsChar){
                value2 = state.Variables[(char)command.Param2];
            } else {
                value2 = command.Param2;
            }

            state.Variables[command.Param1] = value1 == value2 ? 1 : 0;
        }
    }
}
