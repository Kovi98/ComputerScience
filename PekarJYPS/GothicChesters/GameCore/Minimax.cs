﻿using System;
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
        public static Move Search (Board board, Player player, Player enemy, int depth, bool isEnemy = false)
        {
            Board newBoard;
            List<Box> ownedBoxes = new List<Box>(player.GetBoxesWithOwnedPieces(board)); //Generování všech políček hrací desky, které obsahují hráčovu figurku
            if (ownedBoxes.Count == 0) //Pokud hráč žádné políčko obsazené nemá, vrátit null
                return null;
            Move bestMove = null;

            foreach (Box box in ownedBoxes) //Procházení všech políček hrací desky, které obsahují hráčovu figurku
            {
                
                List<Move> movesBox = new List<Move>(board.GetPossibleAttacks(box).Length > 0 ? board.GetPossibleAttacks(box) : board.GetPossibleMoves(box)); //Generování všech možných tahů k hráčovým políčkům
                foreach (Move move in movesBox) //Procházení všech možných tahů
                {
                    if (depth != 0) //Generování dalších větví stromu algoritmu
                    {
                        newBoard = (Board)board.Clone(); //Naklonování herní desky
                        newBoard.DoMove(move); //Provedení tahu na naklonovanou herní desku
                        Move tempMove = Minimax.Search(newBoard, player, enemy, depth-1); //isEnemy ? Minimax.GetBestMove(newBoard, player, enemy, depth--) : Minimax.GetBestMove(newBoard, enemy, player, depth, true);
                        if (bestMove is null || bestMove.Rank < tempMove.Rank)
                        {
                            //BLBOSTmove.Rank += tempMove.Rank;
                            bestMove = move;
                        }
                    }
                    else //Pokud je hloubka 0 - konec procházení
                    {
                        if (bestMove is null || bestMove.Rank < move.Rank)
                            bestMove = move;
                    }
                }
            }

            return bestMove;
        }

        public static Task<Move> SearchAsync(Board board, Player player, Player enemy, int depth, bool isEnemy = false)
        {
            return Task.Run(() =>
            {
                return Search(board, player, enemy, depth, isEnemy);
            });
        }
    }
}
