using System;
using System.IO;

namespace SyntaxScoring
{
    class Program
    {
        static Dictionary<char, char> closers = new Dictionary<char, char>(){
            {'(', ')'},
            {'{', '}'},
            {'<', '>'},
            {'[', ']'}
        };

        static Dictionary<char, int> errorValues= new Dictionary<char, int>(){
            {')', 3},
            {'}', 1197},
            {']', 57},
            {'>', 25137}
        };

        static Dictionary<char, int> completionValues= new Dictionary<char, int>(){
            {')', 1},
            {'}', 3},
            {']', 2},
            {'>', 4}
        };

        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt");
            var errors = new List<char>();

            var lineScores = new List<long>();

            foreach(var line in lines){
                Console.Write(line + ": ");
                var toFinish = new List<char>();
                var error = findError(line, toFinish);
                if(error.HasValue){
                    Console.Write(error);
                    errors.Add(error.Value);
                }
                else{
                    Console.Write(string.Join(' ', toFinish));
                    long lineScore = 0;
                    foreach(char c in toFinish){
                        lineScore = (lineScore * 5) + completionValues[c];
                    }
                    lineScores.Add(lineScore);
                    
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Error Total: " + (errors.Sum(e => errorValues[e])));

            var midLineScore = lineScores.OrderBy(l => l).ToArray()[(int)(Math.Floor(lineScores.Count / 2.0))];

            Console.WriteLine("Mid point completion score: " + midLineScore);

        }

        static char? findError(string input, List<char> toFinish){
            var order = new Stack<char>();
            foreach(char i in input){
                if(closers.Keys.Contains(i)){
                    order.Push(i);
                }
                else{
                    var toMatch = order.Pop();
                    if(i != closers[toMatch])
                        return i;
                }
            }

            while(order.TryPop(out char outresult)){
                toFinish.Add(closers[outresult]);
            }
            return null;
        }
    }
}