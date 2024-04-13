using System.IO;
using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ColorChecker = CheckersCore.Core.Player.Color;

namespace CheckersUI
{
    public partial class MainWindow : Window
    {
        public static GameState GameState = new GameState(12);

        public Dictionary<Image, Position> Positions = new Dictionary<Image, Position>();

        private Image[,] _images = new Image[8, 8];

        private Checker _selectedChecker;
        private DispatcherTimer _whitePlayerTimer;
        private DispatcherTimer _blackPlayerTimer;

        private TimeSpan _whitePlayerTimeElapsed;
        private TimeSpan _blackPlayerTimeElapsed;
        private string _username;
        private string _password;

        public MainWindow(string username,string password)
        {
            InitializeComponent();
            _username = username;
            _password = password;
            
            Application.Current.MainWindow = this;

            GameState.StartGame();
            
            // Инициализация таймера для белого игрока
            _whitePlayerTimer = new DispatcherTimer();
            _whitePlayerTimer.Interval = TimeSpan.FromSeconds(1);
            _whitePlayerTimer.Tick += _WhitePlayerTimer_Tick;
            _whitePlayerTimeElapsed = TimeSpan.Zero;

            // Инициализация таймера для черного игрока
            _blackPlayerTimer = new DispatcherTimer();
            _blackPlayerTimer.Interval = TimeSpan.FromSeconds(1);
            _blackPlayerTimer.Tick += _BlackPlayerTimer_Tick;
            _blackPlayerTimeElapsed = TimeSpan.Zero;

            _whitePlayerTimer.Start();
            CurrentPlayerTextBlock.Text = $"Ходит: Белый";
        }
        
        private void _WhitePlayerTimer_Tick(object sender, EventArgs e)
        {
            // Обновление времени для белого игрока
            _whitePlayerTimeElapsed = _whitePlayerTimeElapsed.Add(TimeSpan.FromSeconds(1));
            WhitePlayerTimerTextBlock.Text = _whitePlayerTimeElapsed.ToString(@"mm\:ss");
        }

        private void _BlackPlayerTimer_Tick(object sender, EventArgs e)
        {
            // Обновление времени для черного игрока
            _blackPlayerTimeElapsed = _blackPlayerTimeElapsed.Add(TimeSpan.FromSeconds(1));
            BlackPlayerTimerTextBlock.Text = _blackPlayerTimeElapsed.ToString(@"mm\:ss");
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

            // Обновление информации о текущем игроке
            CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";

            HighLighter.ShowHighLight(checker);

            _selectChecker(checker);
        }
        
        private void SaveWhitePlayerTime(string username, string password)
        {
            string filePath = "D:\\rider repos\\checkers\\CheckersUI\\Assets\\users1.txt";
            string dataToWrite = $"{username},{password},{_whitePlayerTimeElapsed.ToString(@"hh\:mm\:ss")}";

            if (File.Exists(filePath))
            {
                // Считываем существующие данные из файла
                string[] existingData = File.ReadAllLines(filePath);
                bool userFound = false;

                // Идем по каждой строке ищем пользователя
                for (int i = 0; i < existingData.Length; i++)
                {
                    string[] parts = existingData[i].Split(',');
                    if (parts.Length == 3 && parts[0] == username && parts[1] == password)
                    {
                        // Найден пользователь, сравниваем время
                        TimeSpan existingTime = TimeSpan.Parse(parts[2]);
                        TimeSpan newTime = _whitePlayerTimeElapsed;

                        // Если новое время меньше текущего, обновляем запись
                        if (newTime < existingTime)
                        {
                            existingData[i] = dataToWrite; // Обновляем запись с новым временем
                        }
                        userFound = true;
                        break;
                    }
                }

                // Если пользователь не найден, добавляем новую запись
                if (!userFound)
                {
                    List<string> newData = existingData.ToList();
                    newData.Add(dataToWrite);
                    existingData = newData.ToArray();
                }

                // Записываем обновленные данные в файл
                File.WriteAllLines(filePath, existingData);
            }
            else
            {
                // Если файл не существует, создаем новый и записываем данные
                File.WriteAllText(filePath, dataToWrite);
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

            if (GameState.GameIsEnded(out ColorChecker winner, out string reason))
            {
                if (winner == ColorChecker.White)
                {
                    // Сохраняем время таймера белого игрока под его логином
                    SaveWhitePlayerTime(_username, _password);
                }
                MessageBox.Show(reason, "Game is ended", MessageBoxButton.OK, MessageBoxImage.Information);
                new MenuWindow(_username,_password).Show();
                this.Close();
                return;
            }


            // Возобновление таймера следующего игрока
            _StartPlayerTimer(GameState.Turn);

            // Обновление информации о текущем игроке
            CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";

            _selectChecker(null);
        }
        
        private void _StartPlayerTimer(ColorChecker playerColor)
        {
            _StopPlayerTimer(playerColor == ColorChecker.White ? ColorChecker.Black : ColorChecker.White);
            if (playerColor == ColorChecker.White)
                _whitePlayerTimer.Start();
            else
                _blackPlayerTimer.Start();
        }
        
        private void _StopPlayerTimer(ColorChecker playerColor)
        {
            if (playerColor == ColorChecker.White)
                _whitePlayerTimer.Stop();
            else
                _blackPlayerTimer.Stop();
        }

        private ImageSource _getImage(Checker checker)
        {
            if (checker == null || checker.Color == ColorChecker.None)
                return new BitmapImage(new Uri("", UriKind.Relative));

            if (checker.IsQueen)
                return checker.Color == ColorChecker.White ? new BitmapImage(new Uri("Assets/WhiteCheckerQueen.png", UriKind.Relative)) : new BitmapImage(new Uri("Assets/BlackCheckerQueen.png", UriKind.Relative));

            return checker.Color == ColorChecker.White ? new BitmapImage(new Uri("Assets/WhiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Assets/BlackChecker.png", UriKind.Relative));
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow(_username,_password).Show();
            this.Close();
        }
    }
}