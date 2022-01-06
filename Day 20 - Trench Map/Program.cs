using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace TrenchMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var algorithm = ReadEhancementAlgorithm(input[0]);
            var image = ReadImage(input.Skip(2).ToArray());

            //DrawImage(image);
            for(int i = 0; i < 2; i++){
                image = ProcessImage(image, algorithm);
            }

            image = DeExtendImage(image);

            DrawImage(image);

            Console.WriteLine("Light up: " + image.SelectMany(s => s).Aggregate((t, b) => t += b));
        }

        static List<int> ReadEhancementAlgorithm(string input){
            List<int> algorithm = new List<int>();
            foreach(char c in input){
                if(c == '#'){
                    algorithm.Add(1);
                }
                else{
                    algorithm.Add(0);
                }
            }
            return algorithm;
        }

        static List<List<int>> ReadImage(string[] input){
            var image = new List<List<int>>();
            List<int> currentLine = null;
            foreach(var line in input){
                currentLine = new List<int>();
                foreach(char c in line){
                    if(c == '#'){
                        currentLine.Add(1);
                    }
                    else{
                        currentLine.Add(0);
                    }
                }
                image.Add(currentLine);
            }

            return image;
        }

        static List<List<int>> ExtendImage(List<List<int>> image){
            foreach(var line in image){
                line.AddRange(new int[]{0, 0});
                line.InsertRange(0, new int[]{0, 0});
            }

            for(int i = 0; i < 2; i++){
                image.Add(NewZeroList(image[0].Count));
                image.Insert(0, NewZeroList(image[0].Count));
            }

            return image;
        }

        static List<List<int>> DeExtendImage(List<List<int>> image){
            image.RemoveRange(0, 1);
            image.RemoveRange(image.Count - 1, 1);
            foreach(var line in image){
                line.RemoveRange(0, 1);
                line.RemoveRange(line.Count - 1, 1);
            }
            return image;
        }

        static List<int> NewZeroList(int size){
            var ret = new List<int>();
            for(int i = 0; i < size; i++){
                ret.Add(0);
            }
            return ret;
        }

        static List<List<int>> NewImage(int height, int width){
            var ret = new List<List<int>>();
            for(int i = 0; i < height; i++){
                var list = new List<int>();
                list.AddRange(NewZeroList(width));
                ret.Add(list);
            }
            return ret;
        }

        static List<List<int>> ProcessImage(List<List<int>> image, List<int> algorithm){
            var extended = ExtendImage(image);
            //DrawImage(extended);
            var newImage = NewImage(extended.Count, extended[0].Count);

            for(int i = 1; i < extended.Count - 1; i++){
                for(int n = 1; n < extended[i].Count - 1; n++){
                    var value = extended[i-1][n-1].ToString() + extended[i-1][n].ToString() + extended[i-1][n+1].ToString()
                    + extended[i][n-1].ToString() + extended[i][n].ToString() + extended[i][n+1].ToString()
                    + extended[i + 1][n-1].ToString() + extended[i + 1][n].ToString() + extended[i + 1][n+1].ToString();

                    var intValue = Convert.ToInt32(value, 2);

                    newImage[i][n] = algorithm[intValue];
                }
            }

            return newImage;
        }

        static void DrawImage(List<List<int>> image){
            for(int i = 0; i < image.Count; i++){
                for(int n = 0; n < image[i].Count; n++){
                    Console.Write(image[i][n] == 1 ? '#' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
