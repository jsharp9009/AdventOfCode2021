using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System;

namespace TrickShot
{
    class Program
    {
        static int targetMinX = 150;
            static int targetMaxX = 171;
            static int targetMinY = -70;
            static int targetMaxY = -129;
        static void Main(string[] args)
        {
            List<int> maxYs = new List<int>();

            var minX = MinimunX(targetMinX);

            var hit = 0;

            for(int x = minX; x <= targetMaxX; x++){
                for(int y = targetMaxY; y < -targetMaxY; y++){
                    var maxY =TestStep(x, y);
                    if(maxY != null) {
                        maxYs.Add(maxY.Value);
                        //Console.WriteLine(x + "," + y);
                        hit++;
                    }
                }
            }

            Console.WriteLine("Max Y: " + maxYs.Max());
            Console.WriteLine("Intitial Velocities that Hit: " + hit);
        }

        static int? TestStep(int xStep, int yStep){
            var currentX = 0;
            var currentY = 0;
            var maxY = -1;
            while(currentX <= targetMaxX && currentY >= targetMaxY){
                currentX += xStep;
                if(xStep > 0)
                    xStep--;

                currentY += yStep--;
                if(currentY > maxY){
                    maxY = currentY;
                }

                if(currentX >= targetMinX && currentX <= targetMaxX
                    && currentY <= targetMinY && currentY >= targetMaxY){
                        return maxY;
                    }

                
            }
            return null;
        }

        static int MinimunX(int leftBound)
        {
            var i = 0; 
            for (; i * (i + 1) < 2 * leftBound; i++)
                ;
            return i;
        }
    }
}
