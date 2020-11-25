using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GothicChesters
{
    public class AI : Player
    {
        public AI(PieceColor color) : base(color)
        {
        }

        public void Play(Game game)
        {
            Player enemy = game.WhitePlayer.Equals(this) ? game.BlackPlayer : game.WhitePlayer;
            int depth = 0;
            switch (game.Difficulty)
            {
                case 1:
                    depth = 0;
                    break;
                case 2: 
                    depth = 1;
                    break;
                case 3:
                    depth = 2;
                    break;
            }
            Move bestMove = GameCore.Minimax.GetBestMove(game.Board, this, enemy, depth);
            game.DoMove(bestMove);
        }

        public async Task PlayAsync(Game game)
        {
            Player enemy = game.WhitePlayer.Equals(this) ? game.BlackPlayer : game.WhitePlayer;
            int depth = 0;
            switch (game.Difficulty)
            {
                case 1:
                    depth = 0;
                    break;
                case 2:
                    depth = 1;
                    break;
                case 3:
                    depth = 2;
                    break;
            }
            Move bestMove = await GameCore.Minimax.GetBestMoveAsync(game.Board, this, enemy, depth);
            game.DoMove(bestMove);
        }
    }
}
