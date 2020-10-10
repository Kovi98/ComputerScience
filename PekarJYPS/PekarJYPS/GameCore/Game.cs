using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekarJYPS
{
    public class Game
    {
        private int difficulty;
        public int Difficulty
        {
            get => difficulty;
            set
            {
                if (value>=1 && value<=3)
                {
                    difficulty = value;
                }
                else
                {

                    throw new ArgumentOutOfRangeException("Difficulty", "Difficulty must be 1, 2 or 3");
                }
            }
        }
    }
}
