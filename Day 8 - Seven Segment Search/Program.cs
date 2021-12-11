using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenSegmentSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            var notes = new List<NoteEntry>();
            foreach(string s in input){
                notes.Add(ReadInputLine(s));
            }

            var sevens = notes.Sum(a => a.Output.Count(s => s.Length == 3));
            var four = notes.Sum(a => a.Output.Count(s => s.Length == 4));
            var ones = notes.Sum(a => a.Output.Count(s => s.Length == 2));
            var eight = notes.Sum(a => a.Output.Count(s => s.Length == 7));

            Console.WriteLine("Total occurences of 1,4,7 or 8: " + (sevens + four + ones + eight));

            int total = 0;
            foreach(var note in notes){
                var result = Decode(note);
                total += result;
            }

            Console.WriteLine("Total Value of all Outputs: " + total);

        }

        static NoteEntry ReadInputLine(string line){
            var parts = line.Split('|');

            NoteEntry note = new NoteEntry();
            note.UniqueSignalPatterns = parts[0].Split(' ');
            note.Output = parts[1].Split(' ');

            return note;
        }

        static int Decode(NoteEntry Note){
            var one = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 2).FirstOrDefault().OrderBy(c => c));
            var four = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 4).FirstOrDefault().OrderBy(c => c));
            var seven = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 3).FirstOrDefault().OrderBy(c => c));
            var eight  = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 7).FirstOrDefault().OrderBy(c => c));

            var nine = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 6 && s.Intersect(four).Count() == 4).FirstOrDefault().OrderBy(c => c));
            var zero = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 6 && s.Intersect(four).Count() == 3 && s.Intersect(seven).Count() == 3).FirstOrDefault().OrderBy(c => c));
            var six = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 6 && s.Intersect(four).Count() == 3 && s.Intersect(seven).Count() != 3).FirstOrDefault().OrderBy(c => c));

            var five = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 5 && s.Intersect(six).Count() == 5).FirstOrDefault().OrderBy(c => c));
            var two = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 5 && s.Intersect(nine).Count() == 4).FirstOrDefault().OrderBy(c => c));
            var three = String.Concat(Note.UniqueSignalPatterns.Where(s => s.Length == 5 && s.Intersect(nine).Count() == 5 && s.Intersect(six).Count() != 5).FirstOrDefault().OrderBy(c => c));

            var builder = new StringBuilder();

            foreach(string s in Note.Output){
                var ordered = string.Concat(s.OrderBy(c => c));
                if(ordered.Equals(zero, StringComparison.InvariantCultureIgnoreCase)) builder.Append("0");
                if(ordered.Equals(one, StringComparison.InvariantCultureIgnoreCase)) builder.Append("1");
                if(ordered.Equals(two, StringComparison.InvariantCultureIgnoreCase)) builder.Append("2");
                if(ordered.Equals(three, StringComparison.InvariantCultureIgnoreCase)) builder.Append("3");
                if(ordered.Equals(four, StringComparison.InvariantCultureIgnoreCase)) builder.Append("4");
                if(ordered.Equals(five, StringComparison.InvariantCultureIgnoreCase)) builder.Append("5");
                if(ordered.Equals(six, StringComparison.InvariantCultureIgnoreCase)) builder.Append("6");
                if(ordered.Equals(seven, StringComparison.InvariantCultureIgnoreCase)) builder.Append("7");
                if(ordered.Equals(eight, StringComparison.InvariantCultureIgnoreCase)) builder.Append("8");
                if(ordered.Equals(nine, StringComparison.InvariantCultureIgnoreCase)) builder.Append("9");
            }
            //Console.WriteLine(builder.ToString());
            return Convert.ToInt32(builder.ToString());
        }
    }

    class NoteEntry
    {
        public string[] UniqueSignalPatterns {get;set;}
        public string[] Output{get;set;}
    }
}
