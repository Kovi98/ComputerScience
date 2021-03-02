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
        public double Rank { get; set; }
        public Box[] AttackedPosition { get; private set; }
        public Piece[] DroppedPiece { get; set; }
        public bool HasEvolved { get; set; }
        public double Modifier { 
            get => _modifier;
            set
            {
                _modifier = value;
                RecalculateRank();
            }
        }
        private double _modifier = 1;
        public Move(Box currentPosition, Box nextPosition, Box[] attackedPosition = null)
        {

            CurrentPosition = currentPosition;
            NextPosition = nextPosition;
            AttackedPosition = attackedPosition;
            Rank = 0;

            RecalculateRank();
            HasEvolved = false;
        }

        public bool Equals(Move other)
        {
            return this.CurrentPosition.Equals(other.CurrentPosition) && this.NextPosition.Equals(other.NextPosition);
        }

        public void RecalculateRank()
        {
            Rank = 0;
            //Snaha táhnout figurky vpřed - aby se necyklili namístě, když ještě nevidí protivníka
            if ((CurrentPosition.Piece.Color == PieceColor.White && CurrentPosition.Coordinates.Row < NextPosition.Coordinates.Row) || (CurrentPosition.Piece.Color == PieceColor.Black && CurrentPosition.Coordinates.Row > NextPosition.Coordinates.Row))
                Rank += 1 * Modifier;
            //Výpočet
            if (!(AttackedPosition is null))
            {
                foreach (Box box in AttackedPosition)
                {
                    this.Rank += box.Piece.Value * Modifier;
                }
            }
        }
    }
}
