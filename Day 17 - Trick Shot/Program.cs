using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System;

namespace TrickShot
{
    class Program
    {
        static int targetMinX = 20;
            static int targetMaxX = 30;
            static int targetMinY = -10;
            static int targetMaxY = -5;
        static void Main(string[] args)
        {
            List<int> maxYs = new List<int>();

            for(int x = 1; x < 10; x++){
                for(int y = 1; y < 10; y++){
                    maxYs.Add(TestStep(x, y).Y);
                }
            }

            Console.WriteLine("Max Y: " + maxYs.Max());
        }

        static dynamic TestStep(int xStep, int yStep){
            var currentPostion = new Vector2(0, 0);
            var maxY = -1;
            while(currentPostion.X < targetMaxX && currentPostion.Y > targetMinY){
                currentPostion = new Vector2(currentPostion.X + xStep, currentPostion.Y + yStep);
                
                if(currentPostion.Y > maxY){
                    maxY = (int)currentPostion.Y;
                }

                if(currentPostion.X > 0){
                    currentPostion = new Vector2(currentPostion.X - 1, currentPostion.Y - 1);
                }
                else if(currentPostion.X < 0){
                    currentPostion = new Vector2(currentPostion.X + 1, currentPostion.Y - 1);
                }
                else{
                    currentPostion = new Vector2(currentPostion.X, currentPostion.Y - 1);
                }

                if(currentPostion.X > targetMinX && currentPostion.X < targetMaxX
                    && currentPostion.Y < targetMinY && currentPostion.Y > targetMaxY){
                        return new {Y = maxY};
                    }

                
            }
            return new {Y = -1};
        }
    }
}
