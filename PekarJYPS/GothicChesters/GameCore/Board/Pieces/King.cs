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
            Move[] moves = new Move[0];
            return moves;
        }

        public override Move[] GetPossibleMoves(Board board)
        {
            //Generický List možných pohybů
            List<Move> moves = new List<Move>();
            int i = 0;
            int j = 0;

            //Větev, pokud je figurka bílá
            if (Color.Equals(PieceColor.White))
            {
                //Pohyb nahoru
                if (Coordinates.Row < 7)
                {
                    for (i = Coordinates.Row + 1; i <= 7; i++)
                    {
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column]));
                    }
                }

                //Pohyb dolů
                if (Coordinates.Row > 0)
                {
                    for (i = Coordinates.Row - 1; i >= 0; i--)
                    {
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column]));
                    }
                }

                //Pohyb doprava
                if (Coordinates.Column < 7)
                {
                    for (i = Coordinates.Column + 1; i <= 7; i++)
                    {
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i]));
                    }
                }

                //Pohyb doleva
                if (Coordinates.Column > 0)
                {
                    for (i = Coordinates.Column - 1; i >= 0; i--)
                    {
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i]));
                    }
                }

                //Pohyb diagonalne vpravo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column + 1; i <= 7 && j <= 7; i++, j++)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vpravo dolu
                if (Coordinates.Row > 0 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column + 1; i >= 0 && j <= 7; i--, j++)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vlevo dolu
                if (Coordinates.Row > 0 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vlevo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column - 1; i <= 7 && j >= 0; i++, j--)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }
            }

            //Větev, pokud je figurka černá
            else if (Color.Equals(PieceColor.Black))
            {
                //Pohyb nahoru
                if (Coordinates.Row < 7)
                {
                    for (i = Coordinates.Row + 1; i <= 7; i++)
                    {
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column]));
                    }
                }

                //Pohyb dolů
                if (Coordinates.Row > 0)
                {
                    for (i = Coordinates.Row - 1; i >= 0; i--)
                    {
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column]));
                    }
                }

                //Pohyb doprava
                if (Coordinates.Column < 7)
                {
                    for (i = Coordinates.Column + 1; i <= 7; i++)
                    {
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i]));
                    }
                }

                //Pohyb doleva
                if (Coordinates.Column > 0)
                {
                    for (i = Coordinates.Column - 1; i >= 0; i--)
                    {
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i]));
                    }
                }

                //Pohyb diagonalne vpravo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column + 1; i <= 7 && j <= 7; i++, j++)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vpravo dolu
                if (Coordinates.Row > 0 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column + 1; i >= 0 && j <= 7; i--, j++)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vlevo dolu
                if (Coordinates.Row > 0 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }

                //Pohyb diagonalne vlevo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column - 1; i <= 7 && j >= 0; i++, j--)
                    {
                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White))
                            break;

                        if (board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j]));
                    }
                }
            }
            return moves.ToArray();
        }
    }
}
