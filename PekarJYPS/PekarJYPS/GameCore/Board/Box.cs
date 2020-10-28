using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PekarJYPS
{
    public class Box : ICloneable
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
        public Button Button { get; set; }
        public Grid Grid { get; set; }

        public Box(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }


        public void Mark()
        {
            Grid.Children.Add(MarkedBox(Brushes.Green));
            Button.Content = Grid;
        }

        private Rectangle MarkedBox(Brush color)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = color;
            rectangle.Opacity = 0.3;
            rectangle.Stretch = Stretch.UniformToFill;
            rectangle.HorizontalAlignment = HorizontalAlignment.Stretch;
            rectangle.IsHitTestVisible = false;
            return rectangle;
        }

        public void Unmark()
        {
            Grid.Children.RemoveRange(0, Grid.Children.Count);
            if (Piece != null)
                Grid.Children.Add(Piece.Icon);
        }

        public object Clone()
        {
            Box clonedBox = new Box(this.Coordinates);
            clonedBox.Button = this.Button;
            clonedBox.Grid = this.Grid;

            if (this.Piece is null)
            {
                clonedBox.Piece = null;
            }
            else
            {
                clonedBox.Piece = (Piece)this.Piece.Clone();
            }

            return clonedBox;
        }
    }
}
