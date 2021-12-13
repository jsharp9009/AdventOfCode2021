using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DumboOctopus
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Octopus[,] octopusMap = new Octopus[lines.Count(), lines[0].Length];

            for(int i = 0; i < lines.Count(); i++){
                for(int n = 0; n < lines[0].Length; n++){
                    octopusMap[i, n] =  new Octopus(){Power = int.Parse(lines[i][n].ToString())};
                }
            }

            var blunk = 0;
            var step = 0;
            while(true){
                var blunkThisStep = ProcessStep(octopusMap);
                blunk += blunkThisStep;
                if(step == 99){
                    Console.WriteLine("blink count at step 100: " + blunk);
                }
                if(blunkThisStep == 100){
                    Console.WriteLine("all blink on step: " + (step+1));
                    break;
                }
                step++;

                if(step > 1000)
                    break;
            }

            for(int i = 0; i < lines.Count(); i++){
                for(int n = 0; n < lines[0].Length; n++){
                    if(octopusMap[i,n].Power == 0) Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(octopusMap[i,n].Power);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            
        }

        static int ProcessStep(Octopus[,] octopusMap){
            var taskPool = new List<Task>();
            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    taskPool.Add(ProcessOctopus(octopusMap[i, n]));
                }

            Task.WaitAll(taskPool.ToArray());

            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    if(octopusMap[i, n].Power == 0){
                        ProcessBlink(i, n, octopusMap);
                    }
                }
            int blunk = 0;
            taskPool.Clear();
            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    if(octopusMap[i,n].Blunk) blunk++;
                    taskPool.Add(ResetBlunk(octopusMap[i, n]));
                }

            Task.WaitAll(taskPool.ToArray());

            return blunk;
        }

        static async Task ProcessOctopus(Octopus o){
            o.Power++;
            if(o.Power > 9){
                o.Power = 0;
            }
        }

        static async Task ResetBlunk(Octopus o){
            o.Blunk = false;
        }

        static void ProcessBlink(int i, int n, Octopus[,] octopusMap){
            if(octopusMap[i, n].Blunk)
                return;

            if((i -1) >= 0) {

                if(octopusMap[i-1,n].Power != 0){
                Task.WaitAll(ProcessOctopus(octopusMap[i-1,n]));
                if(octopusMap[i-1,n].Power == 0)
                    ProcessBlink(i-1, n, octopusMap);
                }
                
                if((n-1) >= 0 && octopusMap[i - 1, n - 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i-1,n-1]));
                    if(octopusMap[i-1,n-1].Power == 0)
                        ProcessBlink(i-1, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && octopusMap[i - 1, n + 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i-1,n+1]));
                    if(octopusMap[i-1,n+1].Power == 0)
                        ProcessBlink(i-1, n+1, octopusMap);
                }
            }

            if((i + 1) < octopusMap.GetLength(0)){
                if(octopusMap[i+ 1,n].Power != 0){
                Task.WaitAll(ProcessOctopus(octopusMap[i + 1,n]));
                if(octopusMap[i+ 1,n].Power == 0)
                    ProcessBlink(i+ 1, n, octopusMap);
                }
                
                if((n-1) >= 0 && octopusMap[i + 1, n - 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i + 1,n-1]));
                    if(octopusMap[i+ 1,n-1].Power == 0)
                        ProcessBlink(i+1, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && octopusMap[i + 1, n + 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i+1,n+1]));
                    if(octopusMap[i+1,n+1].Power == 0)
                        ProcessBlink(i+1, n+1, octopusMap);
                }
            }

            if((n-1) >= 0 && octopusMap[i, n - 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i,n-1]));
                    if(octopusMap[i,n-1].Power == 0)
                        ProcessBlink(i, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && octopusMap[i, n + 1].Power != 0){
                    Task.WaitAll(ProcessOctopus(octopusMap[i,n+1]));
                    if(octopusMap[i,n+1].Power == 0)
                        ProcessBlink(i, n+1, octopusMap);
                }

                octopusMap[i, n].Blunk = true;
        }
    }

    class Octopus{
        public int Power{get;set;}
        public bool Blunk{get;set;}
    }
}