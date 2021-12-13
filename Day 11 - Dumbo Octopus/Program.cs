using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

            for(int i = 0; i < 1; i++){
                ProcessStep(octopusMap);
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

        static void ProcessStep(Octopus[,] octopusMap){
            var taskPool = new List<Task>();
            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    taskPool.Add(ProcessOctopus(octopusMap[i, n]));
                }

            Task.WaitAll(taskPool.ToArray());

            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    if(octopusMap[i, n].Flashing){
                        ProcessBlink(i, n, octopusMap);
                    }
                }
            
            taskPool.Clear();
            for(int i = 0; i < octopusMap.GetLength(0); i++)
                for(int n = 0; n < octopusMap.GetLength(1); n++){
                    taskPool.Add(CheckResetPower(octopusMap[i, n]));
                }

            Task.WaitAll(taskPool.ToArray());
        }

        static async Task ProcessOctopus(Octopus o){
            o.Power++;
            if(o.Power == 9)
                o.Flashing = true;
        }

        static async Task CheckResetPower(Octopus o){
            if(o.Power == 9){
                o.Power = 0;
                o.Flashing = false;
            }
        }

        static void ProcessBlink(int i, int n, Octopus[,] octopusMap){
            if((i -1) >= 0 && !octopusMap[i-1,n].Flashing){
                Task.WaitAll(ProcessOctopus(octopusMap[i-1,n]));
                if(octopusMap[i-1,n].Flashing)
                    ProcessBlink(i-1, n, octopusMap);
                
                if((n-1) >= 0 && !octopusMap[i - 1, n - 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i-1,n-1]));
                    if(octopusMap[i-1,n-1].Flashing)
                        ProcessBlink(i-1, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && !octopusMap[i - 1, n + 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i-1,n+1]));
                    if(octopusMap[i-1,n+1].Flashing)
                        ProcessBlink(i-1, n+1, octopusMap);
                }
            }

            if((i + 1) < octopusMap.GetLength(0) && !octopusMap[i+ 1,n].Flashing){
                Task.WaitAll(ProcessOctopus(octopusMap[i + 1,n]));
                if(octopusMap[i+ 1,n].Flashing)
                    ProcessBlink(i+ 1, n, octopusMap);
                
                if((n-1) >= 0 && !octopusMap[i + 1, n - 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i + 1,n-1]));
                    if(octopusMap[i+ 1,n-1].Flashing)
                        ProcessBlink(i+1, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && !octopusMap[i + 1, n + 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i+1,n+1]));
                    if(octopusMap[i+1,n+1].Flashing)
                        ProcessBlink(i+1, n+1, octopusMap);
                }
            }

            if((n-1) >= 0 && !octopusMap[i, n - 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i,n-1]));
                    if(octopusMap[i,n-1].Flashing)
                        ProcessBlink(i, n-1, octopusMap);
                }

                if((n+1) < octopusMap.GetLength(1) && !octopusMap[i, n + 1].Flashing){
                    Task.WaitAll(ProcessOctopus(octopusMap[i,n+1]));
                    if(octopusMap[i,n+1].Flashing)
                        ProcessBlink(i, n+1, octopusMap);
                }
        }
    }

    class Octopus{
        public int Power{get;set;}
        public bool Flashing{get;set;}
    }
}