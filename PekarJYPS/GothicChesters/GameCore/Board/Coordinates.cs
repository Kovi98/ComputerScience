using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters
{
    public class Coordinates
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Coordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
