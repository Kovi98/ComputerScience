using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GothicChesters
{
    public abstract class Piece : ICloneable
    {
        public Image Icon { get; protected set; }
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
    }

    public enum PieceColor
    {
        White,
        Black
    }
}
