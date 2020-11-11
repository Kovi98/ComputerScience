using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PekarJYPS
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

        /// <summary>
        /// Výpočet nejlepšího tahu
        /// </summary>
        /// <param name="board">Hrací deska</param>
        /// <param name="forcedAttackBox">Vynucený útok</param>
        /// <returns>Nejlepší tah, pokud není tah tak null</returns>
        public Move GetBestMoveOnBoard(Board board, Box forcedAttackBox = null)
        {
            List<Box> boxes = new List<Box>();
            List<Move> moves = new List<Move>(); 
            if (forcedAttackBox is null)
            {
                boxes.AddRange(GetBoxesWithOwnedPieces(board));
            }
            else
            {
                boxes.Add(forcedAttackBox);
            }
            if (boxes.Count() == 0)
                return null;

            foreach (Box box in boxes)
            {
                moves.Add(GetBestMoveForBox(board, box));
            }

            Random random = new Random();
            //Vrátí random pohyb - nutnost dodělat MINIMAX
            return moves[random.Next(moves.Count())];
        }

        public Box[] GetBoxesWithOwnedPieces(Board board)
        {
            var boxes = from Box box in board.Boxes
                        where !(box.Piece is null) && box.Piece.Color.Equals(this.Color)
                        select box;
            return boxes.ToArray();
        }

        public Move GetBestMoveForBox(Board board, Box box)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleAttacks(box));

            Random random = new Random();
            //Vrátí random pohyb - nutnost dodělat MINIMAX
            return moves[random.Next(moves.Count())];
        }
    }

    public enum PieceColor
    {
        White,
        Black
    }
}
