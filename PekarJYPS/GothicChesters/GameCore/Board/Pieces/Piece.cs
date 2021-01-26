using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GothicChesters
{
    public abstract class Piece : ICloneable
    {
        public string IconPath { get; protected set; }
        public PieceColor Color { get; private set; }
        public Coordinates Coordinates { get; set; }
        public int Value { get; protected set; }
        public Piece(Coordinates coordinates, PieceColor color)
        {
            Coordinates = coordinates;
            Color = color;
        }

        public abstract Move[] GetPossibleMoves(Board board);

        public abstract Move[] GetPossibleAttacks(Board board);

        public abstract object Clone();

        public static XElement GetXML(Piece piece)
        {
            string pieceKind = piece is Man ? "Man" : (piece is King ? "King" : "NULL");
            XElement boardXML = new XElement("Piece",
                new XElement("Color", piece.Color),
                new XElement("Kind", pieceKind));
            return boardXML;
        }
    }

    public enum PieceColor
    {
        White,
        Black
    }
}
