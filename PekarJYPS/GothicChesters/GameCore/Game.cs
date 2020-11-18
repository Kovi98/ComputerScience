using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace PekarJYPS
{
    public class Game
    {
        public Board Board { get; private set; }
        public bool IsActive { get; set; }
        public bool IsOver { get; private set; }
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
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public int RoundWithoutDead { get; private set; }
        private Player _playerOnMove;
        public Player PlayerOnMove
        {
            get => _playerOnMove;
            private set => _playerOnMove = value;
        }
        public Box MarkedBox { get; set; }
        public Move[] MovesMarkedBox
        {
            get
            {
                if (ForcedAttackBox is null)
                {
                    if (!(MarkedBox is null))
                    {
                        Move[] attacks = GetPossibleAttacks(Board, MarkedBox);
                        if (attacks.Length > 0)
                        {
                            return attacks;
                        }
                        Move[] moves = GetPossibleMoves(Board, MarkedBox);
                        if (moves.Length > 0)
                        {
                            return moves;
                        }
                    }
                    return new Move[0];
                }
                else
                {
                    return GetPossibleAttacks(Board, ForcedAttackBox);
                }
            }
        }
        public Box ForcedAttackBox { get; set; }
        public Game(int diff, Players whitePlayer, Players blackPlayer)
        {
            Difficulty = diff;
            Board = new Board();

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
            IsOver = false;
            IsActive = true;
            BoardHistory = new Dictionary<int, Board>();
            _playerOnMove = WhitePlayer;
        }

        public void DoMove(Move move)
        {
            if(IsActive && !IsOver)
            {
                Board.DoMove(move);
                if (!(!(move.AttackedPosition is null) && Board.GetPossibleAttacks(move.NextPosition).Length > 0))
                {
                    ForcedAttackBox = null;
                    ChangePlayer();
                }
                else
                {
                    ForcedAttackBox = move.NextPosition;
                    if (PlayerOnMove is AI)
                    {
                        //Thread thread = new Thread(() => ((AI)PlayerOnMove).Play(this));
                        //thread.Start();
                        ((AI)PlayerOnMove).Play(this);

                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Nelze udělat pohyb, když je hra ukončena, nebo pozastavena");
            }
        }

        public void ChangePlayer()
        {
            if (PlayerOnMove.Equals(WhitePlayer))
            {
                PlayerOnMove = BlackPlayer;
            }
            else if (PlayerOnMove.Equals(BlackPlayer))
            {
                PlayerOnMove = WhitePlayer;
                Round++;
            }
            else
            {
                throw new InvalidOperationException("Nemůže se změnit hráč, když žádný není");
            }
            if (PlayerOnMove is AI)
                ((AI)PlayerOnMove).Play(this);
        }

        private void BackupBoard() => BoardHistory.Add(Round, (Board)Board.Clone());

        public void Undo()
        {
            Board newBoard;
            if (!BoardHistory.TryGetValue(Round--, out newBoard))
                throw new InvalidOperationException("Nejdá dát UNDO když není v historii desek záznam s this.Round-1");

            Round--;
            Board = (Board)newBoard.Clone();
        }

        public void Redo()
        {
            Board newBoard;
            if (!BoardHistory.TryGetValue(Round++, out newBoard))
                throw new InvalidOperationException("Nejdá dát REDO když není v historii desek záznam s this.Round+1");

            Round++;
            Board = (Board)newBoard.Clone();
        }

        public Move[] GetPossibleMoves(Board board, Box box)
        {
            return board.GetPossibleMoves(box);
        }

        public Move[] GetPossibleAttacks(Board board, Box box)
        {
            return board.GetPossibleAttacks(box);
        }


    }
}