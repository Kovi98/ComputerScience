using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters.GameCore
{
    public class Minimax
    {
        private static int SearchTree (Board board, Player player, Player enemy, int depth, int rank)
        {
            if (depth == 0)
                return rank;
            
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit rank
                return rank;

            foreach (Box box in ownedBoxes) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {
                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box)); //Generování všech možných tahů k hráčovým políčkům
                foreach (Move move in movesBox) //Procházení všech možných tahů
                {
                    //Board clonedBoard = (Board)board.Clone();
                    rank += move.Rank;
                    board.DoMove(move);
                    rank += Minimax.SearchTree(board, enemy, player, depth - 1, -move.Rank);
                    board.UndoMove(move);
                }
            }
            return rank;
        }
        /// <summary>
        /// IMplementace algoritmu Minimax do statické metody
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <param name="depth"></param>
        /// <returns>Nejlepší tah na desce, pokud žádný není, tak null</returns>
        public static Move Search (Board boardOriginal, Player player, Player enemy, int depth)
        {
            Board board = (Board)boardOriginal.Clone();
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit null
                return null;
            Move bestMove = null;

            foreach (Box box in ownedBoxes) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {

                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box)); //Generování všech možných tahů k hráčovým políčkům
                foreach (Move move in movesBox) //Procházení všech možných tahů
                {
                    //Board clonedBoard = (Board)board.Clone();
                    board.DoMove(move);
                    int rank = Minimax.SearchTree(board, player, enemy, depth, move.Rank);
                    move.Rank = rank;
                    if (bestMove is null || bestMove.Rank < move.Rank)
                        bestMove = move;
                    board.UndoMove(move);
                }
            }
            return bestMove;
        }

        public static Task<Move> SearchAsync(Board board, Player player, Player enemy, int depth)
        {
            return Task.Run(() =>
            {
                return Search(board, player, enemy, depth);
            });
        }
    }
}
