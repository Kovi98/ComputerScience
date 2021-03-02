using GothicChesters.GameCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;

namespace GothicChesters
{
    public delegate void BoardChangeHandler();
    public delegate void GameOverHandler();

    public class Game
    {
        public Board Board { get; set; }
        public bool IsActive { get; set; }
        public bool IsOver { get; set; }
        public event BoardChangeHandler OnAfterBoardChange;
        public event GameOverHandler OnAfterGameOver;
        public int Round { get; set; }
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
        private Player _whitePlayer;
        public Player WhitePlayer
        {
            get => _whitePlayer;
            set
            {
                _whitePlayer = value;
                if (PlayerOnMove.Color.Equals(PieceColor.White))
                    PlayerOnMove = value;
            }
        }
        private Player _blackPlayer;
        public Player BlackPlayer
        {
            get => _blackPlayer;
            set
            {
                _blackPlayer = value;
                if (PlayerOnMove.Color.Equals(PieceColor.Black))
                    PlayerOnMove = value;
            }
        }
        public Player EnemyPlayer
        {
            get
            {
                switch (PlayerOnMove.Color)
                {
                    case PieceColor.White:
                        return BlackPlayer;
                    case PieceColor.Black:
                        return WhitePlayer;
                }
                return null;
            }
        }
        public int RoundWithoutDead { get; private set; }
        public Player PlayerOnMove { get; set; }
        public Player Winner
        {
            get
            {
                if (Board.WhiteDead == 16)
                    return BlackPlayer;
                if (Board.BlackDead == 16)
                    return WhitePlayer;
                return null;
            }
        }
        public Box ForcedAttackBox { get; set; }
        public Game(int diff, Players whitePlayer, Players blackPlayer)
        {
            Difficulty = diff;
            Board = new Board();
            Round = 0;

            switch (whitePlayer)
            {
                case Players.Human:
                    _whitePlayer = new Human(PieceColor.White);
                    break;
                case Players.AI:
                    _whitePlayer = new AI(PieceColor.White);
                    break;
            }

            switch (blackPlayer)
            {
                case Players.Human:
                    _blackPlayer = new Human(PieceColor.Black);
                    break;
                case Players.AI:
                    _blackPlayer = new AI(PieceColor.Black);
                    break;
            }
            IsOver = false;
            BoardHistory = new Dictionary<int, Board>();
            PlayerOnMove = WhitePlayer;
            BackupBoard();
            Round++;
            IsActive = true;
        }

        public async void DoMove(Move move)
        {
            if (IsActive && !IsOver && !(move is null))
            {
                if (BoardHistory.ContainsKey(Round))
                {
                    for (int i = Round; i <= BoardHistory.Keys.Max(); i++)
                    {
                        BoardHistory.Remove(i);
                    }
                }
                Board.DoMove(move);
                //30 RoundWithoutDead
                if (RoundWithoutDead >= 30)
                {
                    OnAfterBoardChange?.Invoke();
                    IsOver = true;
                    if (OnAfterGameOver != null)
                        OnAfterGameOver();
                    return;
                }

                if (!(!(move.AttackedPosition is null) && Board.GetPossibleAttacks(move.NextPosition).Length > 0))
                {
                    ForcedAttackBox = null;
                    if (PlayerOnMove==BlackPlayer)
                    {
                        if (!(move.AttackedPosition is null) && move.AttackedPosition.Count() > 0)
                        {
                            RoundWithoutDead = 0;
                        }
                        else
                        {
                            RoundWithoutDead++;
                        }
                    }
                    ChangePlayer();
                }
                else
                {
                    ForcedAttackBox = move.NextPosition;
                }
            }
            if (!(Winner is null))
            {
                OnAfterBoardChange?.Invoke();
                IsOver = true;
                if (OnAfterGameOver != null)
                    OnAfterGameOver();
                return;
            }
            OnAfterBoardChange?.Invoke();
            if (move is null)
                ChangePlayer();
            if (PlayerOnMove is AI && IsActive && !IsOver)
            {
                AI player = (AI)PlayerOnMove;
                await player.PlayAsync(this);
            }
        }

        public void ChangePlayer()
        {
            if (PlayerOnMove == WhitePlayer)
            {
                PlayerOnMove = BlackPlayer;

            }
            else if (PlayerOnMove == BlackPlayer)
            {
                BackupBoard();
                Round++;
                PlayerOnMove = WhitePlayer;
            }
            else
            {
                throw new InvalidOperationException("Není žádný hráč na tahu!");
            }
        }

        public void BackupBoard() => BoardHistory.Add(Round, (Board)Board.Clone());

        public void Undo()
        {
            Board newBoard;
            if (!BoardHistory.TryGetValue(Round-2, out newBoard))
                throw new InvalidOperationException("V historii desek není žádný záznam s Round-1");

            Round--;
            Board = (Board)newBoard.Clone();
        }

        public void Redo()
        {
            Board newBoard;
            if (!BoardHistory.TryGetValue(Round, out newBoard))
                throw new InvalidOperationException("V historii desek není žádný záznam s Round+1");

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

        public static XElement GetXML(Game game)
        {
            try
            {
                XElement gameXML = new XElement("Game",
                new XElement("IsActive", game.IsActive),
                new XElement("IsOver", game.IsOver),
                new XElement("Round", game.Round),
                new XElement("RoundWithoutDead", game.RoundWithoutDead),
                new XElement("Difficulty", game.Difficulty),
                new XElement("WhitePlayer", game.WhitePlayer.GetPlayerType()),
                new XElement("BlackPlayer", game.BlackPlayer.GetPlayerType()),
                new XElement("PlayerOnMove", game.PlayerOnMove.Color),
                new XElement("ForcedAttackBox", ((game.ForcedAttackBox is null) ? null : Box.GetXML(game.ForcedAttackBox))),
                Board.GetXML(game.Board),
                Game.GetXML(game.BoardHistory)
                );

                return gameXML;
            }
            catch
            {
                throw new ParseException("Chyba při ukládání stavu hry do XML!");
            }
        }

        public static Game GetGameFromXML(XElement xml)
        {
            try
            {
                int diff = Int32.Parse(xml.Element("Difficulty").Value);
                Players whitePlayer = xml.Element("WhitePlayer").Value == "Human" ? Players.Human : Players.AI;
                Players blackPlayer = xml.Element("BlackPlayer").Value == "Human" ? Players.Human : Players.AI;
                Game game = new Game(diff, whitePlayer, blackPlayer);

                game.Round = int.Parse(xml.Element("Round").Value);
                game.Board = Board.GetBoardFromXML(xml.Element("Board"));

                game.RoundWithoutDead = Int32.Parse(xml.Element("RoundWithoutDead").Value);
                game.IsActive = false; // xml.Element("IsActive").Value == "true";
                game.IsOver = xml.Element("IsOver").Value == "true";
                game.PlayerOnMove = xml.Element("PlayerOnMove").Value == "White" ? game.WhitePlayer : game.BlackPlayer;
                game.ForcedAttackBox = xml.Element("ForcedAttackBox").HasElements ? Box.GetBoxFromXML(xml.Element("ForcedAttackBox")) : null;
                game.BoardHistory = new Dictionary<int, Board>();
                int round = 0;
                foreach (XElement xElement in xml.Element("BoardHistory").Elements("Board"))
                {
                    game.BoardHistory.Add(round, Board.GetBoardFromXML(xElement));
                    round++;
                }
                bool validationError = (game.RoundWithoutDead > game.Round) || (game.RoundWithoutDead > 30) || (game.WhitePlayer.Color != PieceColor.White) || (game.WhitePlayer.Color != PieceColor.Black);
                if (validationError)
                    throw new ParseException("XML soubor je poškozený a neprošel validací!");

                return game;
            }
            catch (Exception e)
            {
                throw new ParseException("Chyba při načítání stavu hry z XML!" + " " + e.Message);
            }
        }

        public static XElement GetXML(Dictionary<int, Board> boardHistory)
        {
            XElement boardHistoryXML = new XElement("BoardHistory");

            foreach (var board in boardHistory)
            {
                boardHistoryXML.Add(
                    Board.GetXML(board.Value));
            }

            return boardHistoryXML;
        }
    }
}