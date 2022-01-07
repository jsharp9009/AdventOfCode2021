using System;
using System.Collections.Generic;

namespace DiracDice
{
    public static class PartTwo
    {
        static Dictionary<int, int> RollFrequencies = new Dictionary<int, int>(){
            {3, 1},
            {4, 3},
            {5, 6},
            {6, 7},
            {7, 6},
            {8, 3},
            {9, 1}
        };

        public static void Solve(int startPosition1, int startPosition2)
        {
            var player1 = new Player(startPosition1);
            var player2 = new Player(startPosition2);

            var wins = GetWins(player2, player1, 0, true);

            Console.WriteLine("Player 1 Wins: " + wins.Item2);
            Console.WriteLine("Player 2 Wins: " + wins.Item1);
        }

        public static Tuple<long, long> GetWins(Player player1, Player player2, int diceScore, bool FirstRun){
            
            if(!FirstRun){
                player1.Position = (player1.Position + diceScore) % 10;

                if(player1.Position == 0) player1.Position = 10;

                player1.Score += player1.Position;

                if(player1.Score >= 21) return new Tuple<long, long>(1, 0);
            }
            long player1Wins = 0, player2Wins = 0;
            foreach(var pair in RollFrequencies){
                var wins = GetWins((Player)player2.Clone(), (Player)player1.Clone(), pair.Key, false);
                player2Wins += (wins.Item1 * pair.Value);
                player1Wins += (wins.Item2 * pair.Value);
            }

            return new Tuple<long, long>(player1Wins, player2Wins);
        }        
    }
}
