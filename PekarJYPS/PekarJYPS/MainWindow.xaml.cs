using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GothicChesters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameUI GameUI { get; private set; }
        public Game Game => GameUI.Game;
        public bool IsGameCreated => !(GameUI is null);
        private List<Box> toMark = new List<Box>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void NewGame()
        {
            int diff = cmbDiff.SelectedIndex + 1;
            Game newGame;

            switch (cmbPlayer.SelectedIndex)
            {
                case 0:
                    newGame = new Game(diff, Players.Human, Players.Human);
                    break;
                case 1:
                    newGame = new Game(diff, Players.Human, Players.AI);
                    break;
                case 2:
                    newGame = new Game(diff, Players.AI, Players.AI);
                    break;
                default:
                    throw new InvalidOperationException("cmbPlayer nemá žádnou hodnotu");
            }
            GameUI = new GameUI(this, newGame);
        }

        private void grdBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameUI.IsGameActive)
            {
                if ((GameUI.Game.WhitePlayer is Human && GameUI.Game.PlayerOnMove.Equals(GameUI.Game.WhitePlayer)) || (GameUI.Game.BlackPlayer is Human && GameUI.Game.PlayerOnMove.Equals(GameUI.Game.BlackPlayer)))
                {
                    if (e.Source is Button)
                    {
                        Button button = (Button)e.Source;

                        //Označení figurky + možných skoků
                        if (GameUI.Game.MarkedBox is null)
                        {
                            if (!(GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Piece is null) && GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Piece.Color.Equals(GameUI.Game.PlayerOnMove.Color))
                            {
                                if (GameUI.Game.ForcedAttackBox is null)
                                {
                                    GameUI.Game.MarkedBox = GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column];
                                }
                                else
                                {
                                    GameUI.Game.MarkedBox = GameUI.Game.ForcedAttackBox;
                                }

                                toMark.Add(GameUI.Game.MarkedBox);
                                toMark.AddRange(GameUI.Game.MovesMarkedBox.Select(x => x.NextPosition));
                                foreach (Box box in toMark)
                                {
                                    box.Mark();
                                }
                            }
                            return;
                        }

                        var result = from m in GameUI.Game.MovesMarkedBox
                                     where (m.CurrentPosition.Equals(GameUI.Game.MarkedBox)) && (m.NextPosition.Coordinates.Row == ((Coordinates)button.Tag).Row) && (m.NextPosition.Coordinates.Column == ((Coordinates)button.Tag).Column)
                                     select m;

                        if (!(GameUI.Game.MarkedBox is null))
                        {
                            if (result.Count() == 1)
                            {
                                foreach (Box box in toMark)
                                {
                                    box.Unmark();
                                }

                                Move move = result.First();

                                toMark.RemoveRange(0, toMark.Count);
                                GameUI.Game.MarkedBox = null;
                                GameUI.DoMove(move);
                            }
                            else if (result.Count() == 0)
                            {
                                foreach (Box box in toMark)
                                {
                                    box.Unmark();
                                }
                                toMark.RemoveRange(0, toMark.Count);
                                GameUI.Game.MarkedBox = null;
                            }
                            else
                            {
                                throw new InvalidOperationException("výsledkem dotazu result musí být vždy jen 1 pohyb!");
                            }
                        }
                    }
                }
            }
        }

        private void cbOn_Click(object sender, RoutedEventArgs e)
        {
            if (cbOn.IsEnabled)
            {
                if (GameUI is null)
                {
                    NewGame();
                }
                else
                {
                    switch (cmbPlayer.SelectedIndex)
                    {
                        case 0:
                            if (!(GameUI.Game.WhitePlayer is Human))
                                GameUI.Game.WhitePlayer = new Human(PieceColor.White);
                            if (!(GameUI.Game.BlackPlayer is Human))
                                GameUI.Game.BlackPlayer = new Human(PieceColor.Black);
                            break;
                        case 1:
                            if (!(GameUI.Game.WhitePlayer is Human))
                                GameUI.Game.WhitePlayer = new Human(PieceColor.White);
                            if (!(GameUI.Game.BlackPlayer is AI))
                                GameUI.Game.BlackPlayer = new AI(PieceColor.Black);
                            break;
                        case 2:
                            if (!(GameUI.Game.WhitePlayer is AI))
                                GameUI.Game.WhitePlayer = new AI(PieceColor.White);
                            if (!(GameUI.Game.BlackPlayer is AI))
                                GameUI.Game.BlackPlayer = new AI(PieceColor.Black);
                            break;
                        default:
                            throw new InvalidOperationException("cmbPlayer nemá žádnou hodnotu");
                    }
                }

                    GameUI.IsGameActive = cbOn.IsChecked.HasValue ? cbOn.IsChecked.Value : false;
            }
        }

        private void lsBxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!GameUI.Game.IsActive)
            {
                ListBox listBox = (ListBox)e.Source;
                GameUI.DrawBoard(GameUI.Game.BoardHistory[(int)listBox.SelectedItem], true);
            }
        }
        }
    }