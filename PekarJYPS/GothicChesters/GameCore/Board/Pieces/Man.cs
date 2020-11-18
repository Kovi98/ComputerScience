using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PekarJYPS
{
    public class Man : Piece
    {
        public Man(Coordinates coordinates, PieceColor pieceColor) : base(coordinates, pieceColor)
        {
            Value = 3;

            //Přiřazení ikony k figurce
            Icon = new Image();
            string uriString = "";
            switch (this.Color)
            {
                case PieceColor.White:
                    uriString = "Images/white_man.bmp";
                    break;
                case PieceColor.Black:
                    uriString = "Images/black_man.bmp";
                    break;
            }
            Icon.Source = new BitmapImage(new Uri(uriString, UriKind.Relative));
            Icon.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.Fant);
        }

        public override object Clone()
        {
            return new Man(this.Coordinates, this.Color);
        }

        /// <summary>
        /// Evoluce kamene v dámu
        /// </summary>
        /// <returns>Vyvinutá dáma</returns>
        public King Evolve()
        {
            return new King(Coordinates, Color);
        }

        /// <summary>
        /// Vrací Array všech možných útoků - přeskoků (Move)
        /// </summary>
        /// <returns>Move Array</returns>
        public override Move[] GetPossibleAttacks(Board board)
        {
            //Generický List možných útoků (přeskoků)
            List<Move> moves = new List<Move>();

            //Man se nesmí nacházet na poslední pozici (řádku) - dle barev
            if ((Color.Equals(PieceColor.White) && Coordinates.Row < 7) || ((Color.Equals(PieceColor.Black) && Coordinates.Row > 0)))
            {
                //Větev, pokud je figurka bílá - figurky dole
                if (Color.Equals(PieceColor.White))
                {
                    //Útok nahoru
                    if (Coordinates.Row < 6 && !(board.Boxes[Coordinates.Row + 1, Coordinates.Column].Piece is null) && board.Boxes[Coordinates.Row + 1, Coordinates.Column].Piece.Color.Equals(PieceColor.Black) && board.Boxes[Coordinates.Row + 2, Coordinates.Column].Piece is null)
                    {
                        moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 2, Coordinates.Column], new Box[] { board.Boxes[Coordinates.Row + 1, Coordinates.Column] }));
                    }

                    //Man vlevo nebo uprostřed (pohyb doprava, diagonalne doprava)
                    if (Coordinates.Column != 7)
                    {
                        //Diagonalne doprava
                        if (Coordinates.Column < 6 && Coordinates.Row < 6 && !(board.Boxes[Coordinates.Row + 1, Coordinates.Column + 1].Piece is null) && board.Boxes[Coordinates.Row + 1, Coordinates.Column + 1].Piece.Color.Equals(PieceColor.Black) && board.Boxes[Coordinates.Row + 2, Coordinates.Column + 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 2, Coordinates.Column + 2], new Box[] { board.Boxes[Coordinates.Row + 1, Coordinates.Column + 1] }));
                        }

                        //Doprava
                        if (Coordinates.Column < 6 && !(board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece is null) && board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece.Color.Equals(PieceColor.Black) && board.Boxes[Coordinates.Row, Coordinates.Column + 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column + 2], new Box[] { board.Boxes[Coordinates.Row, Coordinates.Column + 1] }));
                        }

                    }

                    //Man vpravo nebo uprostřed (pohyb doleva, diagonalne doleva)
                    if (Coordinates.Column != 0)
                    {
                        //Diagonalne doleva
                        if (Coordinates.Column > 1 && Coordinates.Row < 6 && !(board.Boxes[Coordinates.Row + 1, Coordinates.Column - 1].Piece is null) && board.Boxes[Coordinates.Row + 1, Coordinates.Column - 1].Piece.Color.Equals(PieceColor.Black) && board.Boxes[Coordinates.Row + 2, Coordinates.Column - 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 2, Coordinates.Column - 2], new Box[] { board.Boxes[Coordinates.Row + 1, Coordinates.Column - 1] }));
                        }

                        //Doleva
                        if (Coordinates.Column > 1 && !(board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece is null) && board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece.Color.Equals(PieceColor.Black) && board.Boxes[Coordinates.Row, Coordinates.Column - 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column - 2], new Box[] { board.Boxes[Coordinates.Row, Coordinates.Column - 1] }));
                        }
                    }
                }

                //Větev, pokud je figurka černá - figurky nahoře
                else if (Color.Equals(PieceColor.Black))
                {
                    //Útok dolů
                    if (Coordinates.Row > 1 && !(board.Boxes[Coordinates.Row - 1, Coordinates.Column].Piece is null) && board.Boxes[Coordinates.Row - 1, Coordinates.Column].Piece.Color.Equals(PieceColor.White) && board.Boxes[Coordinates.Row - 2, Coordinates.Column].Piece is null)
                    {
                        moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 2, Coordinates.Column], new Box[] { board.Boxes[Coordinates.Row - 1, Coordinates.Column] }));
                    }

                    //Man vlevo nebo uprostřed (pohyb doprava, diagonalne doprava)
                    if (Coordinates.Column != 7)
                    {
                        //Diagonalne doprava
                        if (Coordinates.Column < 6 && Coordinates.Row > 1 && !(board.Boxes[Coordinates.Row - 1, Coordinates.Column + 1].Piece is null) && board.Boxes[Coordinates.Row - 1, Coordinates.Column + 1].Piece.Color.Equals(PieceColor.White) && board.Boxes[Coordinates.Row - 2, Coordinates.Column + 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 2, Coordinates.Column + 2], new Box[] { board.Boxes[Coordinates.Row - 1, Coordinates.Column + 1] }));
                        }

                        //Doprava
                        if (Coordinates.Column < 6 && !(board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece is null) && board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece.Color.Equals(PieceColor.White) && board.Boxes[Coordinates.Row, Coordinates.Column + 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column + 2], new Box[] { board.Boxes[Coordinates.Row, Coordinates.Column + 1] }));
                        }

                    }

                    //Man vpravo nebo uprostřed (pohyb doleva, diagonalne doleva)
                    if (Coordinates.Column != 0)
                    {
                        //Diagonalne doleva
                        if (Coordinates.Column > 1 && Coordinates.Row > 1 && !(board.Boxes[Coordinates.Row - 1, Coordinates.Column - 1].Piece is null) && board.Boxes[Coordinates.Row - 1, Coordinates.Column - 1].Piece.Color.Equals(PieceColor.White) && board.Boxes[Coordinates.Row - 2, Coordinates.Column - 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 2, Coordinates.Column - 2], new Box[] { board.Boxes[Coordinates.Row - 1, Coordinates.Column - 1] }));
                        }

                        //Doleva
                        if (Coordinates.Column > 1 && !(board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece is null) && board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece.Color.Equals(PieceColor.White) && board.Boxes[Coordinates.Row, Coordinates.Column - 2].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column - 2], new Box[] { board.Boxes[Coordinates.Row, Coordinates.Column - 1] }));
                        }
                    }
                }
            }
            return moves.ToArray();
        }

        /// <summary>
        /// Vrací Array všech možných pohybů (Move)
        /// </summary>
        /// <returns>Move Array</returns>
        public override Move[] GetPossibleMoves(Board board)
        {
            //Generický List možných pohybů
            List<Move> moves = new List<Move>();

            //Man se nesmí nacházet na poslední pozici (řádku) - dle barev
            if ((Color.Equals(PieceColor.White) && Coordinates.Row < 8) || ((Color.Equals(PieceColor.Black) && Coordinates.Row > 0)))
            {
                //Větev, pokud je figurka bílá - figurky dole
                if (Color.Equals(PieceColor.White))
                {
                    //Pohyb nahoru - možný vždy, když je políčko volné
                    if (board.Boxes[Coordinates.Row + 1, Coordinates.Column].Piece is null)
                    {
                        moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 1, Coordinates.Column]));
                    }

                    //Man vlevo nebo uprostřed (pohyb doprava, diagonalne doprava)
                    if (Coordinates.Column != 7)
                    {
                        //Diagonalne doprava
                        if (board.Boxes[Coordinates.Row + 1, Coordinates.Column + 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 1, Coordinates.Column + 1]));
                        }

                        //Doprava
                        if (board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column + 1]));
                        }

                    }

                    //Man vpravo nebo uprostřed (pohyb doleva, diagonalne doleva)
                    if (Coordinates.Column != 0)
                    {
                        //Diagonalne doleva
                        if (board.Boxes[Coordinates.Row + 1, Coordinates.Column - 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row + 1, Coordinates.Column - 1]));
                        }

                        //Doleva
                        if (board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column - 1]));
                        }
                    }
                }

                //Větev, pokud je figurka černá - figurky nahoře
                else if (Color.Equals(PieceColor.Black))
                {
                    //Pohyb dolů - možný vždy, když je políčko volné
                    if (board.Boxes[Coordinates.Row - 1, Coordinates.Column].Piece is null)
                    {
                        moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 1, Coordinates.Column]));
                    }

                    //Man vlevo nebo uprostřed (pohyb doprava, diagonalne doprava)
                    if (Coordinates.Column != 7)
                    {
                        //Diagonalne doprava
                        if (board.Boxes[Coordinates.Row - 1, Coordinates.Column + 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 1, Coordinates.Column + 1]));
                        }

                        //Doprava
                        if (board.Boxes[Coordinates.Row, Coordinates.Column + 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column + 1]));
                        }

                    }

                    //Man vpravo nebo uprostřed (pohyb doleva, diagonalne doleva)
                    if (Coordinates.Column != 0)
                    {
                        //Diagonalne doleva
                        if (board.Boxes[Coordinates.Row - 1, Coordinates.Column - 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row - 1, Coordinates.Column - 1]));
                        }

                        //Doleva
                        if (board.Boxes[Coordinates.Row, Coordinates.Column - 1].Piece is null)
                        {
                            moves.Add(new Move(board.Boxes[Coordinates.Row, Coordinates.Column], board.Boxes[Coordinates.Row, Coordinates.Column - 1]));
                        }
                    }
                }
            }
            return moves.ToArray();
        }
    }
}
