using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PekarJYPS
{
    public class Board
    {
        public Box[,] Boxes { get; private set; }
        public int WhiteDead { get; private set; }
        public int BlackDead { get; private set; }
        public Game Game { get; private set; }
        public Board(Game game)
        {
            Game = game;
            WhiteDead = 0;
            BlackDead = 0;

            Boxes = new Box[8, 8];
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    Coordinates coor = new Coordinates(i, j);
                    Boxes[i, j] = new Box(coor);
                    if (i == 0 || i == 1)
                        Boxes[i, j].Piece = new Man(coor, PieceColor.White);
                    if (i == 6 || i == 7)
                        Boxes[i, j].Piece = new Man(coor, PieceColor.Black);
                }
            }
        }

        /// <summary>
        /// Provedení kroku (pohyb/útok)
        /// </summary>
        /// <param name="move"></param>
        public void DoMove(Move move)
        {
            move.NextPosition.Piece = move.CurrentPosition.Piece;
            move.CurrentPosition.Piece = null;

            if (move.AttackedPosition is object)
                DropPiece(move.AttackedPosition);

            //Evoluce kámen -> dáma pokud je kámen na posledním řádku své barvy
            if ((move.NextPosition.Piece is Man) && ((move.NextPosition.Piece.Color.Equals(PieceColor.White) && move.NextPosition.Coordinates.Row == 7) || (move.NextPosition.Piece.Color.Equals(PieceColor.Black) && move.NextPosition.Coordinates.Row == 0)))
            {
                Man man = (Man)move.NextPosition.Piece;
                move.NextPosition.Piece = man.Evolve();
            }
        }

        /// <summary>
        /// Vyhození figurky z hracího pole
        /// </summary>
        /// <param name="box"></param>
        public void DropPiece(Box box)
        {
            switch (box.Piece.Color)
            {
                case PieceColor.White:
                    WhiteDead++;
                    break;
                case PieceColor.Black:
                    BlackDead++;
                    break;
            }

            box.Piece = null;
        }
    }
}