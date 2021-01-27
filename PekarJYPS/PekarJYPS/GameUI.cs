using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GothicChesters
{
    public class GameUI
    {
        public Game Game { get; private set; }
        public MainWindow GUI { get; private set; }
        public BoxUI[,] BoxesUI { get; private set; }
        public BoxUI MarkedBox { get; set; }
        public bool IsHelpOn { get; set; }
        public bool IsViewMode { get; set; }
        public bool IsLoadMode { get; set; }
        public Move[] MovesMarkedBox
        {
            get
            {
                if (Game.ForcedAttackBox is null)
                {
                    if (!(MarkedBox is null))
                    {
                        Move[] attacks = Game.GetPossibleAttacks(Game.Board, MarkedBox.Box);
                        if (attacks.Length > 0)
                        {
                            return attacks;
                        }
                        Move[] moves = Game.GetPossibleMoves(Game.Board, MarkedBox.Box);
                        if (moves.Length > 0)
                        {
                            return moves;
                        }
                    }
                    return new Move[0];
                }
                else
                {
                    return Game.GetPossibleAttacks(Game.Board, Game.ForcedAttackBox);
                }
            }
        }
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

            BoxesUI = new BoxUI[8, 8];
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    BoxesUI[i, j] = new BoxUI();
                    BoxesUI[i, j].Box = Game.Board.Boxes[i, j];
                }
            }
            IsHelpOn = false;
            IsLoadMode = false;
            RedrawBoard(Game.Board);

            Game.OnAfterBoardChange += Refresh;
            Game.OnAfterGameOver += End;
        }
        public void DoMove(Move move)
        {
            if (Game.IsActive && !Game.IsOver)
            {
                if (!(move.AttackedPosition is null) && move.AttackedPosition.Count() > 0)
                {
                    foreach (Box box in move.AttackedPosition)
                    {
                        if (!(BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Grid is null))
                            BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Grid.Children.RemoveRange(0, BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Grid.Children.Count);
                    }
                }
                Game.DoMove(move);
            }
            else
            {
                throw new InvalidOperationException("Nelze udělat pohyb, když je hra ukončena, nebo pozastavena");
            }
        }
        public void DrawBoard(Board board)
        {
            GUI.txtWhoPlays.Text = "Na tahu je hráč s " + (Game.PlayerOnMove.Color.Equals(PieceColor.White) ? "bílými" : "černými") + " figurkami";
            GUI.txtWhiteOff.Text = Game.Board.WhiteDead.ToString();
            GUI.txtBlackOff.Text = Game.Board.BlackDead.ToString();
            GUI.txtRound.Text = Game.Round.ToString();
            
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (BoxesUI[i, j].Grid.Children.Count > 0)
                        BoxesUI[i, j].Grid.Children.RemoveRange(0, BoxesUI[i, j].Grid.Children.Count);
                }
            }
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (!(BoxesUI[i, j].Box.Piece is null))
                    {
                        BoxesUI[i, j].Grid.Children.Add(BoxesUI[i, j].Icon);
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
                        BoxesUI[i, j].Box = board.Boxes[i, j];
                        BoxesUI[i, j].Button = new Button();
                        BoxesUI[i, j].Button.SetValue(Grid.ColumnProperty, j);
                        BoxesUI[i, j].Button.SetValue(Grid.RowProperty, 7-i);
                        BoxesUI[i, j].Button.SetValue(Border.BorderThicknessProperty, new Thickness(0));
                        BoxesUI[i, j].Button.Focusable = false;
                        BoxesUI[i, j].Button.Padding = new Thickness(0);
                        if (i % 2 != j % 2)
                        {
                            BoxesUI[i, j].Button.Background = Brushes.Black;
                        }
                        else
                        {
                            BoxesUI[i, j].Button.Background = Brushes.White;
                        }
                        GUI.grdBoard.Children.Add(BoxesUI[i, j].Button);
                        BoxesUI[i, j].Button.Tag = board.Boxes[i, j].Coordinates;

                        BoxesUI[i, j].Grid = new Grid();
                        BoxesUI[i, j].Grid.IsHitTestVisible = false;
                        BoxesUI[i, j].Button.Content = BoxesUI[i, j].Grid;
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
        public void Refresh()
        {
            RedrawPieces();
            if (!Game.IsOver)
            {
                IsGameActive = GUI.cbOn.IsChecked.HasValue ? GUI.cbOn.IsChecked.Value : false;

                GUI.menuTah.IsEnabled = Game.PlayerOnMove is Human;
                GUI.menuZpet.IsEnabled = Game.Round > 1 && Game.PlayerOnMove is Human ? true : false;
                GUI.menuVratitZpet.IsEnabled = Game.BoardHistory.Count > 0 && Game.BoardHistory.Keys.Max() >= Game.Round && Game.PlayerOnMove is Human ? true : false;
            }
            else
            {
                GUI.menuTah.IsEnabled = false;
                GUI.menuZpet.IsEnabled = false;
                GUI.menuVratitZpet.IsEnabled = false;
            }
            GUI.menuSave.IsEnabled = !(Game is null);


        }
        void End()
        {
            GUI.cbOn.IsChecked = false;
            GUI.cbOn.IsEnabled = false;
            IsGameActive = false;
            Game.BackupBoard();
            GUI.cmbDiff.IsEnabled = false;
            GUI.cmbPlayer.IsEnabled = false;
            Refresh();
            MessageBox.Show("Vyhrál hráč s barvou figurek: " + Game.Winner.ToString());
        }
    }

    public class BoxUI
    {
        public Image Icon
        {
            get
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(Box.Piece.IconPath, UriKind.Relative));
                icon.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.Fant);
                return icon;
            }
        }
        public Box Box { get; set; }
        public Grid Grid { get; set; }
        public Button Button { get; set; }

        public void Mark()
        {
            Grid.Children.Add(MarkedBox(Brushes.Green));
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
            if (!(Box.Piece is null))
                Grid.Children.Add(Icon);
        }
    }
}
