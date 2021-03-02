using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GothicChesters
{
    public class King : Piece
    {
        public King(Coordinates coordinates, PieceColor pieceColor) : base(coordinates, pieceColor)
        {
            Value = 15;

            //Přiřazení cesty ikony k figurce
            switch (this.Color)
            {
                case PieceColor.White:
                    IconPath = "Images/king_white.bmp";
                    break;
                case PieceColor.Black:
                    IconPath = "Images/king_black.bmp";
                    break;
            }
        }

        public override object Clone()
        {
            return new King(this.Coordinates, this.Color);
        }
        public Man Devolve()
        {
            return new Man(Coordinates, Color);
        }
        public override Move[] GetPossibleAttacks(Board board)
        {
            //Generický List možných pohybů
            List<Move> moves = new List<Move>();
            int i = 0;
            int j = 0;

            List<Box> attackedBoxes = new List<Box>();

            //Větev, pokud je figurka bílá
            if (Color.Equals(PieceColor.White))
            {
                //Pohyb nahoru
                if (Coordinates.Row < 6)
                {
                    for (i = Coordinates.Row + 1; i <= 7; i++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black) && i < 7)
                            attackedBoxes.Add(board.Boxes[i, Coordinates.Column]);

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White) && i < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb dolů
                if (Coordinates.Row > 1)
                {
                    for (i = Coordinates.Row - 1; i >= 0; i--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black) && i > 0)
                            attackedBoxes.Add(board.Boxes[i, Coordinates.Column]);

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White) && i > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb doprava
                if (Coordinates.Column < 6)
                {
                    for (i = Coordinates.Column + 1; i <= 7; i++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i], attackedBoxes.ToArray()));

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black) && i < 7)
                            attackedBoxes.Add(board.Boxes[Coordinates.Row, i]);

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White) && i < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb doleva
                if (Coordinates.Column > 1)
                {
                    for (i = Coordinates.Column - 1; i >= 0; i--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i], attackedBoxes.ToArray()));

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black) && i > 0)
                            attackedBoxes.Add(board.Boxes[Coordinates.Row, i]);

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White) && i > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vpravo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column + 1; i <= 7 && j <= 7; i++, j++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i < 7 && j < 7)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i < 7 && j < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vpravo dolu
                if (Coordinates.Row > 0 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column + 1; i >= 0 && j <= 7; i--, j++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i > 0 && j < 7)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i > 0 && j < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vlevo dolu
                if (Coordinates.Row > 0 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i > 0 && j > 0)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i > 0 && j > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vlevo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column - 1; i <= 7 && j >= 0; i++, j--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i < 7 && j > 0)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i < 7 && j > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }
            }

            //Větev, pokud je figurka černá
            else if (Color.Equals(PieceColor.Black))
            {
                //Pohyb nahoru
                if (Coordinates.Row < 6)
                {
                    for (i = Coordinates.Row + 1; i <= 7; i++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White) && i < 7)
                            attackedBoxes.Add(board.Boxes[i, Coordinates.Column]);

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black) && i < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb dolů
                if (Coordinates.Row > 1)
                {
                    for (i = Coordinates.Row - 1; i >= 0; i--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, Coordinates.Column].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, Coordinates.Column], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.White) && i > 0)
                            attackedBoxes.Add(board.Boxes[i, Coordinates.Column]);

                        if (!(board.Boxes[i, Coordinates.Column].Piece is null) && board.Boxes[i, Coordinates.Column].Piece.Color.Equals(PieceColor.Black) && i > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb doprava
                if (Coordinates.Column < 6)
                {
                    for (i = Coordinates.Column + 1; i <= 7; i++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i], attackedBoxes.ToArray()));

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White) && i < 7)
                            attackedBoxes.Add(board.Boxes[Coordinates.Row, i]);

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black) && i < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb doleva
                if (Coordinates.Column > 1)
                {
                    for (i = Coordinates.Column - 1; i >= 0; i--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[Coordinates.Row, i].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, i], attackedBoxes.ToArray()));

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.White) && i > 0)
                            attackedBoxes.Add(board.Boxes[Coordinates.Row, i]);

                        if (!(board.Boxes[Coordinates.Row, i].Piece is null) && board.Boxes[Coordinates.Row, i].Piece.Color.Equals(PieceColor.Black) && i > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vpravo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column + 1; i <= 7 && j <= 7; i++, j++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i < 7 && j < 7)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i < 7 && j < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vpravo dolu
                if (Coordinates.Row > 0 && Coordinates.Column < 7)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column + 1; i >= 0 && j <= 7; i--, j++)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i > 0 && j < 7)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i > 0 && j < 7)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vlevo dolu
                if (Coordinates.Row > 0 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row - 1, j = Coordinates.Column - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i > 0 && j > 0)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i > 0 && j > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }

                //Pohyb diagonalne vlevo nahoru
                if (Coordinates.Row < 7 && Coordinates.Column > 0)
                {
                    for (i = Coordinates.Row + 1, j = Coordinates.Column - 1; i <= 7 && j >= 0; i++, j--)
                    {
                        if (attackedBoxes.Count() > 0 && board.Boxes[i, j].Piece is null)
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[i, j], attackedBoxes.ToArray()));

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.White) && i < 7 && j > 0)
                            attackedBoxes.Add(board.Boxes[i, j]);

                        if (!(board.Boxes[i, j].Piece is null) && board.Boxes[i, j].Piece.Color.Equals(PieceColor.Black) && i < 7 && j > 0)
                            break;
                    }
                    attackedBoxes.RemoveRange(0, attackedBoxes.Count());
                }
            }
            return moves.ToArray();
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
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null))
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
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null))
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
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null))
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
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null))
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
                        if (!(board.Boxes[i, Coordinates.Column].Piece is null))
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
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null))
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
                        if (!(board.Boxes[Coordinates.Row, i].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
                        if (!(board.Boxes[i, j].Piece is null))
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
