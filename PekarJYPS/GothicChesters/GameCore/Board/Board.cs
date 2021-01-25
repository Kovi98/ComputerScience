﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace GothicChesters
{
    public class Board : ICloneable
    {
        public Box[,] Boxes { get; private set; }
        public int WhiteDead { get; private set; }
        public int BlackDead { get; private set; }
        public Board()
        {
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
                        Boxes[i, j].Piece = new King(coor, PieceColor.White);
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
            List<Piece> droppedPieces = new List<Piece>();
            Boxes[move.NextPosition.Coordinates.Row, move.NextPosition.Coordinates.Column].Piece = Boxes[move.CurrentPosition.Coordinates.Row, move.CurrentPosition.Coordinates.Column].Piece;
            Boxes[move.CurrentPosition.Coordinates.Row, move.CurrentPosition.Coordinates.Column].Piece = null;

            if (!(move.AttackedPosition is null) && move.AttackedPosition.Count() > 0)
            {
                foreach (Box box in move.AttackedPosition)
                {
                    droppedPieces.Add(Boxes[box.Coordinates.Row, box.Coordinates.Column].Piece);
                    DropPiece(Boxes[box.Coordinates.Row, box.Coordinates.Column]);
                }
            }
            move.DroppedPiece = droppedPieces.ToArray();

            //Evoluce kámen -> dáma pokud je kámen na posledním řádku své barvy
            if ((move.NextPosition.Piece is Man) && ((move.NextPosition.Piece.Color.Equals(PieceColor.White) && move.NextPosition.Coordinates.Row == 7) || (move.NextPosition.Piece.Color.Equals(PieceColor.Black) && move.NextPosition.Coordinates.Row == 0)))
            {
                Man man = (Man)move.NextPosition.Piece;
                move.NextPosition.Piece = man.Evolve();
                move.HasEvolved = true;
            }
        }

        /// <summary>
        /// Undo kroku
        /// </summary>
        /// <param name="move"></param>
        public void UndoMove(Move move)
        {
            //Evoluce kámen -> dáma pokud je kámen na posledním řádku své barvy
            if (move.HasEvolved)
            {
                King king = (King)move.NextPosition.Piece;
                move.NextPosition.Piece = king.Devolve();
                move.HasEvolved = false;
            }

            foreach (Piece piece in move.DroppedPiece)
            {
                Boxes[piece.Coordinates.Row, piece.Coordinates.Column].Piece = piece;
            }

            Boxes[move.CurrentPosition.Coordinates.Row, move.CurrentPosition.Coordinates.Column].Piece = Boxes[move.NextPosition.Coordinates.Row, move.NextPosition.Coordinates.Column].Piece;
            Boxes[move.NextPosition.Coordinates.Row, move.NextPosition.Coordinates.Column].Piece = null;
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

        public object Clone()
        {
            Board clonedBoard = new Board();

            clonedBoard.BlackDead = this.BlackDead;
            clonedBoard.WhiteDead = this.WhiteDead;

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    clonedBoard.Boxes[i,j] = (Box)this.Boxes[i,j].Clone();
                }
            }

            return clonedBoard;
        }

        public Move[] GetPossibleMoves(Box box)
        {
            if (!(box.Piece is null))
            {
                return box.Piece.GetPossibleMoves(this);
            }
            else
            {
                return new Move[0];
            }
        }

        public Move[] GetPossibleAttacks(Box box)
        {
            if (!(box.Piece is null))
            {
                return box.Piece.GetPossibleAttacks(this);
            }
            else
            {
                return new Move[0];
            }
        }

        public static XElement GetXML(Board board)
        {
            XElement boardXML = new XElement("Board",
                new XElement("WhiteDead", board.WhiteDead),
                new XElement("BlackDead", board.BlackDead));

            foreach (Box box in board.Boxes)
            {
                boardXML.Add(Box.GetXML(box));
            }

            return boardXML;
        }
    }
}