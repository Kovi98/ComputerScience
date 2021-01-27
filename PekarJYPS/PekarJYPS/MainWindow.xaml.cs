using PekarJYPS;
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
using System.Xml;
using System.Xml.Linq;

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
        public void NewGame(Game game)
        {
            if (game.WhitePlayer is Human && game.BlackPlayer is Human)
                cmbPlayer.SelectedIndex = 0;
            if (game.WhitePlayer is Human && game.BlackPlayer is AI)
                cmbPlayer.SelectedIndex = 1;
            if (game.WhitePlayer is AI && game.BlackPlayer is AI)
                cmbPlayer.SelectedIndex = 2;
            cmbDiff.SelectedIndex = game.Difficulty - 1;
            GameUI = new GameUI(this, game);
        }

        private void grdBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameUI.IsHelpOn)
            {
                GameUI.Refresh();
                GameUI.IsHelpOn = false;
                return;
            }
            if (GameUI.IsGameActive)
            {
                if ((GameUI.Game.WhitePlayer is Human && GameUI.Game.PlayerOnMove.Equals(GameUI.Game.WhitePlayer)) || (GameUI.Game.BlackPlayer is Human && GameUI.Game.PlayerOnMove.Equals(GameUI.Game.BlackPlayer)))
                {
                    if (e.Source is Button)
                    {
                        Button button = (Button)e.Source;

                        //Označení figurky + možných skoků
                        if (GameUI.MarkedBox is null)
                        {
                            if (!(GameUI.BoxesUI[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Box.Piece is null) && GameUI.BoxesUI[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Box.Piece.Color.Equals(GameUI.Game.PlayerOnMove.Color))
                            {
                                if (GameUI.Game.ForcedAttackBox is null)
                                {
                                    GameUI.MarkedBox = GameUI.BoxesUI[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column];
                                }
                                else
                                {
                                    GameUI.MarkedBox = GameUI.BoxesUI[GameUI.Game.ForcedAttackBox.Coordinates.Row, GameUI.Game.ForcedAttackBox.Coordinates.Column];
                                }

                                toMark.Add(GameUI.MarkedBox.Box);
                                toMark.AddRange(GameUI.MovesMarkedBox.Select(x => x.NextPosition));
                                foreach (Box box in toMark)
                                {
                                    GameUI.BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Mark();
                                }
                            }
                            return;
                        }

                        var result = from m in GameUI.MovesMarkedBox
                                     where (m.CurrentPosition.Equals(GameUI.MarkedBox.Box)) && (m.NextPosition.Coordinates.Row == ((Coordinates)button.Tag).Row) && (m.NextPosition.Coordinates.Column == ((Coordinates)button.Tag).Column)
                                     select m;

                        if (!(GameUI.MarkedBox is null))
                        {
                            if (result.Count() == 1)
                            {
                                foreach (Box box in toMark)
                                {
                                    GameUI.BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Unmark();
                                }

                                Move move = result.First();

                                toMark.RemoveRange(0, toMark.Count);
                                GameUI.MarkedBox = null;
                                GameUI.DoMove(move);
                            }
                            else if (result.Count() == 0)
                            {
                                foreach (Box box in toMark)
                                {
                                    GameUI.BoxesUI[box.Coordinates.Row, box.Coordinates.Column].Unmark();
                                }
                                toMark.RemoveRange(0, toMark.Count);
                                GameUI.MarkedBox = null;
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

        private async void cbOn_Click(object sender, RoutedEventArgs e)
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
                    GameUI.Game.Difficulty = cmbDiff.SelectedIndex + 1;
                }
                GameUI.Refresh();
                if (GameUI.IsViewMode)
                {
                    GameUI.RedrawBoard(GameUI.Game.Board);
                    GameUI.IsViewMode = false;
                }
                GameUI.IsGameActive = cbOn.IsChecked.HasValue ? (bool)cbOn.IsChecked : false;
                if (GameUI.Game.IsActive && !GameUI.Game.IsOver && GameUI.Game.PlayerOnMove is AI)
                {
                    AI player = (AI)GameUI.Game.PlayerOnMove;
                    await player.PlayAsync(GameUI.Game);
                }
            }
        }

        private void lsBxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(GameUI is null) && !GameUI.Game.IsActive && !GameUI.IsLoadMode)
            {
                ListBox listBox = (ListBox)e.Source;
                if (!(listBox.SelectedItem is null) && listBox.Items.Count > 1)
                {
                    int number = (int)listBox.SelectedItem;
                    Board tempBoard = (Board)GameUI.Game.BoardHistory[number].Clone();
                    GameUI.DrawBoard(tempBoard, true);
                    GameUI.IsViewMode = true;
                }
            }
        }

        private async void menuTah_Click(object sender, RoutedEventArgs e)
        {
            Player player = GameUI.Game.PlayerOnMove;
            Player enemy = GameUI.Game.PlayerOnMove.Color is PieceColor.White ? GameUI.Game.BlackPlayer : GameUI.Game.WhitePlayer;
            int depth = 0;
            switch (GameUI.Game.Difficulty)
            {
                case 1:
                    depth = 0;
                    break;
                case 2:
                    depth = 1;
                    break;
                case 3:
                    depth = 2;
                    break;
            }
            Move bestMove = await GameCore.Minimax.SearchAsync(GameUI.Game.Board, player, enemy, depth);
            GameUI.BoxesUI[bestMove.CurrentPosition.Coordinates.Row, bestMove.CurrentPosition.Coordinates.Column].Mark();
            GameUI.BoxesUI[bestMove.NextPosition.Coordinates.Row, bestMove.NextPosition.Coordinates.Column].Mark();
            GameUI.IsHelpOn = true;
        }

        private void menuZpet_Click(object sender, RoutedEventArgs e)
        {
            GameUI.Game.Undo();
            GameUI.Game.PlayerOnMove = GameUI.Game.WhitePlayer;
            GameUI.RedrawBoard(Game.Board);
            GameUI.Refresh();
        }

        private void menuVratitZpet_Click(object sender, RoutedEventArgs e)
        {
            GameUI.Game.Redo();
            GameUI.Game.PlayerOnMove = GameUI.Game.WhitePlayer;
            GameUI.RedrawBoard(Game.Board);
            GameUI.Refresh();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowRules window = new WindowRules();
            window.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            WindowHelp window = new WindowHelp();
            window.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "XML Files|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Game.GetXML(Game).Save(saveFileDialog.FileName);
                    MessageBox.Show("Hra byla uložena do souboru " + saveFileDialog.FileName);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "XML Files|*.xml";
                if (openFileDialog.ShowDialog() == true)
                {
                    XElement xml = XElement.Load(openFileDialog.FileName);
                    NewGame(Game.GetGameFromXML(xml));
                    GameUI.IsGameActive = false;
                    GameUI.IsLoadMode = true;
                    MessageBox.Show("Hra byla nahrána ze souboru " + openFileDialog.FileName);
                    cbOn.IsChecked = false;
                    GameUI.Refresh();
                    if (GameUI.Game.IsOver)
                    {
                        cbOn.IsEnabled = false;
                        GameUI.IsGameActive = false;
                        cmbDiff.IsEnabled = false;
                        cmbPlayer.IsEnabled = false;
                    }
                    GameUI.IsLoadMode = false;
                }

            }
            catch (Exception error)
             {
                 MessageBox.Show("Nepodařilo se načíst soubor! Chyba: " + error.Message);
             }
        }
    }
    }