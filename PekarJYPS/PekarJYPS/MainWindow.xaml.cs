using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PekarJYPS
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

            GameUI = new GameUI(this, new Game(1, Players.Human, Players.AI));
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void NewGame()
        {
            //GameUI = new GameUI(this, new Game());
        }

        private void grdBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                Button button = (Button)e.Source;
                if (GameUI.Game.MarkedBox is null)
                {
                    if (!(GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Piece is null) && GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column].Piece.Color.Equals(GameUI.Game.PlayerOnMove.Color))
                    {
                        GameUI.Game.MarkedBox = GameUI.Game.Board.Boxes[((Coordinates)button.Tag).Row, ((Coordinates)button.Tag).Column];
                        toMark.Add(GameUI.Game.MarkedBox);
                        toMark.AddRange(GameUI.Game.MovesMarkedBox.Select(x => x.NextPosition));
                        foreach ( Box box in toMark)
                        {
                            box.Mark();
                        }
                    }
                }
                else
                {
                    foreach (Box box in toMark)
                    {
                        box.Unmark();
                    }
                    toMark.RemoveRange(0, toMark.Count);
                    GameUI.Game.MarkedBox = null;
                }
            }
            else
            {
                MessageBox.Show("Něco");
            }
        }

        private void cbOn_Click(object sender, RoutedEventArgs e)
        {
            if (cbOn.IsChecked == true)
            {
                cmbDiff.IsEnabled = false;
                cmbPlayer.IsEnabled = false;
                lsBxHistory.IsEnabled = false;
                GameUI.Game.IsActive = true;
            }
            else
            {
                cmbDiff.IsEnabled = true;
                cmbPlayer.IsEnabled = true;
                lsBxHistory.IsEnabled = true;
                GameUI.Game.IsActive = false;
            }
        }
    }
}
