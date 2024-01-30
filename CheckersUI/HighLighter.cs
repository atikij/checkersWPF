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
            
            List<int> avaibleIndexes = Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, out var positiosToRed, grid.Rows);

            foreach (int index in avaibleIndexes)
                ((Image)((MainWindow)Application.Current.MainWindow).HighLightGrid.Children[index]).Opacity = 0;

            foreach (Position pos in positiosToRed)
                _hideRedLight(pos);
        }

        public static void ShowHighLight(Checker checker)
        {
            if (Board.Instance[checker.GetPosition().X, checker.GetPosition().Y].Color == Color.None)
                throw new Exception($"checker in position: x:{checker.GetPosition().X}, y:{checker.GetPosition().Y} not playable");

            UniformGrid grid = ((MainWindow)Application.Current.MainWindow).HighLightGrid;

            List<int> avaibleIndexes = Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, out var positiosToRed, grid.Rows);

            foreach (int index in avaibleIndexes)
                ((Image)((MainWindow)Application.Current.MainWindow).HighLightGrid.Children[index]).Opacity = 0.5;
            
            foreach (Position pos in positiosToRed)
                _showRedLight(pos);
        }

        private static void _showRedLight(Position position)
        {
            int index = Position.ToIndex(position);

            ((Image)((MainWindow)Application.Current.MainWindow).RedLightGrid.Children[index]).Opacity = 0.3;
        }

        private static void _hideRedLight(Position position)
        {
            int index = Position.ToIndex(position);

            ((Image)((MainWindow)Application.Current.MainWindow).RedLightGrid.Children[index]).Opacity = 0;
        }
    }
}
