using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Amphipod
{
    public class BurrowState
    {
        private string state;
        public int roomSize {get; private set;}
        public Hallway hallway {get; private set;}
        public int cost {get;set;}
        private Room[] rooms = new Room[4];

        private static Dictionary<int, int> positionOfRoomInHallway;

        public static Dictionary<char, int> amphipodCost;

        public BurrowState previousState {get;set;}

        public int InBurrow{get{
            return hallway.hallway.Count(c => c != '.')
                + rooms.SelectMany(r => r.room).Count(c => c != '.');
        }}

        static BurrowState(){
            amphipodCost = new Dictionary<char, int>();
            amphipodCost.Add('A', 1);
            amphipodCost.Add('B', 10);
            amphipodCost.Add('C', 100);
            amphipodCost.Add('D', 1000);

            positionOfRoomInHallway = new Dictionary<int, int>();
            positionOfRoomInHallway.Add(0, 2);
            positionOfRoomInHallway.Add(1, 4);
            positionOfRoomInHallway.Add(2, 6);
            positionOfRoomInHallway.Add(3, 8);
        }

        public BurrowState(string[] state){
            roomSize = state.Length - 3;
            this.state = string.Join("\n", state);
            hallway = new Hallway(state[1].Substring(1, 11));
            for(int i = 0; i < 4; i++){
                var builder = new StringBuilder();
                for(int n = 2; n < state.Length-1; n++){
                    builder.Append(state[n][2*(i+1)+1]);
                }
                rooms[i] = new Room(builder.ToString(), i);
            }
        }

        public BurrowState(BurrowState other): this(other.state.Split("\n")){}

        public Room[] GetRoomCopy(){
            return rooms.Select(r => new Room(r)).ToArray();
        }

        public List<BurrowState> GetNextValidStates(){
            var states = new List<BurrowState>();
            foreach(var move in hallway.getValidMovesFromCharToRoom()){
                var roomId = Room.roomsPosOf.First(a => a.Value == move.Item2).Key;
                if(rooms[roomId].isClean()){
                    var cost = (move.Item3 + rooms[roomId].StepsToEnter) * amphipodCost[move.Item2];
                    var newRooms = GetRoomCopy();
                    newRooms[roomId].Push(move.Item2);
                    BurrowState state = encodeHallwayRooms(move.Item1, newRooms);
                    state.cost = (this.cost + cost);
                    state.previousState = this;
                    states.Add(state);
                }
            }

            if(states.Count > 0) return states;

            for(int i = 0; i < 4; i++){
                if(!rooms[i].isClean()){
                    foreach(var move in hallway.GetValidMoves(positionOfRoomInHallway[i], rooms[i].Peek())){
                        var cost = (move.Item2 + rooms[i].StepsToExit) * amphipodCost[rooms[i].Peek()];
                        var newRooms = GetRoomCopy();
                        newRooms[i].Pop();
                        var newState = encodeHallwayRooms(move.Item1, newRooms);
                        newState.cost = this.cost + cost;
                        newState.previousState = this;
                        states.Add(newState);
                    }
                }
            }

            return states;
        }

        public static BurrowState encodeHallwayRooms(Hallway hallway, Room[] rooms){
            var state = new List<string>();
            state.Add(string.Join("", Enumerable.Repeat("#", 13)));
            state.Add("#" + hallway.hallway + "#");

            var room1 = rooms[0].ToString();
            var room2 = rooms[1].ToString();
            var room3 = rooms[2].ToString();
            var room4 = rooms[3].ToString();

            state.Add("###" + room1[0] + "#" + room2[0] + "#" + room3[0] + "#" + room4[0] + "###");        

            for(int i = 1; i < room1.Length; i++){
                state.Add("  #" + room1[i] + "#" + room2[i] + "#" + room3[i] + "#" + room4[i] + "#");                
            }
            state.Add("  #########");
            return new BurrowState(state.ToArray());
        }

        public override string ToString()
        {
            return state;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is BurrowState)) return false;

            return ((BurrowState)obj).state.Equals(this.state);
        }

        public override int GetHashCode()
        {
            return state.GetHashCode();
        }

        public static BurrowState GetFinalBurrowState(BurrowState initialState){
            var roomSize = initialState.roomSize;
            var state = new List<string>();
            state.Add(string.Join("", Enumerable.Repeat("#", 13)));
            state.Add("#" + string.Join("", Enumerable.Repeat(".", 11)) + "#");
            state.Add("###A#B#C#D###");
            for(int i = 1; i < roomSize; i++){
                state.Add("  #A#B#C#D#");
            }
            state.Add("  #########");

            return new BurrowState(state.ToArray());
        }
    }
}
