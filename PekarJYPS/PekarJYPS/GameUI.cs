using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PekarJYPS
{
    public class GameUI
    {
        public Game Game { get; private set; }
        public MainWindow GUI { get; private set; }

        public GameUI(MainWindow gui, Game game)
        {
            Game = game;
            GUI = gui;
            
            RedrawBoard(Game.Board);
        }
        public void DrawBoard(Board board)
        {
            GUI.txtWhoPlays.Text = "Na tahu je hráč s figurkami barvy DOPLNIT";

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (Game.Board.Boxes[i, j].Grid.Children.Count > 0)
                        Game.Board.Boxes[i, j].Grid.Children.RemoveAt(0);
                    if (!(Game.Board.Boxes[i, j].Piece is null))
                    {
                        Game.Board.Boxes[i, j].Grid.Children.Add(Game.Board.Boxes[i, j].Piece.Icon);
                    }
                    Game.Board.Boxes[i, j].Button.Content = Game.Board.Boxes[i, j].Grid;
                }
            }
        }

        public void DrawBoard(Board board, bool HasToDrawAllBoard)
        {
            if (HasToDrawAllBoard)
            {
                GUI.grdBoard.Children.RemoveRange(0, GUI.grdBoard.Children.Count);

                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        // Tlačítka pro ovládání hrací desky v GUI
                        Game.Board.Boxes[i, j].Button = new Button();
                        Game.Board.Boxes[i, j].Button.SetValue(Grid.ColumnProperty, j);
                        Game.Board.Boxes[i, j].Button.SetValue(Grid.RowProperty, i);
                        Game.Board.Boxes[i, j].Button.SetValue(Border.BorderThicknessProperty, new Thickness(0));
                        Game.Board.Boxes[i, j].Button.Focusable = false;
                        Game.Board.Boxes[i, j].Button.Padding = new Thickness(0);
                        if (i % 2 != j % 2)
                        {
                            Game.Board.Boxes[i, j].Button.Background = Brushes.Black;
                        }
                        else
                        {
                            Game.Board.Boxes[i, j].Button.Background = Brushes.White;
                        }
                        GUI.grdBoard.Children.Add(Game.Board.Boxes[i, j].Button);
                        Game.Board.Boxes[i, j].Button.Tag = Game.Board.Boxes[i, j].Coordinates;

                        Game.Board.Boxes[i, j].Grid = new Grid();
                        Game.Board.Boxes[i, j].Grid.IsHitTestVisible = false;
                    }
                }
            }
            RedrawPieces();
        }

        public void RedrawPieces()
        {
            DrawBoard(Game.Board);
        }

        public void RedrawBoard(Board board)
        {
            DrawBoard(board, true);
        }

        public void ChangeDifficulty(int diff)
        {
            try
            {
                Game.Difficulty = diff;
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show(e.Message);
            }
                
        }
    }
}
