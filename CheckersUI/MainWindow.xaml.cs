using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using ColorChecker = CheckersCore.Core.Player.Color;

namespace CheckersUI
{
    public partial class MainWindow : Window
    {
        public Dictionary<Image, Position> Positions = new Dictionary<Image, Position>();

        public Image[,] Images = new Image[8, 8];

        private Checker _selectedChecker;

        public MainWindow()
        {
            InitializeComponent();

            _initMainBoard();
            
            _initHighLighterBoard();

            _drawFromBoard();
        }

        private void _changeImage(Checker checker, Position newPosition, Position oldPosition)
        {
            Images[oldPosition.X, oldPosition.Y].Source = _getImage(new Checker(ColorChecker.None));
            Images[newPosition.X, newPosition.Y].Source = _getImage(checker);
        }

        private void _selectChecker(Checker? checker)
        {
            #pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            _selectedChecker = checker;
        }

        private void _initHighLighterBoard() 
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Image greenImage = new Image()
                    {
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
                    
                    Images[x, y] = noneImage;

                    BoardGrid.Children.Add(noneImage);
                }
            }
        }

        private void _drawFromBoard() 
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Checker checker = Board.Instance[x, y];
                    checker.SetPosition(x, y);

                    Images[x, y].Source = _getImage(checker);
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

            List<int> avaibleMovesIndexes = Move.GetAvaibleMovesIndex(checker, checker.Color == ColorChecker.Black, HighLightGrid.Rows);

            if (!Move.MoveChecker(checker, selectedPosition, out Checker brokenChecker))
                return;

            _changeImage(checker, selectedPosition, oldPosition);

            if (brokenChecker != null)
                _changeImage(brokenChecker, brokenChecker.GetPosition(), brokenChecker.GetPosition());

            HighLighter.HideHighLight(avaibleMovesIndexes);

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

            HighLighter.ShowHighLight(checker);

            _selectChecker(checker);
        }

        private ImageSource _getImage(Checker checker)
        {
            if (checker.Color == ColorChecker.None) 
                return new BitmapImage(new Uri("", UriKind.Relative));
            else
                return checker.Color == ColorChecker.White ? new BitmapImage(new Uri("Assets/WhiteChecker.png", UriKind.Relative)) 
                                                           : new BitmapImage(new Uri("Assets/BlackChecker.png", UriKind.Relative));
        }
    }
}