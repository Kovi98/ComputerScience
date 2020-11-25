using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicChesters
{
    public class Coordinates : IEquatable<Coordinates>
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Coordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(Coordinates other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }
    }
}
