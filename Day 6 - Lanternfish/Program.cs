using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Lanternfish
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt").Split(",");
            var lanternfish = Array.ConvertAll(input, s=> int.Parse(s));
            //var lanternfish = Array.ConvertAll(input, s => new LanternFish(int.Parse(s))).ToList();

            var day80 = ProcessGrowth(lanternfish, 80);
            var day256 = ProcessGrowth(lanternfish, 256);

            Console.WriteLine("Lanterfish on Day 80: " + day80);
            Console.WriteLine("Lanterfish on Day 256: " + day256);

// #region Part 1 Solution.

//             for(int i = 0; i < 80; i++){
//                 ProcessDay(lanternfish);
//             }

//             Console.WriteLine("Lanterfish on Day 80: " + lanternfish.Count);
// #endregion



        }

        static long ProcessGrowth(int[] starterFish, int days){
            long[] fish = new long[10];
            
            foreach(int f in starterFish){
                fish[f]++;
            }

            for(int day = 0; day < days; day++){
                fish[7] += fish[0];
                fish[9] = fish[0];
                for(int i = 0; i < 9; i++){
                    fish[i] = fish[i+1];
                }
                fish[9] = 0;
            }

            return fish.Sum();
        }

        // static void ProcessDay(List<LanternFish> lanternfish){
        //     var startingLength = lanternfish.Count;

        //     lanternfish.Take(startingLength).ToList().ForEach(
        //         f => ProcessFish(ref f, ref lanternfish)
        //     );

        //     // for(int i = 0; i < startingLength; i++){
        //     //     if(lanternfish[i] == 0){
        //     //         lanternfish[i] = 6;
        //     //         lanternfish.Add(8);
        //     //     }
        //     //     else{
        //     //         lanternfish[i]--;
        //     //     }
        //     // }
        // }

        // static void ProcessFish(ref LanternFish fish, ref List<LanternFish> lanternfish){
        //     if(fish.DaysTillSpawn == 0){
        //             fish.DaysTillSpawn = 6;
        //             lanternfish.Add(new LanternFish(8));
        //         }
        //         else{
        //             fish.DaysTillSpawn--;
        //         }
        // }
    }

    // class LanternFish{
    //     public int DaysTillSpawn {get;set;}
    //     public LanternFish(int daysTillSpawn){
    //         DaysTillSpawn = daysTillSpawn;
    //     }
    // }
}
