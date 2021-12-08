using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Numerics;

namespace Dive
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var commands = lines.Select(CreateCommand).ToArray();

            var finishPoint = Move(commands);
            Console.WriteLine("Finish Point for Move: " + finishPoint);

            var vectorFinishPoint = MoveWithAim(commands);
            Console.WriteLine("Finish Point with Aim: " + vectorFinishPoint);
        }

        static Command CreateCommand(string line){
            var parts = line.Split(" ");
            return new Command(){
                Direction = parts[0],
                Step = int.Parse(parts[1])
            };
        }

        static Point Move(Command[] commands){
            var currentPosition = new Point(0, 0);
            foreach(var command in commands){
                switch(command.Direction){
                    case "forward":
                        currentPosition.Y += command.Step;
                        break;
                    case "up":
                        currentPosition.X -= command.Step;
                        break;
                    case "down":
                        currentPosition.X += command.Step;
                        break;
                }
            }
            return currentPosition;
        }

        static Vector3 MoveWithAim(Command[] commands){
            var currentPosition = new Vector3(0, 0, 0);
            foreach(var command in commands){
                switch(command.Direction){
                    case "forward":
                        currentPosition.Y += command.Step;
                        currentPosition.X += command.Step * currentPosition.Z;
                        break;
                    case "up":
                        currentPosition.Z -= command.Step;
                        break;
                    case "down":
                        currentPosition.Z += command.Step;
                        break;
                }
            }
            return currentPosition;
        }
    }

    class Command{
        public string Direction {get;set;}
        public int Step {get;set;}
    }
}
