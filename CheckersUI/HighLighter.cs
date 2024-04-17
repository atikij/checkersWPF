using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System;
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
                throw new Exception($"шашка на позиции: x:{checker.GetPosition().X}, y:{checker.GetPosition().Y} не играбельна");

            Panel grid = GetHighlightGrid();
            
            List<int> avaibleIndexes = Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, out var positiosToRed, GetGridRows(grid));

            foreach (int index in avaibleIndexes)
                ((Image)grid.Children[index]).Opacity = 0;

            foreach (Position pos in positiosToRed)
                _hideRedLight(pos);
        }

        public static void ShowHighLight(Checker checker)
        {
            if (Board.Instance[checker.GetPosition().X, checker.GetPosition().Y].Color == Color.None)
                throw new Exception($"шашка на позиции: x:{checker.GetPosition().X}, y:{checker.GetPosition().Y} не играбельна");

            Panel grid = GetHighlightGrid();

            List<int> avaibleIndexes = Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, out var positiosToRed, GetGridRows(grid));

            foreach (int index in avaibleIndexes)
                ((Image)grid.Children[index]).Opacity = 0.5;
            
            foreach (Position pos in positiosToRed)
                _showRedLight(pos);
        }

        private static void _showRedLight(Position position)
        {
            int index = Position.ToIndex(position);

            ((Image)GetRedLightGrid().Children[index]).Opacity = 0.3;
        }

        private static void _hideRedLight(Position position)
        {
            int index = Position.ToIndex(position);

            ((Image)GetRedLightGrid().Children[index]).Opacity = 0;
        }

        private static Panel GetHighlightGrid()
        {
            Window currentWindow = Application.Current.MainWindow;
            if (currentWindow is MainWindow mainWin)
            {
                return mainWin.HighLightGrid;
            }
            else if (currentWindow is BotWindow botWin)
            {
                return botWin.HighLightGrid;
            }
            else
            {
                throw new InvalidOperationException("Невозможно определить тип текущего окна.");
            }
        }

        private static Panel GetRedLightGrid()
        {
            Window currentWindow = Application.Current.MainWindow;
            if (currentWindow is MainWindow mainWin)
            {
                return mainWin.RedLightGrid;
            }
            else if (currentWindow is BotWindow botWin)
            {
                return botWin.RedLightGrid;
            }
            else
            {
                throw new InvalidOperationException("Невозможно определить тип текущего окна.");
            }
        }

        private static int GetGridRows(Panel grid)
        {
            if (grid is UniformGrid uniformGrid)
            {
                return uniformGrid.Rows;
            }
            else if (grid is Grid gridPanel)
            {
                return gridPanel.RowDefinitions.Count;
            }
            else
            {
                throw new NotSupportedException("Тип панели не поддерживается.");
            }
        }
    }
}
