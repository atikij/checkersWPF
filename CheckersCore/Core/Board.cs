﻿using CheckersCore.Core.Player;
using System.Collections.Generic;

namespace CheckersCore.Core
{
    public class Board
    {
        public static Board Instance { get; private set; } = new Board();

        private Checker[,] _grid = new Checker[8, 8];

        public Board()
        {
            _initializeBoard();
        }

        public Checker this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x > 7 || y > 7)
                    return null;

                return _grid[x, y];
            }
            set 
            {
                if (x < 0 || y < 0 || x > 7 || y > 7)
                    return;

                _grid[x, y] = value;
            }
        }

        public static bool InBound(Position pos)
        {
            return pos.X <= 7 && pos.Y <= 7 && pos.X >= 0 && pos.Y >= 0;
        }

        public static bool IsEmpty(Position pos)
        {
            return Board.Instance[pos.X, pos.Y] != null && Board.Instance[pos.X, pos.Y].Color == Color.None;
        }

        public static void ResetBoard()
        {
            Instance = new Board();
        }

        public List<Checker> GetCheckersByColor(Color color)
        {
            List<Checker> result = new List<Checker>();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (Board.IsEmpty(new Position(x, y)))
                        continue;

                    Checker checker = Board.Instance[x, y];

                    if (checker.Color == color)
                        result.Add(checker);
                }
            }

            return result;
        }

        private void _initializeBoard()
        {
            // Заполняем доску пустыми клетками
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    this[x, y] = new Checker(Color.None);
                }
            }
            
            #region Белые шашки

            this[1, 0] = new Checker(Color.White);
            this[3, 0] = new Checker(Color.White);
            this[5, 0] = new Checker(Color.White);
            this[7, 0] = new Checker(Color.White);

            this[0, 1] = new Checker(Color.White);
            this[2, 1] = new Checker(Color.White);
            this[4, 1] = new Checker(Color.White);
            this[6, 1] = new Checker(Color.White);

            this[1, 2] = new Checker(Color.White);
            this[3, 2] = new Checker(Color.White);
            this[5, 2] = new Checker(Color.White);
            this[7, 2] = new Checker(Color.White);

            #endregion Белые шашки

            #region Черные шашки

            this[0, 5] = new Checker(Color.Black);
            this[2, 5] = new Checker(Color.Black);
            this[4, 5] = new Checker(Color.Black);
            this[6, 5] = new Checker(Color.Black);

            this[1, 6] = new Checker(Color.Black);
            this[3, 6] = new Checker(Color.Black);
            this[5, 6] = new Checker(Color.Black);
            this[7, 6] = new Checker(Color.Black);

            this[0, 7] = new Checker(Color.Black);
            this[2, 7] = new Checker(Color.Black);
            this[4, 7] = new Checker(Color.Black);
            this[6, 7] = new Checker(Color.Black);

            #endregion Черные шашки
        }
    }
}
