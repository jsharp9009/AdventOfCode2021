using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

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
            var lowPoints = new List<Vector2>();

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
                        lowPoints.Add(new Vector2(i,n));
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

            List<List<Vector2>> basins = new List<List<Vector2>>();

            foreach(var lowpoint in lowPoints){
                var basin = new List<Vector2>();
                var max = new Vector2(heightmap.GetLength(0), heightmap.GetLength(1));

                GetBasin(heightmap, lowpoint, basin, max);

                basins.Add(basin);
            }

                Console.WriteLine("Total Lowpoint Risk Level: " + total);
                
                var basinTotal = basins.OrderByDescending(b => b.Count()).Take(3).Select(t => t.Count()).Aggregate(1, (a, b) => a * b);

                Console.WriteLine("Basin Total: " + basinTotal);
        }

        static void GetBasin(int[,] heightmap, Vector2 point, List<Vector2> basin, Vector2 max)
        {
            if(heightmap[(int)point.X, (int)point.Y] == 9)
                return;

            basin.Add(point);
            if((point.X - 1) >= 0 && !basin.Any(b => b.X == (point.X - 1) && b.Y == point.Y)){
                GetBasin(heightmap, new Vector2(point.X - 1, point.Y), basin, max);
            }
            if((point.X + 1) < max.X  && !basin.Any(b => b.X == (point.X + 1) && b.Y == point.Y)){
                GetBasin(heightmap, new Vector2(point.X + 1, point.Y), basin,max);
            }
            if((point.Y - 1) >= 0  && !basin.Any(b => b.X == point.X && b.Y == (point.Y - 1))){
                GetBasin(heightmap, new Vector2(point.X, point.Y - 1), basin,max);
            }
            if((point.Y + 1) < max.Y  && !basin.Any(b => b.X == point.X && b.Y == (point.Y + 1))){
                GetBasin(heightmap, new Vector2(point.X, point.Y + 1), basin,max);
            }
        }
    }
}
