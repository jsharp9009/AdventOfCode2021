using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace TransparentOrigami
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var points = new List<Vector2>();
            var folds = new Queue<dynamic>();

            ReadInput(lines, points, folds);
            var firstRun = true;;
            while(folds.Count > 0){
                var fold = folds.Dequeue();
                if(fold.Key == "x"){
                    FoldX(ref points, fold.Value);
                }
                else{
                    FoldY(ref points, fold.Value);
                }

                if(firstRun){
                    firstRun = false;
                    Console.WriteLine("Points visible after first fold: " + points.Count);
                }
            }

            Console.WriteLine("Points visible after all folds: " + points.Count);
            
            var maxX = points.Max(x => x.X) + 1;
            var maxY = points.Max(y => y.Y) + 1;

            for(int y = 0; y < maxY; y++){
                for(int x = 0; x < maxX; x++){
                    if(points.Any(p => p.X == x && p.Y == y)){
                        Console.Write(" *");
                    }
                    else{
                        Console.Write(" .");
                    }
                }
                Console.WriteLine("");
            }
        }

        static void FoldY(ref List<Vector2> points, int foldLine){
            
            for(int i = 0; i < points.Count(); i++){
                if(points[i].Y > foldLine){
                    var dif = points[i].Y - foldLine;
                    var newY = foldLine - dif;
                    points[i] = new Vector2(points[i].X, newY);
                }
            }

            points = points.Distinct(new Vector2EqualityComparer()).ToList();
        }

        static void FoldX(ref List<Vector2> points, int foldLine){
            for(int i = 0; i < points.Count(); i++){
                if(points[i].X > foldLine){
                    var dif = points[i].X - foldLine;
                    var newX = foldLine - dif;
                    points[i] = new Vector2(newX, points[i].Y);
                }
            }

            points = points.Distinct(new Vector2EqualityComparer()).ToList();
        }

        static void ReadInput(string[] lines, List<Vector2> points, Queue<dynamic> folds){
            bool afterSpace = false;
            foreach(string line in lines){
                if(string.IsNullOrEmpty(line)){
                    afterSpace = true;
                    continue;
                }
                if(!afterSpace){
                    var pieces = line.Split(',');
                    points.Add(new Vector2(int.Parse(pieces[0]), int.Parse(pieces[1])));
                }
                else{
                    var parts = line.Split(' ');
                    var foldParts = parts.Last().Split('=');
                    folds.Enqueue(new {Key = foldParts[0], Value = int.Parse(foldParts[1])});
                }
            }
        }
    }

    public class Vector2EqualityComparer : IEqualityComparer<Vector2>
    {
        public bool Equals([AllowNull] Vector2 x, [AllowNull] Vector2 y)
        {
            if(x == null && y == null) return true;
            if(x == null || y == null) return false;
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode([DisallowNull] Vector2 obj)
        {
           return Convert.ToInt32(obj.X.ToString() + obj.Y.ToString());
        }
    }
}
