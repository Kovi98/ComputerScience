using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekarJYPS
{
    public class Move
    {
        public Box CurrentPosition { get; private set; }
        public Box NextPosition { get; private set; }
        public int Value { get; private set; }
        public Box[] AttackedPosition { get; private set; }
        public Move(Box currentPosition, Box nextPosition, Box[] attackedPosition = null)
        {
            CurrentPosition = currentPosition;
            NextPosition = nextPosition;
            AttackedPosition = attackedPosition;
        }


    }
}
