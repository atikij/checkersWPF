using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorChecker = CheckersCore.Core.Player.Color;

namespace CheckersUI
{
    public partial class MainWindow : Window
    {
        public static GameState GameState = new GameState(12);

        public Dictionary<Image, Position> Positions = new Dictionary<Image, Position>();

        private Image[,] _images = new Image[8, 8];

        private Checker _selectedChecker;

        public MainWindow()
        {
            InitializeComponent();
            
            Application.Current.MainWindow = this;

            GameState.StartGame();
        }

        public void ResetGame()
        {
            HighLightGrid.Children.Clear();
            RedLightGrid.Children.Clear();
            BoardGrid.Children.Clear();
            Positions.Clear();

            _initMainBoard();

            _initHighLightBoard();

            _drawMainBoard();
        }

        private void _changeImage(Checker checker, Position newPosition, Position oldPosition)
        {
            if (newPosition == null || oldPosition == null)
                return;

            _images[oldPosition.X, oldPosition.Y].Source = _getImage(new Checker(ColorChecker.None));
            _images[newPosition.X, newPosition.Y].Source = _getImage(checker);
        }

        private void _selectChecker(Checker? checker)
        {
            #pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            _selectedChecker = checker;
        }

        private void _initHighLightBoard() 
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Image redImage = new Image() {
                        Source = new BitmapImage(new Uri("Assets/HighLightRed.png", UriKind.Relative)),
                        Opacity = 0
                    };

                    RedLightGrid.Children.Add(redImage);

                    Image greenImage = new Image() {
                        Source = new BitmapImage(new Uri("Assets/HighLightGreen.png", UriKind.Relative)),
                        Opacity = 0
                    };

                    greenImage.MouseLeftButtonDown += _onMouseLeftClick_SelectChecker;
                    greenImage.MouseLeftButtonDown += _onMouseLeftClick_Move;

                    Positions.Add(greenImage, new Position(x, y));

                    HighLightGrid.Children.Add(greenImage);
                }
            }
        }

        private void _initMainBoard() 
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Image noneImage = new Image();
                    
                    _images[x, y] = noneImage;

                    BoardGrid.Children.Add(noneImage);
                }
            }
        }

        private void _drawMainBoard() 
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Checker checker = Board.Instance[x, y];
                    checker.SetPosition(x, y);

                    _images[x, y].Source = _getImage(checker);
                }
            }
        }

        private void _onMouseLeftClick_Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_selectedChecker == null)
                return;

            Checker checker = _selectedChecker;

            Position oldPosition = checker.GetPosition();
            Position selectedPosition = Positions[(Image)sender];

            if (!Move.CanMoveChecker(checker, selectedPosition, out Checker brokenChecker))
                return;

            HighLighter.HideHighLight(checker);
            
            if (brokenChecker != null)
            {
                GameState.Delete(brokenChecker);

                brokenChecker.DeleteFromGame();

                _changeImage(brokenChecker, brokenChecker.GetPosition(), brokenChecker.GetPosition());
            }

            Move.MoveChecker(checker, selectedPosition, out bool switchTurn);

            _changeImage(checker, selectedPosition, oldPosition);

            if (switchTurn || brokenChecker == null)
                GameState.SwitchTurn();

            if (GameState.GameIsEnded(out _, out string reason))
            {
                MessageBox.Show(reason, "Game is ended", MessageBoxButton.OK, MessageBoxImage.Information);

                new MenuWindow().Show();
                this.Close();

                return;
            }

            _selectChecker(null);
        }

        private void _onMouseLeftClick_SelectChecker(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_selectedChecker != null)
                HighLighter.HideHighLight(_selectedChecker);

            Position selectedPosition = Positions[(Image)sender];
            Checker checker = Board.Instance[selectedPosition.X, selectedPosition.Y];

            if (checker == null || checker.Color == ColorChecker.None)
                return;

            if (GameState.Turn != checker.Color)
                return;

            HighLighter.ShowHighLight(checker);

            _selectChecker(checker);
        }

        private ImageSource _getImage(Checker checker)
        {
            if (checker == null || checker.Color == ColorChecker.None)
                return new BitmapImage(new Uri("", UriKind.Relative));

            if (checker.IsQueen)
                return checker.Color == ColorChecker.White ? new BitmapImage(new Uri("Assets/WhiteCheckerQueen.png", UriKind.Relative)) : new BitmapImage(new Uri("Assets/BlackCheckerQueen.png", UriKind.Relative));

            return checker.Color == ColorChecker.White ? new BitmapImage(new Uri("Assets/WhiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Assets/BlackChecker.png", UriKind.Relative));
        }
    }
}