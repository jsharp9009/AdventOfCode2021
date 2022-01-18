using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Amphipod
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var burrow = new BurrowState(input);

            Console.WriteLine("Best Cost Part 1: " + GetBestCost(burrow));

            var lineList = input.ToList();
            lineList.Insert(3, "  #D#B#A#C#");
            lineList.Insert(3, "  #D#C#B#A#");

            var burrow2 = new BurrowState(lineList.ToArray());
            
            Console.WriteLine("Best Cost Part 2:" + GetBestCost(burrow2));
        }

        static int GetBestCost(BurrowState burrow){
            var finalState = BurrowState.GetFinalBurrowState(burrow);

            var stateCosts = new Dictionary<BurrowState, int>();
            stateCosts.Add(burrow, 0);

            var queue = new Queue<BurrowState>();
            queue.Enqueue(burrow);

            while(queue.Count > 0){
                var currentState = queue.Dequeue();

                currentState.cost = (Math.Min(currentState.cost, stateCosts.ContainsKey(currentState) ? stateCosts[currentState] : int.MaxValue));

                foreach(var nextState in currentState.GetNextValidStates()){
                    var cost = (Math.Min(nextState.cost, stateCosts.ContainsKey(nextState) ? stateCosts[nextState] : int.MaxValue));
                    if(stateCosts.ContainsKey(nextState)){
                            if(cost < stateCosts[nextState]){
                                stateCosts[nextState] = cost;
                                stateCosts.First(p => p.Key.Equals(nextState)).Key.previousState = nextState.previousState;
                            }
                        }
                    else
                        stateCosts.Add(nextState, cost);
                    nextState.cost = cost;
                    if(!queue.Contains(nextState)){
                        queue.Enqueue(nextState);
                    }
                }
            }

            //File.WriteAllLines("debug.txt", stateCosts.Keys.Select(k => k.ToString()));
            // Console.WriteLine(finalState.ToString());
            // Console.WriteLine(stateCosts.Last().Key.ToString());
            //Console.WriteLine("Total State: " + stateCosts.Count);
            return stateCosts[finalState];

            //var finalStateScored = stateCosts.First(p => p.Key.Equals(finalState)).Key;

            // while(finalStateScored != null){
            //     Console.WriteLine(finalStateScored.ToString());
            //     Console.WriteLine();

            //     finalStateScored = finalStateScored.previousState;
            // }
        }
    }
}
