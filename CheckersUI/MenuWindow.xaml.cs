using System.Windows;

namespace CheckersUI
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void StartGame(object s, RoutedEventArgs e)
        {
            new MainWindow().Show();

            this.Close();
        }
    }
}
