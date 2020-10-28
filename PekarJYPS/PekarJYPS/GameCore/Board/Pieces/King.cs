using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PekarJYPS
{
    public class King : Piece
    {
        public King(Coordinates coordinates, PieceColor pieceColor) : base(coordinates, pieceColor)
        {
            Value = 3;

            //Přiřazení ikony k figurce
            Icon = new Image();
            string uriString = "";
            switch (this.Color)
            {
                case PieceColor.White:
                    uriString = "Images/king_white.bmp";
                    break;
                case PieceColor.Black:
                    uriString = "Images/king_black.bmp";
                    break;
            }
            Icon.Source = new BitmapImage(new Uri(uriString, UriKind.Relative));
            Icon.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.Fant); 
        }

        public override object Clone()
        {
            return new King(this.Coordinates, this.Color);
        }

        public override Move[] GetPossibleAttacks(Board board)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
