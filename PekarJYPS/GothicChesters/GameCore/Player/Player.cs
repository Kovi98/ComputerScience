using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters
{
    public abstract class Player
    {
        public PieceColor Color { get; protected set; }

        public Player(PieceColor color)
        {
            Color = color;
        }

        public override string ToString()
        {
            return Color.ToString();
        }

        public Box[] GetBoxesWithOwnedPieces(Board board)
        {
            var boxes = from Box box in board.Boxes
                        where !(box.Piece is null) && box.Piece.Color.Equals(this.Color)
                        select box;
            return boxes.ToArray();
        }
    }

    public enum Players
    {
        Human, AI
    }
}
