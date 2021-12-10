using System;
using System.IO;
using System.Linq;

namespace TreacheryOfWhales
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            int[] startingPostions = Array.ConvertAll(input.Split(','), s => int.Parse(s));
            
            var maxLocation = startingPostions.Max();

            FindMinFuelConsuption(startingPostions, maxLocation);
            FindMinFuelConsuptionForPart2(startingPostions, maxLocation);
        }

        static void FindMinFuelConsuption(int[] positions, int max){
            int? bestFuel = null;
            int bestLocation = 0;

            for(int i = 0; i < max; i++){
                var fuelConsuption = 0;
                foreach(int position in positions){
                    fuelConsuption += Math.Abs(position - i);
                }

                if(bestFuel == null || bestFuel > fuelConsuption){
                        bestFuel = fuelConsuption;
                        bestLocation = i;
                    }
            }

            Console.WriteLine("Best Position: " + bestLocation + " with fuel consuption of " + bestFuel);            
        }

        static void FindMinFuelConsuptionForPart2(int[] positions, int max){
            int? bestFuel = null;
            int bestLocation = 0;

            for(int i = 0; i < max; i++){
                var fuelConsuption = 0;
                foreach(int position in positions){
                    var steps = Math.Abs(position - i);
                    for(int n = 1; n <= steps; n++){
                    fuelConsuption += n ;
                    }
                }

                if(bestFuel == null || bestFuel > fuelConsuption){
                        bestFuel = fuelConsuption;
                        bestLocation = i;
                    }
            }

            Console.WriteLine("Best Position Part 2: " + bestLocation + " with fuel consuption of " + bestFuel);            
        }
    }

}
