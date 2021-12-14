using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ExtendedPolymerization
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var startingPolymer = lines[0];
            var rules = GetRules(lines.Skip(2).ToArray());

            for(int i = 0; i < 10; i++){
                startingPolymer = ProcessStep(startingPolymer, rules);
            }

            //Console.WriteLine(startingPolymer);

            var counts = CountElements(startingPolymer);
            var max = counts.Aggregate((k, v) => k.Value > v.Value ? k : v ).Key;
            var min = counts.Aggregate((k, v) => k.Value < v.Value ? k : v ).Key;

            Console.WriteLine("Part 1 Max is " + max + " with " + counts[max] + " occurences");
            Console.WriteLine("Part 1 Min is " + min + " with " + counts[min] + " occurences");
            Console.WriteLine("Part 1 answer: " + (counts[max] - counts[min]));

            for(int i = 0; i < 30; i++){
                startingPolymer = ProcessStep(startingPolymer, rules);
            }

            counts = CountElements(startingPolymer);
            max = counts.Aggregate((k, v) => k.Value > v.Value ? k : v ).Key;
            min = counts.Aggregate((k, v) => k.Value < v.Value ? k : v ).Key;

            Console.WriteLine("Part 2 Max is " + max + " with " + counts[max] + " occurences");
            Console.WriteLine("Part 2 Min is " + min + " with " + counts[min] + " occurences");
            Console.WriteLine("Part 2 answer: " + (counts[max] - counts[min]));
        }

        static string ProcessStep(string polymer, Dictionary<string, string> rules){
            StringBuilder newPolymer = new StringBuilder();
            newPolymer.Append(polymer[0]);
            for(int i = 1; i < polymer.Count(); i++){
                var ruleCheck = newPolymer[newPolymer.Length-1].ToString() + polymer[i].ToString();
                if(rules.ContainsKey(ruleCheck)){
                    newPolymer.Append(rules[ruleCheck]);
                }
                newPolymer.Append(polymer[i]);
            }
            return newPolymer.ToString();
        }

        static Dictionary<string, string> GetRules(string[] lines){
            Dictionary<string, string> rules = new Dictionary<string, string>();
            foreach(string line in lines){
                var parts = line.Split(" -> ");
                rules.Add(parts[0], parts[1]);
            }

            return rules;
        }

        static Dictionary<string, int> CountElements(string polymer)
        {
            var counts = new Dictionary<string, int>();
            var uniqueElements = polymer.Distinct();
            foreach(var element in uniqueElements){
                counts.Add(element.ToString(), polymer.Count(c => c == element));
            }
            return counts;
        }
    }
}
