using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphipod
{
    public class Room
    {
        public Stack<char> room;
        public int Size {get; private set;}
        public char RoomOf{get; private set;}

        public int OccupiedSize {get{return room.Count;}}

        public int StepsToEnter {get{return Size - OccupiedSize;}}

        public int StepsToExit {get{return StepsToEnter + 1;}}
        public static Dictionary<int, char>  roomsPosOf;

        static Room(){
            roomsPosOf = new Dictionary<int, char>();
            roomsPosOf.Add(0, 'A');
            roomsPosOf.Add(1, 'B');
            roomsPosOf.Add(2, 'C');
            roomsPosOf.Add(3, 'D');
        }

        public  Room(string room, int roomOf){
            this.room = new Stack<char>();
            for(int i = room.Length -1; i >= 0; i--){
                var leter = room[i];
                if(leter != '.'){
                    this.room.Push(leter);
                }
                Size = room.Length;
                RoomOf = roomsPosOf[roomOf];
            }
        }

        public Room(Room other){
            this.room = new Stack<char>(other.room.Reverse());
            this.Size = other.Size;
            this.RoomOf = other.RoomOf;
        }

        public void Push(char letter){
            room.Push(letter);
        }

        public char Pop(){
            return room.Pop();
        }

        public char Peek(){
            return room.Peek();
        }

        public bool isClean(){
            return room.All(a => a == RoomOf);
        }

        public override string ToString()
        {
            string empty = string.Join("", Enumerable.Repeat(".", Size - OccupiedSize));
            var occupants = new StringBuilder();
            foreach(var c in room){
                occupants.Append(c);
            }
            return empty + occupants.ToString();
        }
    }
}
