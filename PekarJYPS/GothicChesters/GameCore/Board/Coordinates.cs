using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public static XElement GetXML(Coordinates coordinates)
        {
            XElement boardXML = new XElement("Coordinates", 
                new XElement("Row", coordinates.Row),
                new XElement("Column", coordinates.Column));
            return boardXML;
        }
    }
}
