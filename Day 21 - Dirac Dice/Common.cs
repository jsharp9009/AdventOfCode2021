using System;

namespace DiracDice
{
     public class Player : ICloneable{

        public Player(int StartPosition){
            Position = StartPosition;
        }

        private Player(){}
        public int Position{get;set;}
        public int Score{get;set;}

        public object Clone()
        {
            return new Player(){Position = Position, Score = Score};
        }
    }
}
