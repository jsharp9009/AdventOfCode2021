using System;
using System.IO;
using System.Linq;

namespace BinaryDiagnostic
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var columns = lines[0].Length;

            var gamaBinary = CalculateGama(lines, columns);
            Console.WriteLine("Gama Binary: " + gamaBinary);
            var gamaInt = Convert.ToInt32(gamaBinary, 2);
            Console.WriteLine("Gama Integer: " + gamaInt);

            var epsilonBinary = CalculateEpsilon(lines, columns);
            Console.WriteLine("Epsilon Binary: " + epsilonBinary);
            var epsilonInt = Convert.ToInt32(epsilonBinary, 2);
            Console.WriteLine("Epsilon Integer: " + epsilonInt);
            Console.WriteLine("Power Consuption Rate: " + (gamaInt * epsilonInt));

            var oxygenGeneratorRating = CalculateOxygenGeneratorRating(lines, columns);
            Console.WriteLine("Oxygen Generator Rating Binary: " + oxygenGeneratorRating);
            var ogrInt = Convert.ToInt32(oxygenGeneratorRating, 2);
            Console.WriteLine("Oxygen Generator Rating int: " + ogrInt);

            var cO2ScrubberRatting = CalculateCO2ScubberRating(lines, columns);
            Console.WriteLine("CO2 Scrubber Rating Binary: " + cO2ScrubberRatting);
            var cO2ScrubRattingInt = Convert.ToInt32(cO2ScrubberRatting, 2);
            Console.WriteLine("CO2 Scrubber Rating Int: " + cO2ScrubRattingInt);

            Console.WriteLine("Life Support Rating: " + (ogrInt * cO2ScrubRattingInt));
        }

        static string CalculateGama(string[] lines, int columns){
            string resultBinary = "";

            for(int i = 0; i < columns; i++){
                int sum = lines.Sum(c => c[i] == '1' ? 1 : 0);
                if(sum > lines.Count() / 2)
                    resultBinary += "1";
                else 
                    resultBinary += "0";
            }
            return resultBinary;
        }

        static string CalculateEpsilon(string[] lines, int columns){
            string resultBinary = "";

            for(int i = 0; i < columns; i++){
                int sum = lines.Sum(c => c[i] == '1' ? 1 : 0);
                if(sum < lines.Count() / 2)
                    resultBinary += "1";
                else 
                    resultBinary += "0";
            }
            return resultBinary;
        }

        static string CalculateOxygenGeneratorRating(string[] lines, int columns){
            for(int i = 0; i < columns; i++){
                int sum = lines.Sum(c => c[i] == '1' ? 1 : 0);
                if(sum >= Math.Ceiling(lines.Count() / 2.0)){
                    lines = lines.Where(l => l[i] == '1').ToArray();
                }
                else {
                    lines = lines.Where(l => l[i] == '0').ToArray();
                }

                if(lines.Count() == 1)
                    return lines[0];
            }

            return "";
        }

        static string CalculateCO2ScubberRating(string[] lines, int columns){
            for(int i = 0; i < columns; i++){
                int sum = lines.Sum(c => c[i] == '1' ? 1 : 0);
                if(sum < Math.Ceiling(lines.Count() / 2.0)){
                    lines = lines.Where(l => l[i] == '1').ToArray();
                }
                else {
                    lines = lines.Where(l => l[i] == '0').ToArray();
                }

                if(lines.Count() == 1)
                    return lines[0];
            }

            return "";
        }
    }
}
