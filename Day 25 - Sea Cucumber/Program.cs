using System;
using System.IO;
using System.Linq;

namespace SeaCucumber
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var map = ReadInput(input);

            //WriteMap(map);

            var count = 0;
            var moved = true;

            var lefts = (from Cucumber a in map
                        where a != null && a.MoveLeft
                        select a).ToList();

            var downs = (from Cucumber a in map
                        where a != null && !a.MoveLeft
                        select a).ToList();

            while(moved){
                count++;
                
                lefts.ForEach(c => c.CheckCanMove());
                moved = lefts.Aggregate(false, (move, sc) => sc.Move() | move);
                
                downs.ForEach(c => c.CheckCanMove());
                moved = downs.Aggregate(false, (move, sc) => sc.Move() | move) | moved;
                //Console.WriteLine(count);
                //WriteMap(map);
            }

            Console.WriteLine("Sea Cucumbers stopped moving at: " + count);
        }

        static Cucumber[,] ReadInput(string[] input)
        {
            var map = new Cucumber[input[0].Length, input.Length];
            for (int x = 0; x < input[0].Length; x++)
            {
                for (int y = 0; y < input.Length; y++)
                {
                    if (input[y][x] == '.') continue;

                    var sc = new Cucumber()
                    {
                        X = x,
                        Y = y,
                        Map = map,
                        MoveLeft = input[y][x] == '>'
                    };
                    map[x, y] = sc;
                }
            }
            return map;
        }

        static void WriteMap(Cucumber[,] map){
            for(int i = 0; i < map.GetLength(1); i++){
                for(int n = 0; n < map.GetLength(0); n++){
                    if(map[n,i] == null) Console.Write('.');
                    else if(map[n,i].MoveLeft) Console.Write(">");
                    else Console.Write("v");
                }
                Console.WriteLine();
            }
        }
    }
}
