using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Amphipod
{
    public class Hallway
    {
        public string hallway {get;private set;}

        public static readonly Dictionary<char, int> hallwayRoomPosition;

        static Hallway(){
            hallwayRoomPosition = new Dictionary<char, int>();
            hallwayRoomPosition.Add('A', 2);
            hallwayRoomPosition.Add('B', 4);
            hallwayRoomPosition.Add('C', 6);
            hallwayRoomPosition.Add('D', 8);
        }

        public Hallway(string hallway){
            this.hallway = hallway;
        }

        public Hallway(Hallway other){
            this.hallway = other.hallway;
        }

        public List<Tuple<Hallway, char, int>> getValidMovesFromCharToRoom(){
            var validMoves = new List<Tuple<Hallway, char, int>>();
            var dotReg = new Regex("[ABCD]");
            for(int i = 0; i<hallway.Length;i++){
                if("ABCD".Contains(hallway[i])){
                    char amphipod = hallway[i];
                    int position = i;
                    int roomPosition = hallwayRoomPosition[amphipod];

                    if(position < roomPosition && !dotReg.IsMatch(hallway.Substring(position + 1, (roomPosition + 1) - (position + 1)))){
                        var sb = new StringBuilder(hallway);
                        sb[i] = '.';
                        validMoves.Add(new Tuple<Hallway, char, int>(new Hallway(sb.ToString()), amphipod, roomPosition - position));
                    }
                    else if(position > roomPosition && !dotReg.IsMatch(hallway.Substring(roomPosition, position - roomPosition))){
                        var sb = new StringBuilder(hallway);
                        sb[i] = '.';
                        validMoves.Add(new Tuple<Hallway, char, int>(new Hallway(sb.ToString()), amphipod, position - roomPosition));
                    }
                }
            }
            return validMoves;
        }

        public List<Tuple<Hallway, int>> GetValidMoves(int position, char amphipod){
            int i = position - 1;
            var moves = new List<Tuple<Hallway, int>>();
            while(i>=0 && hallway[i]=='.'){
                if(!isRoomPosition(i)){
                    var sb = new StringBuilder(hallway);
                    sb[i] = amphipod;
                    moves.Add(new Tuple<Hallway, int>(new Hallway(sb.ToString()), position - i));
                }
                i--;
            }

            i = position + 1;
            while(i< hallway.Length && hallway[i] == '.'){
                if(!isRoomPosition(i)){
                    var builder = new StringBuilder(hallway);
                    builder[i] = amphipod;
                    moves.Add(new Tuple<Hallway, int>(new Hallway(builder.ToString()), i - position));
                }
                i++;
            }
            return moves;
        }

        static bool isRoomPosition(int position){
            return position >= 2 && position <= 8 && position%2 ==0;
        }
    }
}
