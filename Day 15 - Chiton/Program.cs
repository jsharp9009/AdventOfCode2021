using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;


namespace Chiton
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            int[,] chitonmap = new int[lines.Count(), lines[0].Length];

            for(int i = 0; i < lines.Count(); i++){
                for(int n = 0; n < lines[0].Length; n++){
                    chitonmap[i, n] = int.Parse(lines[i][n].ToString());
                }
            }

            var values = Dijkstra_MapPath(chitonmap, new Vector2(0, 0), new Vector2(chitonmap.GetLength(0) - 1,chitonmap.GetLength(1) - 1));
            Console.WriteLine("Safest Route Part 1: " + values.distance);

            int[,] fullChitonMap = buildFullMap(chitonmap);


            values = Dijkstra_MapPath(fullChitonMap, new Vector2(0, 0), new Vector2(fullChitonMap.GetLength(0) - 1,fullChitonMap.GetLength(1) - 1));
            Console.WriteLine("Safest Route Part 2: " + values.distance);            

            // for(int i = 0; i < fullChitonMap.GetLength(0); i++){
            //     for(int n = 0; n < fullChitonMap.GetLength(1); n++){
            //         Console.Write(fullChitonMap[i,n]);
            //     }
            //     Console.WriteLine("");
            // }

        }

        static dynamic BellmanFord_MapPath(int[,] chitonMap, Vector2 source, Vector2 end){
            var distances = new Dictionary<Vector2, long>();
            var previous = new Dictionary<Vector2, Vector2>();

            for(int i = 0; i < chitonMap.GetLength(0); i++){
                for(int n = 0; n < chitonMap.GetLength(1); n++){
                    distances.Add(new Vector2(i, n), int.MaxValue);
                }
            }

            distances[source] = 0;

            for(int i = 0; i < chitonMap.GetLength(0); i++){
                for(int n = 0; n < chitonMap.GetLength(1); n++){
                    
                    if(n+1 < chitonMap.GetLength(1)){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i, n + 1];
                        var oldPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n + 1).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i && v.Key.Y == n + 1).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i && v.Key.Y == n + 1))
                                previous[previous.First(v => v.Key.X == i && v.Key.Y == n + 1).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i, n+1), new Vector2(i, n));
                        }
                    }

                    if(n-1 >= 0){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i, n - 1];
                        var oldPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n - 1).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i && v.Key.Y == n - 1).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i && v.Key.Y == n - 1))
                                previous[previous.First(v => v.Key.X == i && v.Key.Y == n - 1).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i, n-1), new Vector2(i, n));
                        }
                    }

                    if(i+1 < chitonMap.GetLength(0)){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i + 1, n];
                        var oldPathLength = distances.First(v => v.Key.X == i + 1 && v.Key.Y == n).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i + 1 && v.Key.Y == n).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i+1 && v.Key.Y == n))
                                previous[previous.First(v => v.Key.X == i+1 && v.Key.Y == n).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i+1, n), new Vector2(i, n));
                        }
                    }

                    if(i-1 >= 0){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i-1, n];
                        var oldPathLength = distances.First(v => v.Key.X == i-1 && v.Key.Y == n).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i-1 && v.Key.Y == n).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i-1 && v.Key.Y == n))
                                previous[previous.First(v => v.Key.X == i-1 && v.Key.Y == n).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i-1, n), new Vector2(i, n));
                        }
                    }
                }
            }

            // for(int i = 0; i < chitonMap.GetLength(0); i++){
            //     for(int n = 0; n < chitonMap.GetLength(1); n++){
            //         var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i, n + 1];
            //         var oldPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n + 1).Value;

            //         if(newPathLength < oldPathLength){
            //             return null;
            //         }
            //     }
            // }

            return new {distance = distances.First(v => v.Key.X == end.X && v.Key.Y == end.Y).Value, path = previous};
        }

        static int[,] buildFullMap(int[,] chitonMap){
            int[,] fullChitonMap = new int[chitonMap.GetLength(0) * 5, chitonMap.GetLength(1) * 5];
            
            for(int x = 0; x < 5; x++){
                for(int y = 0; y < 5; y++){
                    for(int i = 0; i < chitonMap.GetLength(0); i++){
                        for(int n = 0; n < chitonMap.GetLength(1); n++){
                            var fullX = i + (x * chitonMap.GetLength(0));
                            var fullY = n + (y * chitonMap.GetLength(1));
                            fullChitonMap[fullX,fullY] = chitonMap[i, n] + x + y;
                            if(fullChitonMap[fullX, fullY] > 9)
                                fullChitonMap[fullX, fullY] = fullChitonMap[fullX, fullY] - 9;
                        }
                    }
                }
            }
            return fullChitonMap;
        }

        static dynamic BFS_MapPath(int[,] chitonMap, Vector2 source, Vector2 end){
            var previous = new Dictionary<Vector2, Vector2>();
            var visited = new List<Vector2>();
            var queue = new Queue<VectorDistance>();
            queue.Enqueue(new VectorDistance(){node = new Vector2(0, 0), distance = 0});

            while(queue.Count > 0){
                var current = queue.Dequeue();

                if(current.node.X == end.X && current.node.Y == end.Y){
                    return new {distance = current.distance, path = previous};
                }

                if(current.node.X - 1 >= 0){
                    Vector2 neighbor = new Vector2(current.node.X - 1, current.node.Y);
                    if(!visited.Any(v => v.X == neighbor.X && v.Y == neighbor.Y)){
                        if(previous.Any(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y))
                                previous[previous.First(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y).Key] = current.node;
                            else
                                previous.Add(neighbor, current.node);
                        queue.Enqueue(new VectorDistance(){node = neighbor, distance = current.distance + (chitonMap[(int)neighbor.X, (int)neighbor.Y]) });
                        visited.Add(neighbor);
                    }
                }

                if(current.node.X + 1 < chitonMap.GetLength(0)){
                    Vector2 neighbor = new Vector2(current.node.X + 1, current.node.Y);
                    if(!visited.Any(v => v.X == neighbor.X && v.Y == neighbor.Y)){
                        if(previous.Any(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y))
                                previous[previous.First(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y).Key] = current.node;
                            else
                                previous.Add(neighbor, current.node);
                        queue.Enqueue(new VectorDistance(){node = neighbor, distance = current.distance + (chitonMap[(int)neighbor.X, (int)neighbor.Y]) });
                        visited.Add(neighbor);
                    }
                }

                if(current.node.Y - 1 >= 0){
                    Vector2 neighbor = new Vector2(current.node.X, current.node.Y - 1);
                    if(!visited.Any(v => v.X == neighbor.X && v.Y == neighbor.Y)){
                        if(previous.Any(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y))
                                previous[previous.First(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y).Key] = current.node;
                            else
                                previous.Add(neighbor, current.node);
                        queue.Enqueue(new VectorDistance(){node = neighbor, distance = current.distance + (chitonMap[(int)neighbor.X, (int)neighbor.Y]) });
                        visited.Add(neighbor);
                    }
                }

                if(current.node.Y + 1 < chitonMap.GetLength(1)){
                    Vector2 neighbor = new Vector2(current.node.X, current.node.Y + 1);
                    if(!visited.Any(v => v.X == neighbor.X && v.Y == neighbor.Y)){
                        if(previous.Any(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y))
                                previous[previous.First(v => v.Key.X == neighbor.X && v.Key.Y == neighbor.Y).Key] = current.node;
                            else
                                previous.Add(neighbor, current.node);
                        queue.Enqueue(new VectorDistance(){node = neighbor, distance = current.distance + (chitonMap[(int)current.node.X, (int)current.node.Y]) });
                        visited.Add(neighbor);
                    }
                }
            }

            return new {distance = -1, path = previous};
        }

        static dynamic Dijkstra_MapPath(int[,] chitonMap, Vector2 source, Vector2 end){
            var distances = new Dictionary<Vector2, long>();
            var previous = new Dictionary<Vector2, Vector2>();
            var remaining = new Dictionary<Vector2, long>();

            for(int i = 0; i < chitonMap.GetLength(0); i++){
                for(int n = 0; n < chitonMap.GetLength(1); n++){
                    var vector = new Vector2(i, n);
                    
                    if(i == 0 && n == 0){
                        distances.Add(vector, 0);
                        remaining.Add(vector, 0);
                        continue;
                    }
                    distances.Add(vector, int.MaxValue);
                    remaining.Add(vector, int.MaxValue);
                }
            }

            while(remaining.Count > 1){
                var current = remaining.Aggregate((curMin, x) => x.Value < curMin.Value ? x : curMin);
                remaining.Remove(current.Key);

                var i = (int)current.Key.X;
                var n = (int)current.Key.Y;

                if(n+1 < chitonMap.GetLength(1)){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i, n + 1];
                        var oldPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n + 1).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i && v.Key.Y == n + 1).Key] = newPathLength;
                            remaining[remaining.First(v => v.Key.X == i && v.Key.Y == n + 1).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i && v.Key.Y == n + 1))
                                previous[previous.First(v => v.Key.X == i && v.Key.Y == n + 1).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i, n+1), new Vector2(i, n));
                        }
                    }

                    if(n-1 >= 0){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i, n - 1];
                        var oldPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n - 1).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i && v.Key.Y == n - 1).Key] = newPathLength;
                            remaining[remaining.First(v => v.Key.X == i && v.Key.Y == n - 1).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i && v.Key.Y == n - 1))
                                previous[previous.First(v => v.Key.X == i && v.Key.Y == n - 1).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i, n-1), new Vector2(i, n));
                        }
                    }

                    if(i+1 < chitonMap.GetLength(0)){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i + 1, n];
                        var oldPathLength = distances.First(v => v.Key.X == i + 1 && v.Key.Y == n).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i + 1 && v.Key.Y == n).Key] = newPathLength;
                            remaining[remaining.First(v => v.Key.X == i + 1 && v.Key.Y == n).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i+1 && v.Key.Y == n))
                                previous[previous.First(v => v.Key.X == i+1 && v.Key.Y == n).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i+1, n), new Vector2(i, n));
                        }
                    }

                    if(i-1 >= 0){
                        var newPathLength = distances.First(v => v.Key.X == i && v.Key.Y == n).Value + chitonMap[i-1, n];
                        var oldPathLength = distances.First(v => v.Key.X == i-1 && v.Key.Y == n).Value;

                        if(newPathLength < oldPathLength){
                            distances[distances.First(v => v.Key.X == i-1 && v.Key.Y == n).Key] = newPathLength;
                            remaining[remaining.First(v => v.Key.X == i-1 && v.Key.Y == n).Key] = newPathLength;
                            if(previous.Any(v => v.Key.X == i-1 && v.Key.Y == n))
                                previous[previous.First(v => v.Key.X == i-1 && v.Key.Y == n).Key] = new Vector2(i, n);
                            else
                                previous.Add(new Vector2(i-1, n), new Vector2(i, n));
                        }
                    }
            }

            return new {distance =  distances[distances.First(v => v.Key.X == end.X && v.Key.Y == end.Y).Key],
                path= previous};
        }
    }

    public class VectorDistance{
        public Vector2 node {get;set;}
        public int distance {get;set;}
    }
}
