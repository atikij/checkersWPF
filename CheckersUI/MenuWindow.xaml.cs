using System.Windows;

namespace CheckersUI
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private string _username;
        private string _password;

        public MenuWindow(string username, string password)
        {
            InitializeComponent();
            _username = username;
            _password = password;
            LoggedInUserTextBlock.Text = _username;
        }

        private void StartGame(object s, RoutedEventArgs e)
        {
            new MainWindow(_username,_password,true).Show();
            this.Close();
        }

        private void Stats(object sender, RoutedEventArgs e)
        {
            new StatWindow(_username,_password).Show();
            this.Close();
        }
        
        private void Bot(object sender, RoutedEventArgs e)
        {
            //new BotWindow(_username,_password).Show();
            new MainWindow(_username,_password,false).Show();
            this.Close();
        }
    }
}
