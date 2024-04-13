using System.IO;
using System.Windows;

namespace CheckersUI
{
    public partial class RegWindow : Window
    {
        private const string UsersFileName = "D:\\rider repos\\checkers\\CheckersUI\\Assets\\users1.txt";

        public RegWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста напишите имя или пароль.");
                return;
            }

            // Проверка, существует ли уже пользователь с таким именем
            if (UserExists(username))
            {
                MessageBox.Show("Пользователь с таким именем уже есть!\nПожалуста выберите другое.");
                return;
            }

            // Форматирование строки для записи в файл
            string userData = $"{username},{password}";

            // Добавление нового пользователя в файл
            using (StreamWriter writer = File.AppendText(UsersFileName))
            {
                writer.WriteLine(userData); // Запись данных о пользователе в новой строке
                new AuthWindow().Show();
                this.Close();
            }

            MessageBox.Show("Регистрация успешна, теперь вы можете войти!");
            ClearFields();
        }


        private bool UserExists(string username)
        {
            if (File.Exists(UsersFileName))
            {
                string[] lines = File.ReadAllLines(UsersFileName);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length > 0 && parts[0] == username)
                    {
                        return true; // Пользователь уже существует
                    }
                }
            }
            return false; // Пользователь не существует
        }

        private void ClearFields()
        {
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
        }
    }
}
