using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekarJYPS
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
                moves.AddRange(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box));
            }


            Random random = new Random();
            //Vrátí random pohyb - nutnost dodělat MINIMAX
            if (moves.Count > 0)
                return moves[random.Next(moves.Count)];
            return null;
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
