﻿using System;

namespace DiracDice
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player1 = new Player(6);
            Player player2 = new Player(2);
            DetermanisticDice dice = new DetermanisticDice();

            while(true){
                player1 = PlayRound(player1, dice);

                if(player1.Score >= 1000) break;

                player2 = PlayRound(player2, dice);

                if(player2.Score >= 1000) break;
            }

            var loserScore = player1.Score > player2.Score ? player2.Score : player1.Score;
            Console.WriteLine("Losing Score: " + loserScore);
            Console.WriteLine("Rolls: " + dice.Rolls);
            Console.WriteLine("Times rolls: " + (loserScore * dice.Rolls));

        }

        static Player PlayRound(Player player, DetermanisticDice dice){
            var diceScore = 0;
            for(int i = 0; i < 3; i++){
                diceScore += dice.RollDice();
            }

            player.Position = (player.Position + diceScore) % 10;

            if(player.Position == 0) player.Position = 10;

            player.Score += player.Position;

            return player;
        }
    }

    public class DetermanisticDice{
        public int Rolls{get;set;}
        
        public int RollDice(){
            return (++Rolls) % 100;
        }
    }

    public class Player{

        public Player(int StartPosition){
            Position = StartPosition;
        }
        public int Position{get;set;}
        public int Score{get;set;}
    }
}
