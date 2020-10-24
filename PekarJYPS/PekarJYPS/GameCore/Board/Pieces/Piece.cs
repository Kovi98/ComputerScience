using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PekarJYPS
{
    public abstract class Piece
    {
        public Image Icon { get; protected set; }
        public PieceColor Color { get; private set; }
        public Coordinates Coordinates { get; set; }
        public int Value { get; protected set; }
        public Game Game { get; private set; }
        public Piece(Game game, Coordinates coordinates, PieceColor color)
        {
            Game = game;
            Coordinates = coordinates;
            Color = color;
        }

        public abstract Move[] GetPossibleMoves();

        public abstract Move[] GetPossibleAttacks();
    }

    public enum PieceColor
    {
        White, Black
    }
}
