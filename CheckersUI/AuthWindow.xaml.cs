using System;
using System.IO;
using System.Windows;

namespace CheckersUI
{
    public partial class AuthWindow : Window
    {
        private const string UsersFileName = "D:\\rider repos\\checkers\\CheckersUI\\Assets\\users1.txt";

        public AuthWindow()
        {
            InitializeComponent();
        }

        private bool Authenticate(string username, string password)
        {
            if (File.Exists(UsersFileName))
            {
                string[] lines = File.ReadAllLines(UsersFileName);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2 && parts[0] == username && parts[1] == password)
                    {
                        return true; // Пользователь найден в файле и данные совпадают
                    }
                }
            }
            return false; // Пользователь не найден или данные не совпадают
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (Authenticate(username, password))
            {
                new MenuWindow(username,password).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }
        
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            new RegWindow().Show();
            this.Close();
        }

    }
}