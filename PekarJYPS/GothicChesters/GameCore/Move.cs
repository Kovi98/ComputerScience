using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters
{
    public class Move : IEquatable<Move>
    {
        public Box CurrentPosition { get; private set; }
        public Box NextPosition { get; private set; }
        public int Rank { get; set; }
        public Box[] AttackedPosition { get; private set; }
        public Piece[] DroppedPiece { get; set; }
        public bool HasEvolved { get; set; }
        public Move(Box currentPosition, Box nextPosition, Box[] attackedPosition = null)
        {
            CurrentPosition = currentPosition;
            NextPosition = nextPosition;
            AttackedPosition = attackedPosition;
            Rank = 0;

            if (!(AttackedPosition is null))
            {
                foreach (Box box in AttackedPosition)
                {
                    this.Rank += box.Piece.Value;
                }
            }
            //Snaha táhnout figurky vpřed - aby se necyklili namístě, když ještě nevidí protivníka
            if ((currentPosition.Piece.Color == PieceColor.White && currentPosition.Coordinates.Row < nextPosition.Coordinates.Row) || (currentPosition.Piece.Color == PieceColor.Black && currentPosition.Coordinates.Row > nextPosition.Coordinates.Row))
                Rank++;
            HasEvolved = false;
        }

        public bool Equals(Move other)
        {
            return this.CurrentPosition.Equals(other.CurrentPosition) && this.NextPosition.Equals(other.NextPosition);
        }
    }
}
