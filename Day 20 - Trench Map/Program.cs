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
                image = ProcessImage(image, algorithm, i % 2 == 0);
                //image = DeExtendImage(image, 1);
            }

            //image = DeExtendImage(image, 2);        

            Console.WriteLine("Light up after 2: " + image.SelectMany(s => s).Aggregate((t, b) => t += b));

            for(int i = 0; i < 48; i++){
                image = ProcessImage(image, algorithm, i % 2 == 0);
            }

            Console.WriteLine("Light up after 50: " + image.SelectMany(s => s).Aggregate((t, b) => t += b));
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

        static List<List<int>> ExtendImage(List<List<int>> image, List<int> algorithm, bool isOdd){
            var toAdd = !isOdd ? algorithm[0] : 0;
            
            foreach(var line in image){
                line.AddRange(new int[]{toAdd, toAdd});
                line.InsertRange(0, new int[]{toAdd, toAdd});
            }

            for(int i = 0; i < 2; i++){
                image.Add(NewIntList(toAdd, image[0].Count));
                image.Insert(0, NewIntList(toAdd, image[0].Count));
            }

            return image;
        }

        static List<List<int>> DeExtendImage(List<List<int>> image, int length){
            image.RemoveRange(0, length);
            image.RemoveRange(image.Count - length, length);
            foreach(var line in image){
                line.RemoveRange(0, length);
                line.RemoveRange(line.Count - length, length);
            }
            return image;
        }

        static List<int> NewIntList(int toAdd, int size){
            var ret = new List<int>();
            for(int i = 0; i < size; i++){
                ret.Add(toAdd);
            }
            return ret;
        }

        static List<List<int>> NewImage(int height, int width, List<int> algorithm, bool isOdd){
            var ret = new List<List<int>>();

            var toAdd = isOdd ? algorithm[0] : 0;

            for(int i = 0; i < height; i++){
                var list = new List<int>();
                list.AddRange(NewIntList(toAdd, width));
                ret.Add(list);
            }
            return ret;
        }

        static List<List<int>> ProcessImage(List<List<int>> image, List<int> algorithm, bool isOdd){
            var extended = ExtendImage(image, algorithm, isOdd);
            //DrawImage(extended);
            var newImage = NewImage(extended.Count, extended[0].Count, algorithm, isOdd);

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
