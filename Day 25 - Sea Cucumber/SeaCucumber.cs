using System;

namespace SeaCucumber
{
    public class Cucumber
    {
        public bool MoveLeft {get;set;}
        public int X {get;set;}
        public int Y {get;set;}
        public Cucumber[,] Map {get;set;}

        private bool canMove;
        public bool CheckCanMove(){
            if(MoveLeft){
                canMove = Map[(X + 1) % Map.GetLength(0), Y] == null;
            }
            else{
                canMove = Map[X, (Y+1) % Map.GetLength(1)] == null;
            }
            return canMove;
        }

        public bool Move(){
            if(!canMove) return false;
            if(MoveLeft){
                var newX = (X + 1) % Map.GetLength(0);
                Map[X, Y] = null;
                Map[newX, Y] = this;
                X = newX;
            }
            else{
                var newY = (Y+1) % Map.GetLength(1);
                Map[X,Y] = null;
                Map[X,newY] = this;
                Y = newY;
            }
            return true;
        }
    }
}
