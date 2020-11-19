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
            game.DoMove(GetBestMoveOnBoard(game.Board));
        }
    }
}
