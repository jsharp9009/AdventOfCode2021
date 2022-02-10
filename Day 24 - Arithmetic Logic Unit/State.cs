using System;
using System.Collections.Generic;
using System.Linq;

namespace ALU
{
    public class State : ICloneable
    {
        public State Previous{get;set;}
        public List<Command> Commands {get;set;}
        public Dictionary<char, int> Variables {get;set;} = new Dictionary<char, int>{
            {'w', 0},
            {'x', 0},
            {'y', 0},
            {'z', 0},
        };
        public int Input {get;set;}

        public override bool Equals(object obj)
        {
            if(!(obj is State) || obj == null) return false;

            var other = (State)obj;

            return other.Commands == Commands
                && other.Variables['z'] == this.Variables['z']
                && other.Input == Input;
        }

        public object Clone()
        {
            return new State(){
                Commands = this.Commands,
                Variables = this.Variables.ToDictionary(k => k.Key, v => v.Value),
                Input = this.Input
            };
        }

        public override int GetHashCode()
        {
            int hash = 1;
            foreach(Command cmd in Commands){
                hash = hash ^ cmd.GetHashCode();
            }

            hash ^= Variables['z'];

            return hash ^ Input;
        }

        public override string ToString()
        {
            return Previous?.ToString() + Input;
        }
    }
}
