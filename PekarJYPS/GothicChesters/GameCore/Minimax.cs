using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters.GameCore
{
    public class Minimax
    {
        /// <summary>
        /// IMplementace algoritmu Minimax do statické metody
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <param name="depth"></param>
        /// <returns>Nejlepší tah na desce, pokud žádný není, tak null</returns>
        public static Move GetBestMove (Board board, Player player, Player enemy, int depth, bool isEnemy = false)
        {
            Board newBoard;
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit null
                return null;
            Move bestMove = null;

            for (int i = 0; i < ownedBoxes.Count; i++) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {
                
                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(ownedBoxes[i]).Length > 0 ? board.GetPossibleAttacks(ownedBoxes[i]) : board.GetPossibleMoves(ownedBoxes[i])); //Generování všech možných tahů k hráčovým políčkům
                for (int j = 0; j < movesBox.Count; j++) //Procházení všech možných tahů
                {
                    if (depth != 0) //Generování dalších větví stromu algoritmu
                    {
                        newBoard = (Board)board.Clone(); //Naklonování herní desky
                        newBoard.DoMove(movesBox[j]); //Provedení tahu na naklonovanou herní desku
                        Move tempMove = Minimax.GetBestMove(newBoard, isEnemy ? enemy : player, isEnemy ? player : enemy, depth--, !isEnemy);
                        if (bestMove is null || bestMove.Rank < tempMove.Rank)
                        {
                            movesBox[i].Rank += tempMove.Rank;
                            bestMove = movesBox[i];
                        }
                    }
                    else //Pokud je hloubka 0 - konec procházení
                    {
                        if (bestMove is null || bestMove.Rank < movesBox[i].Rank)
                            bestMove = movesBox[i];
                    }
                }
            }

            return bestMove;
        }
    }
}
