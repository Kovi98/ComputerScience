﻿using System;
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
    class King : Piece
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
                    uriString = "Images/king_white.png";
                    break;
                case PieceColor.Black:
                    uriString = "Images/king_black.png";
                    break;
            }
            Icon.Source = new BitmapImage(new Uri(uriString, UriKind.Relative));
            Icon.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.Fant); 
        }
    }
}
