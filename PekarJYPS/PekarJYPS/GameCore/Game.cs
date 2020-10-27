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
        public bool IsActive { get; set; }
        /// <summary>
        /// Kolo hry
        /// </summary>
        public int Round
        {
            get => _round;
            private set
            {
                BackupBoard();
                _round = value;
            }
        }
        private int _round = 1;
        public Dictionary<int, Board> BoardHistory { get; private set; }
        private int _difficulty;
        /// <summary>
        /// Obtížnost hry - 1, 2 nebo 3
        /// </summary>
        public int Difficulty
        {
            get => _difficulty;
            set
            {
                if (value>=1 && value<=3)
                {
                    _difficulty = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Difficulty", "Difficulty must be 1, 2 or 3");
                }
            }
        }
        public Player WhitePlayer { get; private set; }
        public Player BlackPlayer { get; private set; }
        public int RoundWithoutDead { get; private set; }
        private Player _playerOnMove;
        public Player PlayerOnMove 
        { 
            get => _playerOnMove;
            private set
            {
                _playerOnMove = value;
                Round++;
            }
        }
        public Game(int diff, Players whitePlayer, Players blackPlayer)
        {
            Difficulty = diff;

            Board = new Board(this);

            switch (whitePlayer)
            {
                case Players.Human:
                    WhitePlayer = new Human(PieceColor.White);
                    break;
                case Players.AI:
                    WhitePlayer = new AI(PieceColor.White);
                    break;
            }

            switch (blackPlayer)
            {
                case Players.Human:
                    BlackPlayer = new Human(PieceColor.Black);
                    break;
                case Players.AI:
                    BlackPlayer = new AI(PieceColor.Black);
                    break;
            }

            IsActive = true;
        }

        public void ChangePlayer(PieceColor color, Players player)
        {
            switch (color)
            {
                case PieceColor.White:
                    switch (player)
                    {
                        case Players.Human:
                            WhitePlayer = new Human(color);
                            break;
                        case Players.AI:
                            WhitePlayer = new AI(color);
                            break;
                    }
                    break;

                case PieceColor.Black:
                    switch (player)
                    {
                        case Players.Human:
                            BlackPlayer = new Human(color);
                            break;
                        case Players.AI:
                            BlackPlayer = new AI(color);
                            break;
                    }
                    break;

            }
        }

        private void BackupBoard() => BoardHistory.Add(Round, (Board)Board.Clone());
    }
}