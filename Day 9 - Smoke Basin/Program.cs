using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SmokeBasin
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            int[,] heightmap = new int[lines.Count(), lines[0].Length];

            for(int i = 0; i < lines.Count(); i++){
                for(int n = 0; n < lines[0].Length; n++){
                    heightmap[i, n] = int.Parse(lines[i][n].ToString());
                }
            }

            int total = 0;
            var lowPoints = new List<object>();

            for(int i = 0; i < heightmap.GetLength(0); i++){
                for(int n = 0; n < heightmap.GetLength(1); n++){
                    int current = heightmap[i,n];
                    bool isLowpoint = true;

                    if(i - 1 >= 0){
                        if(heightmap[i-1,n]<=current)
                            isLowpoint = false;
                    }
                    if(i + 1 < heightmap.GetLength(0) && isLowpoint){
                        if(heightmap[i + 1,n]<=current)
                            isLowpoint = false;
                    }

                    if(n + 1 < heightmap.GetLength(1) && isLowpoint){
                        if(heightmap[i,n+1]<=current)
                            isLowpoint = false;
                    }
                    if(n - 1 >= 0 && isLowpoint){
                        if(heightmap[i,n-1]<=current)
                            isLowpoint = false;
                    }

                    if(isLowpoint){
                        total+=(current + 1);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(current);
                        Console.ResetColor();
                    }
                    else{
                        if(current != 9){
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(current);
                        Console.ResetColor();
                        }
                        else{
                            Console.Write(current);
                        }
                    }
                }
                Console.WriteLine("");
            }

                Console.WriteLine("Total Lowpoint Risk Level: " + total);
        }
    }
}
