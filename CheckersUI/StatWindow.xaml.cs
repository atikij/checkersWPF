using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CheckersUI
{
    public partial class StatWindow : Window
    {
        private string _username;
        private string _password;

        public StatWindow(string username, string password)
        {
            InitializeComponent();
            _username = username;
            _password = password;

            // Чтение данных из файла
            List<PlayerStats> playerStatsList = ReadPlayerStatsFromFile("D:\\rider repos\\checkers\\CheckersUI\\Assets\\users1.txt");

            // Привязка списка игроков к ListView в каждой вкладке
            TimeListView.ItemsSource = playerStatsList
                .OrderBy(player => TimeSpan.Parse(player.BestTime))
                .ToList();

            WinListView.ItemsSource = playerStatsList
                .OrderByDescending(player => player.Victories)
                .ToList();

            StatListView.ItemsSource = playerStatsList
                .Where(player => player.Username == _username && player.Password == _password)
                .ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow(_username,_password).Show();
            this.Close();
        }

        private List<PlayerStats> ReadPlayerStatsFromFile(string filePath)
        {
            List<PlayerStats> playerStatsList = new List<PlayerStats>();

            if (File.Exists(filePath))
            {
                // Чтение строк из файла
                string[] lines = File.ReadAllLines(filePath);

                // Обработка каждой строки
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    // Проверка наличия времени у игрока
                    string username = parts[0];
                    string password = parts.Length > 1 ? parts[1] : "";
                    string time = parts.Length > 2 ? parts[2] : "-";
                    int victories = parts.Length > 3 ? int.Parse(parts[3]) : 0;
                    int losses = parts.Length > 4 ? int.Parse(parts[4]) : 0;

                    playerStatsList.Add(new PlayerStats(username, password, time, victories, losses));
                }
            }

            return playerStatsList;
        }
    }
}

    public class PlayerStats
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string BestTime { get; set; }
        public int Victories { get; set; }
        public int Losses { get; set; }

        public PlayerStats(string username, string password, string bestTime, int victories, int losses)
        {
            Username = username;
            Password = password;
            BestTime = bestTime;
            Victories = victories;
            Losses = losses;
        }
    }

