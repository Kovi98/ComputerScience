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
    public class Box
    {
        public Coordinates Coordinates { get; private set; }
        public Piece piece;
        public Piece Piece
        {
            get => piece;
            set
            {
                piece = value;
                piece.Coordinates = Coordinates;
            }
        }
        public Button Button { get; private set; }
        public Grid Grid { get; set; }

        public Box(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Grid = new Grid();
            Grid.IsHitTestVisible = false;
            if (Coordinates.Column % 2 != Coordinates.Row % 2)
            {
                Button.Background = Brushes.Black;
            }
            else
            {
                Button.Background = Brushes.White;
            }
            Button.Tag = Coordinates;
        }

        public Box()
        {
        }

        private void Redraw()
        {
            if (Grid.Children.Count > 0)
                Grid.Children.RemoveAt(0);
            if (Piece != null)
            {
                Grid.Children.Add(Piece.Icon);
            }
            Button.Content = Grid;
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
    }
}
