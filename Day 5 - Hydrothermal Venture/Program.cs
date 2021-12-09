using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HydrothermalVenture
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var lines = GetLines(input);

            var straightLines = lines.Where(l => l.Start.X == l.End.X || l.Start.Y == l.End.Y).ToList();

            var allPoints = new List<Vector2>();

            straightLines.ForEach(l => allPoints.AddRange(l.AllPoints));

            var crosses = allPoints.GroupBy(x => ComputeHash(x));

            Console.WriteLine("Crosses " + crosses.Where(c => c.Count() > 1).Count() + " times with only horizontal and vertical lines");

            allPoints.Clear();

            lines.ForEach(l => allPoints.AddRange(l.AllPoints));

            crosses = allPoints.GroupBy(x => ComputeHash(x));

            Console.WriteLine("Crosses " + crosses.Where(c => c.Count() > 1).Count() + " times for all lines");
        }

        static List<Line> GetLines(string[] lines){
            var inputLines = new List<Line>();
            foreach(var inputLine in lines){
                var startEnd = inputLine.Split(" -> ");
                var startSplit = startEnd[0].Split(",");
                var endSplit = startEnd[1].Split(",");

                var startPoint = new Vector2(float.Parse(startSplit[0]), float.Parse(startSplit[1]));
                var endPoint = new Vector2(float.Parse(endSplit[0]), float.Parse(endSplit[1]));

                inputLines.Add(new Line(startPoint, endPoint));
            }

            return inputLines;
        }

        static long ComputeHash(Vector2 point){
            return Convert.ToInt64(LegthenString(point.X.ToString()) + LegthenString(point.Y.ToString()));
        }

        static string LegthenString(string number){
            while(number.Length < 3){
                number = "0" + number;
            }
            return number;
        }
    }

    class Line
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        public Vector2 Step { get; private set; }
        public List<Vector2> AllPoints { get; private set; }

        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;

            Step = CalculateStep();

            AllPoints = GetAllPoints();
        }

        private Vector2 CalculateStep()
        {
            var x = End.X - Start.X;
            var y = End.Y - Start.Y;

            var gcd = Math.Abs(GCD(x, y));

            return new Vector2() { X = x / gcd, Y = y / gcd };
        }

        static float GCD(float a, float b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        List<Vector2> GetAllPoints(){
            var allPoints = new List<Vector2>();
            allPoints.Add(Start);
            var nextPoint = Vector2.Add(allPoints.Last(), Step);
            while(nextPoint.X != End.X || nextPoint.Y != End.Y){
                allPoints.Add(nextPoint);
                nextPoint = Vector2.Add(allPoints.Last(), Step);
            }
            allPoints.Add(End);

            return allPoints;
        }
    }
}
