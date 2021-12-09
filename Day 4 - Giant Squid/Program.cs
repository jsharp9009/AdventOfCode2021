using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace GiantSquid
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToList();
            var DrawNumbers = ReadDrawNumbers(lines[0]);

            lines.RemoveAt(0);

            lines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

            var boards = ReadBoards(lines);

            List<BingoBoard> winningBoards = new List<BingoBoard>();
            int lastNum = 0;
            while(DrawNumbers.Count() > 0){
                lastNum = DrawNumbers.Dequeue();

                boards.ForEach(b => b.MarkSquare(lastNum));

                winningBoards.AddRange(boards.Where(b => b.HasWon()));
                boards.RemoveAll(b => b.HasWon());

                if(boards.Count() == 0) break;
            }
            Console.WriteLine("Last Number Called: " + lastNum);
            Console.WriteLine("Winning Board Score: " + winningBoards.FirstOrDefault().GetScore());

            Console.WriteLine("Last Winning Board Score: " + winningBoards.LastOrDefault().GetScore());
        }

        static BingoBoard NewBoard(){
            var boardSquares = new BoardSquare[5][];

            boardSquares[0] = new BoardSquare[5] {new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare()};
            boardSquares[1] = new BoardSquare[5]{new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare()};
            boardSquares[2] = new BoardSquare[5]{new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare()};
            boardSquares[3] = new BoardSquare[5]{new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare()};
            boardSquares[4] = new BoardSquare[5]{new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare(), new BoardSquare()};

            return new BingoBoard(){ Board = boardSquares};
        }

        static Queue<int> ReadDrawNumbers(string input){
            var numbers = input.Split(',');
            var queue = new Queue<int>();

            foreach(var num in numbers){
                queue.Enqueue(Convert.ToInt32(num.Trim()));
            }

            return queue;
        }

        static List<BingoBoard> ReadBoards(List<string> lines){
            List<BingoBoard> boards = new List<BingoBoard>();
            for(int i = 0; i < lines.Count(); i++){
                if(i % 5  == 0)
                    boards.Add(NewBoard());
                for(int n = 0; n < lines[i].Length; n+=3){
                    var stringNum = lines[i][n].ToString() + lines[i][n+1].ToString();
                    var intNum = Convert.ToInt32(stringNum.Trim());
                    boards.Last().Board[i%5][n/3].Number = intNum;
                }
            }
            return boards;
        }
    }

    class BoardSquare{
        public int Number {get;set;}
        public bool Marked {get;set;} = false;
    }

    class BingoBoard {
        public BoardSquare[][] Board {get;set;}

        public int GetScore(){
            return this.Board.Sum(r => r.Sum(s => s.Marked ? 0 : s.Number));
        }

        public void MarkSquare(int number){
            var squares = (from row in Board
                        from square in row
                        where square.Number == number
                        select square).ToList();
            squares.ForEach(s => s.Marked = true);
        }

        public bool HasWon(){
            if(Board.Any(r => r.All(b => b.Marked)))
                return true;

            for(int i = 0; i < Board[0].Count(); i++){
                if(Board.All(r => r[i].Marked)){
                    return true;
                }
            }

            return false;
        }
    }
}
