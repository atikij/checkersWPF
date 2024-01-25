using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CheckersUI
{
    public static class HighLighter
    {
        public static void HideHighLight(Checker checker)
        {
            if (Board.Instance[checker.GetPosition().X, checker.GetPosition().Y].Color == Color.None)
                throw new Exception($"checker in position: x:{checker.GetPosition().X}, y:{checker.GetPosition().Y} not playable");

            UniformGrid grid = ((MainWindow)Application.Current.MainWindow).HighLightGrid;

            foreach (int index in Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, grid.Rows))
                ((Image)((MainWindow)Application.Current.MainWindow).HighLightGrid.Children[index]).Opacity = 0;
        }

        public static void ShowHighLight(Checker checker)
        {
            if (Board.Instance[checker.GetPosition().X, checker.GetPosition().Y].Color == Color.None)
                throw new Exception($"checker in position: x:{checker.GetPosition().X}, y:{checker.GetPosition().Y} not playable");

            UniformGrid grid = ((MainWindow)Application.Current.MainWindow).HighLightGrid;

            foreach (int index in Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, grid.Rows))
                ((Image)((MainWindow)Application.Current.MainWindow).HighLightGrid.Children[index]).Opacity = 0.7;
        }
    }
}
