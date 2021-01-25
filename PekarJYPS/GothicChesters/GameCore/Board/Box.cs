using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GothicChesters
{
    public class Box : ICloneable, IEquatable<Box>
    {
        public Coordinates Coordinates { get; private set; }
        public Piece piece;
        public Piece Piece
        {
            get => piece;
            set
            {
                piece = value;
                if (!(piece is null))
                    piece.Coordinates = Coordinates;
            }
        }

        public Box(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }

        public object Clone()
        {
            Box clonedBox = new Box(this.Coordinates);

            clonedBox.Piece = this.Piece is null ? null : (Piece)this.Piece.Clone();

            return clonedBox;
        }

        public bool Equals(Box other)
        {
            return this.Coordinates.Equals(other.Coordinates);
        }

        public static XElement GetXML(Box box)
        {
            XElement boardXML = new XElement("Box",
                new XElement("Coordinates", Coordinates.GetXML(box.Coordinates)),
                new XElement("Piece", Piece.GetXML(box.Piece)));
            return boardXML;
        }
    }
}
