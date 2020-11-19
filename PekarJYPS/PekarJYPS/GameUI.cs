using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GothicChesters
{
    public class GameUI
    {
        public Game Game { get; private set; }
        public MainWindow GUI { get; private set; }
        private bool _isGameActive;
        public bool IsGameActive
        {
            get => _isGameActive;
            set
            {
                if (value)
                {
                    GUI.cmbDiff.IsEnabled = false;
                    GUI.cmbPlayer.IsEnabled = false;
                    GUI.lsBxHistory.IsEnabled = false;
                    Game.IsActive = true;
                }
                else
                {
                    GUI.cmbDiff.IsEnabled = true;
                    GUI.cmbPlayer.IsEnabled = true;
                    GUI.lsBxHistory.IsEnabled = true;
                    Game.IsActive = false;
                }
                _isGameActive = value;
            }
        }
        public GameUI(MainWindow gui, Game game)
        {
            Game = game;
            GUI = gui;
            
            RedrawBoard(Game.Board);

            Game.Board.OnAfterBoardChange += Refresh;
            Game.OnAfterGameOver += End;
        }
        public void DoMove(Move move)
        {
            if (Game.IsActive && !Game.IsOver)
            {
                Game.DoMove(move);
                RedrawPieces();
            }
            else
            {
                throw new InvalidOperationException("Nelze udělat pohyb, když je hra ukončena, nebo pozastavena");
            }

            if (Game.PlayerOnMove is AI && Game.IsActive && !Game.IsOver)
            {
                //Thread thread = new Thread(() => ((AI)PlayerOnMove).Play(this));
                //thread.Start();
                //((AI)Game.PlayerOnMove).Play(Game);
            }
        }
        public void DrawBoard(Board board)
        {
            GUI.txtWhoPlays.Text = "Na tahu je hráč s " + (Game.PlayerOnMove.Color.Equals(PieceColor.White) ? "bílými" : "černými") + " figurkami";

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (board.Boxes[i, j].Grid.Children.Count > 0)
                        board.Boxes[i, j].Grid.Children.RemoveRange(0, board.Boxes[i, j].Grid.Children.Count);
                }
            }
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (!(board.Boxes[i, j].Piece is null))
                    {
                        board.Boxes[i, j].Grid.Children.Add(board.Boxes[i, j].Piece.Icon);
                    }
                }
            }
            GUI.lsBxHistory.ItemsSource = Game.BoardHistory.Keys;
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
                        board.Boxes[i, j].Button = new Button();
                        board.Boxes[i, j].Button.SetValue(Grid.ColumnProperty, j);
                        board.Boxes[i, j].Button.SetValue(Grid.RowProperty, 7-i);
                        board.Boxes[i, j].Button.SetValue(Border.BorderThicknessProperty, new Thickness(0));
                        board.Boxes[i, j].Button.Focusable = false;
                        board.Boxes[i, j].Button.Padding = new Thickness(0);
                        if (i % 2 != j % 2)
                        {
                            board.Boxes[i, j].Button.Background = Brushes.Black;
                        }
                        else
                        {
                            board.Boxes[i, j].Button.Background = Brushes.White;
                        }
                        GUI.grdBoard.Children.Add(Game.Board.Boxes[i, j].Button);
                        board.Boxes[i, j].Button.Tag = Game.Board.Boxes[i, j].Coordinates;

                        board.Boxes[i, j].Grid = new Grid();
                        board.Boxes[i, j].Grid.IsHitTestVisible = false;
                        board.Boxes[i, j].Button.Content = board.Boxes[i, j].Grid;
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
        void Refresh()
        {
            RedrawPieces();
        }
        void End()
        {
            GUI.cbOn.IsChecked = false;
            GUI.cbOn.IsEnabled = false;
            GUI.lsBxHistory.IsEnabled = true;
        }
    }
}
