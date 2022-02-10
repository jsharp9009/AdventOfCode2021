using System;

namespace ALU
{
    public class Command
    {
        public string Instruction{get;set;}
        public char Param1 {get;set;}
        public int Param2{get;set;}
        public bool Param2IsChar{get;set;}

        private int hash{get;set;} = 0;

        public override bool Equals(object obj)
        {
            if(!(obj is Command) || obj == null) return false;

            var other = (Command)obj;

            return other.Instruction == Instruction && other.Param1 == Param1
                && other.Param2 == Param2 && other.Param2IsChar == Param2IsChar;
        }

        public override int GetHashCode()
        {
            if(hash == 0)
                hash = Instruction.GetHashCode() ^ (int)Param1 ^ Param2 ^ (Param2IsChar ? 1 : 0);
            return hash;
        }

        public override string ToString()
        {
            if(Param2IsChar)
                return Instruction + " " + Param1 + " " + (char)Param2;
            else
                return Instruction + " " + Param1 + " " + Param2;
        }
    }
}
