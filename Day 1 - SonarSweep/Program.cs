using System;
using System.IO;
using System.Linq;

namespace SonarSweep
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.csv");

            var numbers = lines.Select(s => int.Parse(s)).ToArray();

            var drops = 0;
            int? lastNum = null;

            foreach(int depth in numbers){
               
                if(lastNum < depth && lastNum != null){
                    drops++;
                }
                lastNum = depth;

            }

            Console.WriteLine("Number of Drops: " + drops);

            drops = 0;
            lastNum = null;
            for(int i = 0; i < numbers.Count(); i++){
                var current = GetNumber(numbers, i) + GetNumber(numbers, i+1) + GetNumber(numbers, i+2);
                if(lastNum < current && lastNum != null)
                    drops++;
                lastNum = current;
            }

            Console.WriteLine("Number of Drops for three-measurement sliding window: " + drops);
        }

        private static int GetNumber(int[] numbers, int index){
            if(index < numbers.Count())
                return numbers[index];
            else
                return 0;
        }
    }
}
