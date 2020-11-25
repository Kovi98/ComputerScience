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
            Player rival = game.WhitePlayer.Equals(this) ? game.BlackPlayer : game.WhitePlayer;
            int depth = 1;
            switch (game.Difficulty)
            {
                case 1:
                    depth = 1;
                    break;
                case 2: 
                    depth = 5;
                    break;
                case 3:
                    depth = 10;
                    break;
            }
            game.DoMove(GetBestMoveOnBoard(game.Board, rival, depth));
        }
    }
}
