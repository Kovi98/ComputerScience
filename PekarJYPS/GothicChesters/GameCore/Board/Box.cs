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
                Coordinates.GetXML(box.Coordinates),
                (box.Piece is null) ? null : Piece.GetXML(box.Piece));
            return boardXML;
        }

        public static Box GetBoxFromXML(XElement xml)
        {
            int row = Int32.Parse(xml.Element("Coordinates").Element("Row").Value);
            int column = Int32.Parse(xml.Element("Coordinates").Element("Column").Value);

            Coordinates coor = new Coordinates(row, column);
            Box box = new Box(coor);

            if (!(xml.Element("Piece") is null) && xml.Element("Piece").HasElements)
            {
                PieceColor pieceColor = xml.Element("Piece").Element("Color").Value == "White" ? PieceColor.White : PieceColor.Black;
                if (xml.Element("Piece").Element("Kind").Value == "Man")
                {
                    box.Piece = new Man(coor, pieceColor);
                }
                else
                {
                    box.Piece = new King(coor, pieceColor);
                }
            }

            return box;
        }
    }
}
