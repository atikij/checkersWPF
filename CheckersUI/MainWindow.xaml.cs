﻿using System.IO;
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
        private bool _typeGame;//true против человек, false против бота

        public MainWindow(string username,string password, bool typeGame)
        {
            InitializeComponent();
            _typeGame = typeGame;
            _username = username;
            _password = password;
            
            Application.Current.MainWindow = this;

            GameState.StartGame();
            
            _whitePlayerTimer = new DispatcherTimer();
            _whitePlayerTimer.Interval = TimeSpan.FromSeconds(1);
            _whitePlayerTimer.Tick += _WhitePlayerTimer_Tick;
            _whitePlayerTimeElapsed = TimeSpan.Zero;

            _blackPlayerTimer = new DispatcherTimer();
            _blackPlayerTimer.Interval = TimeSpan.FromSeconds(1);
            _blackPlayerTimer.Tick += _BlackPlayerTimer_Tick;
            _blackPlayerTimeElapsed = TimeSpan.Zero;

            _whitePlayerTimer.Start();
            CurrentPlayerTextBlock.Text = $"Ходит: Белый";
        }
        
        private void _WhitePlayerTimer_Tick(object sender, EventArgs e)
        {
            _whitePlayerTimeElapsed = _whitePlayerTimeElapsed.Add(TimeSpan.FromSeconds(1));
            WhitePlayerTimerTextBlock.Text = _whitePlayerTimeElapsed.ToString(@"mm\:ss");
        }

        private void _BlackPlayerTimer_Tick(object sender, EventArgs e)
        {
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
            #pragma warning disable CS8601 //Возможно, назначение-ссылка, допускающее значение NULL.
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

            CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";

            HighLighter.ShowHighLight(checker);

            _selectChecker(checker);
        }
        
        private void SaveWhitePlayerTime(string username, string password, bool isVictory)
        {
            string filePath = "D:\\rider repos\\checkers\\CheckersUI\\Assets\\users1.txt";
            string dataToWrite = $"{username},{password},{_whitePlayerTimeElapsed.ToString(@"hh\:mm\:ss")},{(isVictory ? "1,0" : "0,1")}"; // Если победа - увеличиваем количество побед, иначе увеличиваем количество поражений

            if (File.Exists(filePath))
            {
                string[] existingData = File.ReadAllLines(filePath);
                bool userFound = false;

                for (int i = 0; i < existingData.Length; i++)
                {
                    string[] parts = existingData[i].Split(',');
                    if (parts.Length >= 5 && parts[0] == username && parts[1] == password)
                    {
                        int victories = int.Parse(parts[3]) + (isVictory ? 1 : 0); // Увеличиваем количество побед
                        int losses = int.Parse(parts[4]) + (isVictory ? 0 : 1); // Увеличиваем количество поражений

                        existingData[i] = $"{username},{password},{_whitePlayerTimeElapsed.ToString(@"hh\:mm\:ss")},{victories},{losses}";

                        userFound = true;
                        break;
                    }
                }

                if (!userFound)
                {
                    List<string> newData = existingData.ToList();
                    newData.Add(dataToWrite);
                    existingData = newData.ToArray();
                }

                File.WriteAllLines(filePath, existingData);
            }
            else
            {
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
                    SaveWhitePlayerTime(_username, _password, true);
                }
                else
                {
                    SaveWhitePlayerTime(_username, _password, false);
                }
                MessageBox.Show(reason, "Game is ended", MessageBoxButton.OK, MessageBoxImage.Information);
                new MenuWindow(_username,_password).Show();
                this.Close();
                return;
            }
            
            _StartPlayerTimer(GameState.Turn);

            CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";

            _selectChecker(null);

            if (_typeGame == false)
                MakeMove();
        }
        
        private async Task MakeMove()
        {
            if (GameState.Turn == ColorChecker.Black)
            {
                var checkers = Board.Instance.GetCheckersByColor(GameState.Turn);

                if (checkers != null && checkers.Count > 0)
                {
                    bool hasTakenMove = false;

                    foreach (var checker in checkers)
                    {
                        List<int> availableMoves = Move.GetAvaibleMovesIndex(checker, checker.Color == ColorChecker.Black, out _);
                        Position oldPosition = checker.GetPosition();

                        if (availableMoves.Count > 0)
                        {
                            foreach (int moveIndex in availableMoves)
                            {
                                (int x, int y) = Position.FromIndex(moveIndex);
                                Position selectedPosition = new Position(x, y);
                                Move.CanMoveChecker(checker, selectedPosition, out Checker brokenChecker);

                                if (brokenChecker != null)
                                {
                                    Move.MoveChecker(checker, selectedPosition, out bool switchTurn);
                                    GameState.Delete(brokenChecker);
                                    brokenChecker.DeleteFromGame();
                                    await Task.Delay(1000);
                                    _changeImage(brokenChecker, brokenChecker.GetPosition(), brokenChecker.GetPosition());
                                    _changeImage(checker, selectedPosition, oldPosition);
                                    GameState.SwitchTurn();
                                    CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";
                                    _StartPlayerTimer(GameState.Turn);

                                    if (GameState.GameIsEnded(out ColorChecker winner, out string reason))
                                    {
                                        if (winner == ColorChecker.White)
                                        {
                                            SaveWhitePlayerTime(_username, _password, true);
                                        }
                                        else
                                        {
                                            SaveWhitePlayerTime(_username, _password, false);
                                        }
                                        MessageBox.Show(reason, "Game is ended", MessageBoxButton.OK, MessageBoxImage.Information);
                                        new MenuWindow(_username, _password).Show();
                                        this.Close();
                                        return;
                                    }

                                    hasTakenMove = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!hasTakenMove)
                    { 
                        Random random = new Random(); 
                        Checker randomChecker; 
                        Position oldPosition;
                        do 
                        { 
                            randomChecker = checkers[random.Next(checkers.Count)];
                            oldPosition = randomChecker.GetPosition();

                            List<int> availableMoves = Move.GetAvaibleMovesIndex(randomChecker, randomChecker.Color == ColorChecker.Black, out _);

                            if (availableMoves.Count > 0)
                            {
                                int randomMoveIndex = availableMoves[random.Next(availableMoves.Count)];
                                (int x, int y) = Position.FromIndex(randomMoveIndex);
                                Position selectedPosition = new Position(x, y);
                                Move.CanMoveChecker(randomChecker, selectedPosition, out Checker brokenChecker);

                                Move.MoveChecker(randomChecker, selectedPosition, out bool switchTurn);

                                await Task.Delay(1000);
                                if (brokenChecker != null)
                                {
                                    await Task.Delay(1000);
                                    GameState.Delete(brokenChecker);
                                    brokenChecker.DeleteFromGame();
                                    _changeImage(brokenChecker, brokenChecker.GetPosition(), brokenChecker.GetPosition());
                                }   

                                _changeImage(randomChecker, selectedPosition, oldPosition);

                                GameState.SwitchTurn();
                                CurrentPlayerTextBlock.Text = $"Ходит: {(GameState.Turn == ColorChecker.White ? "Белый" : "Черный")}";
                                _StartPlayerTimer(GameState.Turn);

                                if (GameState.GameIsEnded(out ColorChecker winner, out string reason))
                                {
                                    if (winner == ColorChecker.White)
                                    {
                                        SaveWhitePlayerTime(_username, _password,true);
                                    }
                                    else 
                                    {
                                        SaveWhitePlayerTime(_username, _password,false);
                                    }
                                    MessageBox.Show(reason, "Game is ended", MessageBoxButton.OK, MessageBoxImage.Information);
                                    new MenuWindow(_username, _password).Show();
                                    this.Close();
                                    return;
                                }

                                return;
                            }
                        } while (true);
                    }
                }
                else
                {
                    MessageBox.Show("Нет доступных ходов.");
                }
            }
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