using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekarJYPS
{
    public class Game
    {
        public Board Board { get; private set; }
        public bool IsActive { get; private set; }
        public int Round { get; private set; }
        private int difficulty;
        /// <summary>
        /// Obtížnost hry - 1, 2 nebo 3
        /// </summary>
        public int Difficulty
        {
            get => difficulty;
            set
            {
                if (value>=1 && value<=3)
                {
                    difficulty = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Difficulty", "Difficulty must be 1, 2 or 3");
                }
            }
        }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public int RoundWithoutDead { get; private set; }

        public Game(int diff, Players whitePlayer, Players blackPlayer)
        {
            Round = 1;
            Difficulty = diff;

            Board = new Board(this);

            switch (whitePlayer)
            {
                case Players.Human:
                    WhitePlayer = new Human();
                    break;
                case Players.AI:
                    WhitePlayer = new AI();
                    break;
            }

            switch (blackPlayer)
            {
                case Players.Human:
                    BlackPlayer = new Human();
                    break;
                case Players.AI:
                    BlackPlayer = new AI();
                    break;
            }

            IsActive = true;
        }
    }
}