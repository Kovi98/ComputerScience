using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    Coordinates coor = new Coordinates(i, j);
                    Boxes[i, j] = new Box(coor);
                    if (i == 0 || i == 1)
                        Boxes[i, j].Piece = new Man(Game, coor, PieceColor.White);
                    if (i == 6 || i == 7)
                        Boxes[i, j].Piece = new Man(Game, coor, PieceColor.Black);
                }
            }
        }

        /// <summary>
        /// Provedení kroku (pohyb/útok)
        /// </summary>
        /// <param name="move"></param>
        public void DoMove(Move move)
        {

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