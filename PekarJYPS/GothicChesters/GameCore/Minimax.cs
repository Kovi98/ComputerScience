using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters.GameCore
{
    public class Minimax
    {
        //proměnná pro modifikátor ranku
        private static double _rankModifier = 1.3;
        private static Random _randomRankModifier = new Random();
        private static double SearchTree (Board board, Player player, Player enemy, int depth, double rank, Player playerOnMove = null)
        {
            if (depth == 0)
                return rank;
            
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit rank
                return rank;

            double? bestRank = null;
            Random random = new Random();

            foreach (Box box in ownedBoxes) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {
                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box)); //Generování všech možných tahů k hráčovým políčkům
                if (movesBox.Count == 0)
                    continue;
                foreach (Move move in movesBox) //Procházení všech možných tahů
                {
                    //Board clonedBoard = (Board)board.Clone();
                    if (player == playerOnMove)
                        move.Modifier = _rankModifier;
                    rank += move.Rank;
                    board.DoMove(move);
                    rank += Minimax.SearchTree(board, enemy, player, depth - 1, -rank, playerOnMove);
                    if (bestRank is null || bestRank < rank || (bestRank == rank && random.Next(1, 2) == 1))
                        bestRank = rank;
                    board.UndoMove(move);
                }
            }
            return bestRank.HasValue ? bestRank.Value : rank;
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
            Random random = new Random();
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit null
                return null;
            Move bestMove = null;

            foreach (Box box in ownedBoxes) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {

                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box)); //Generování všech možných tahů k hráčovým políčkům
                foreach (Move move in movesBox) //Procházení všech možných tahů
                {
                    move.Modifier = _rankModifier;
                    board.DoMove(move);
                    move.Rank = -Minimax.SearchTree(board, enemy, player, depth, -move.Rank, player);
                    if (bestMove is null || bestMove.Rank < move.Rank || (bestMove.Rank == move.Rank && random.Next(1,3) == 1))
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
