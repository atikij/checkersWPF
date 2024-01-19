
using CheckersCore.Core.Player;
using System.Collections.Generic;

namespace CheckersCore.Core.Move
{
    public static class Move
    {
        public static bool MoveChecker(Checker checker, Position toPosition, out Checker brokenChecker)
        {
            brokenChecker = null;

            if (checker == null)
                return false;

            List<(Position, Checker)> checkersToBreak = new List<(Position, Checker)>();

            List<Position> avaibleMoves = Move.GetAvaibleMovesPositions(checker, checker.Color == Color.Black, out checkersToBreak);

            if (avaibleMoves.Find(x => x.X == toPosition.X && x.Y == toPosition.Y) == null)
                return false;

            checker.SetPosition(toPosition);

            brokenChecker = checkersToBreak.Find(x => x.Item1.X == toPosition.X && x.Item1.Y == toPosition.Y).Item2;

            if (brokenChecker != null)
                brokenChecker.DeleteFromGame();

            return true;
        }

        /// <summary>
        /// Get avaible moves
        /// </summary>
        /// <param name="selectedChecker"></param>
        /// <param name="downCheckers"></param>
        /// <param name="checkersToBreak"><c>Item1</c> - mean position where a player need to jump to break other checker, <c>Item2 - checker to break if player jumped to need position</c></param>
        /// <param name="countRowsGrid"></param>
        /// <returns></returns>
        public static List<Position> GetAvaibleMovesPositions(Checker selectedChecker, bool downCheckers, out List<(Position, Checker)> checkersToBreak, int countRowsGrid = 8)
        {
            checkersToBreak = new List<(Position, Checker)>();

            List <Position> result = new List<Position>();

            Position selectedCheckerPosition = selectedChecker.GetPosition();

            if (selectedChecker.IsQueen)
            {
                int[,] directions = { { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int dx = directions[i, 0];
                    int dy = directions[i, 1];

                    for (int j = 1; j < countRowsGrid; j++)
                    {
                        int x = selectedCheckerPosition.X + j * dx;
                        int y = selectedCheckerPosition.Y + j * dy;

                        Position tempPos = new Position(x, y);

                        // TODO: check checkers in lines

                        if (Board.InBound(tempPos) && Board.IsEmpty(tempPos))
                        {
                            result.Add(tempPos);
                        }
                        else // Detect checker
                        {
                            if (!Board.IsEmpty(tempPos))
                            {
                                Checker checkerInPos = Board.Instance[tempPos.X, tempPos.Y];

                                if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                                {
                                    // Don't change
                                    Position moveAfterChecker = downCheckers ? new Position(tempPos.X - 1, tempPos.Y - 1) : new Position(tempPos.X - 1, tempPos.Y + 1);

                                    if (Board.IsEmpty(moveAfterChecker))
                                    {
                                        checkersToBreak.Add((tempPos, checkerInPos));

                                        result.Add(tempPos);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Position firstMove = downCheckers ? new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y - 1) : new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y + 1);
            Position secondMove = downCheckers ? new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y - 1) : new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y + 1);

            if (Board.InBound(firstMove) && Board.IsEmpty(firstMove))
            {
                result.Add(firstMove);
            }
            else // Detect checker
            {
                if (!Board.IsEmpty(firstMove))
                {
                    Checker checkerInPos = Board.Instance[firstMove.X, firstMove.Y];

                    if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                    {
                        // Don't change
                        Position moveAfterChecker = downCheckers ? new Position(firstMove.X - 1, firstMove.Y - 1) : new Position(firstMove.X - 1, firstMove.Y + 1);

                        if (Board.IsEmpty(moveAfterChecker))
                        {
                            checkersToBreak.Add((moveAfterChecker, checkerInPos));

                            result.Add(moveAfterChecker);
                        }
                    }
                }
            }

            if (Board.InBound(secondMove) && Board.IsEmpty(secondMove))
            {
                result.Add(secondMove);
            }
            else // Detect checker
            {
                if (!Board.IsEmpty(secondMove))
                {
                    Checker checkerInPos = Board.Instance[secondMove.X, secondMove.Y];

                    if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                    {
                        // Don't change
                        Position moveAfterChecker = downCheckers ? new Position(secondMove.X + 1, secondMove.Y - 1) : new Position(secondMove.X + 1, secondMove.Y + 1);

                        if (Board.IsEmpty(moveAfterChecker))
                        {
                            checkersToBreak.Add((moveAfterChecker, checkerInPos));

                            result.Add(moveAfterChecker);
                        }
                    }
                }
            }

            return result;
        }

        public static List<int> GetAvaibleMovesIndex(Checker checker, bool downCheckers, int countRowsGrid = 8)
        {
            List<int> result = new List<int>();

            foreach (Position pos in GetAvaibleMovesPositions(checker, downCheckers, out _, countRowsGrid))
                result.Add(Position.ToIndex(pos));

            return result;
        }
    }
}
