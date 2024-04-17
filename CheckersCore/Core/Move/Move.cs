using CheckersCore.Core.Player;
using System.Collections.Generic;

namespace CheckersCore.Core.Move
{
    public static class Move
    {
        private static readonly Dictionary<Color, List<Position>> _positionsToQueen = new Dictionary<Color, List<Position>>()
        {
            { Color.White, new List<Position> () 
                {
                    new Position(0, 7),
                    new Position(1, 7),
                    new Position(2, 7),
                    new Position(3, 7),
                    new Position(4, 7),
                    new Position(5, 7),
                    new Position(6, 7),
                    new Position(7, 7),
                } 
            },
            { Color.Black, new List<Position> ()
                {
                    new Position(0, 0),
                    new Position(1, 0),
                    new Position(2, 0),
                    new Position(3, 0),
                    new Position(4, 0),
                    new Position(5, 0),
                    new Position(6, 0),
                    new Position(7, 0),
                } 
            },
        };

        public static bool CanMoveChecker(Checker checker, Position toPosition, out Checker brokenChecker)
        {
            Position fromPosition = checker.GetPosition();

            List<Position> avaibleMoves = Move.GetAvaibleMovesPositions(checker, checker.Color == Color.Black, out List<(Position, Checker)> checkersToBreak);

            brokenChecker = checkersToBreak.Find(x => x.Item1 == toPosition).Item2;

            return avaibleMoves.Contains(toPosition) && fromPosition != toPosition;
        }


        public static void MoveChecker(Checker checker, Position toPosition, out bool switchTurn)
        {
            checker?.SetPosition(toPosition);

            Move.GetAvaibleMovesPositions(checker, checker.Color == Color.Black, out List<(Position, Checker)> checkersToBreak);

            switchTurn = checkersToBreak.Count == 0;

            if (_positionsToQueen[checker.Color].Contains(toPosition))
                checker.SetToQueen();
        }

        public static List<int> GetAvaibleMovesIndex(Checker checker, bool downCheckers, out List<Position> positionsBrokenChecker, int countRowsGrid = 8)
        {
            positionsBrokenChecker = new List<Position>();
            
            List<int> result = new List<int>();

            List<Position> avaibleMoves = GetAvaibleMovesPositions(checker, downCheckers, out var checkersToBreak, countRowsGrid);

            foreach (Position pos in avaibleMoves)
                result.Add(Position.ToIndex(pos));
            
            foreach (var checkerBreak in checkersToBreak)
            {
                Position pos = checkerBreak.Item2.GetPosition();

                positionsBrokenChecker.Add(pos);
            }

            return result;
        }

        public static List<Position> GetAvaibleMovesPositions(Checker selectedChecker, bool downCheckers, out List<(Position, Checker)> checkersToBreak, int countRowsGrid = 8)
        {
            List<Position> result = new List<Position>();

            checkersToBreak = new List<(Position, Checker)>();

            Position selectedCheckerPosition = selectedChecker.GetPosition();

            if (selectedChecker.IsQueen)
            {
                result = _getAvaibleMovesQueen(selectedChecker, out checkersToBreak, countRowsGrid);

                return result;
            }

            Position firstMove = downCheckers ? new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y - 1) : new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y + 1);
            Position secondMove = downCheckers ? new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y - 1) : new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y + 1);

            if (Board.InBound(firstMove) && Board.IsEmpty(firstMove))
            {
                result.Add(firstMove);
            }
            else
            {
                if (!Board.IsEmpty(firstMove))
                {
                    Checker checkerInPos = Board.Instance[firstMove.X, firstMove.Y];

                    if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                    {
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
            else
            {
                if (!Board.IsEmpty(secondMove))
                {
                    Checker checkerInPos = Board.Instance[secondMove.X, secondMove.Y];

                    if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                    {
                        Position moveAfterChecker = downCheckers ? new Position(secondMove.X + 1, secondMove.Y - 1) : new Position(secondMove.X + 1, secondMove.Y + 1);

                        if (Board.IsEmpty(moveAfterChecker))
                        {
                            checkersToBreak.Add((moveAfterChecker, checkerInPos));

                            result.Add(moveAfterChecker);
                        }
                    }
                }
            }

            Position thirdMove = downCheckers ? new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y + 1) : new Position(selectedCheckerPosition.X - 1, selectedCheckerPosition.Y - 1);
            Position fourMove = downCheckers ? new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y + 1)  : new Position(selectedCheckerPosition.X + 1, selectedCheckerPosition.Y - 1);

            if (Board.InBound(thirdMove) && !Board.IsEmpty(thirdMove))
            {
                Checker checkerInPos = Board.Instance[thirdMove.X, thirdMove.Y];

                if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                {
                    Position moveAfterChecker = downCheckers ? new Position(thirdMove.X - 1, thirdMove.Y + 1) : new Position(thirdMove.X - 1, thirdMove.Y - 1);

                    if (Board.IsEmpty(moveAfterChecker))
                    {
                        checkersToBreak.Add((moveAfterChecker, checkerInPos));

                        result.Add(moveAfterChecker);
                    }
                }
            }

            if (Board.InBound(fourMove) && !Board.IsEmpty(fourMove))
            {
                Checker checkerInPos = Board.Instance[fourMove.X, fourMove.Y];

                if (checkerInPos != null && checkerInPos.Color != selectedChecker.Color)
                {
                    Position moveAfterChecker = downCheckers ? new Position(fourMove.X + 1, fourMove.Y + 1) : new Position(fourMove.X + 1, fourMove.Y - 1);

                    if (Board.IsEmpty(moveAfterChecker))
                    {
                        checkersToBreak.Add((moveAfterChecker, checkerInPos));

                        result.Add(moveAfterChecker);
                    }
                }
            }

            return result;
        }

        private static List<Position> _getAvaibleMovesQueen(Checker selectedChecker, out List<(Position, Checker)> checkersToBreak, int countRowsGrid = 8)
        {
            List<Position> result = new List<Position>();

            checkersToBreak = new List<(Position, Checker)>();

            Dictionary<Directional, List<Position>> movesQueen = _getAllQueenMoves(selectedChecker, countRowsGrid);

            foreach (var positions in movesQueen.Values)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    Position pos = positions[i];

                    // Проверка если на пути есть своя шашка, то следующие ходы скипаются
                    if (!Board.IsEmpty(pos))
                    {
                        Checker checkerInPos = Board.Instance[pos.X, pos.Y];

                        if (selectedChecker.Color == checkerInPos.Color)
                            break;

                        // Будем делать проверку на то, есть ли после шашки на позиции (pos) свободная клетка 
                        // Если все хорошо, мы делам проверку на тоже самое
                        if (i + 1 < positions.Count)
                        {
                            Position nextPos = positions[i + 1];

                            // Если на пути есть другая шашка после первой, то мы скипает следующие позиции
                            if (Board.InBound(nextPos) && !Board.IsEmpty(nextPos))
                                break;

                            for (int j = i + 1; j < positions.Count; j++)
                            {
                                nextPos = positions[j];

                                if (Board.InBound(nextPos) && Board.IsEmpty(nextPos))
                                    checkersToBreak.Add((nextPos, checkerInPos));
                            }
                        }
                    }
                    else
                    {
                        result.Add(pos);
                    }
                }
            }

            return result;
        }

        private static Dictionary<Directional, List<Position>> _getAllQueenMoves(Checker checker, int countRowsGrid = 8)
        {
            Directional[] directions =
            {
                new Directional(DirectionHorizontal.Left, DirectionVertical.Up),
                new Directional(DirectionHorizontal.Right, DirectionVertical.Up),
                new Directional(DirectionHorizontal.Left, DirectionVertical.Down),
                new Directional(DirectionHorizontal.Right, DirectionVertical.Down),
            };

            Dictionary<Directional, List<Position>> result = new Dictionary<Directional, List<Position>>()
            {
                { directions[0], new List<Position>() },
                { directions[1], new List<Position>() },
                { directions[2], new List<Position>() },
                { directions[3], new List<Position>() }
            };

            Position selectedCheckerPosition = checker.GetPosition();

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                Directional direction = directions[i];

                int dx = direction.IntLeftRight;
                int dy = direction.IntUpDown;

                for (int j = 1; j < countRowsGrid; j++)
                {
                    int xPos = selectedCheckerPosition.X + j * dx;
                    int yPos = selectedCheckerPosition.Y + j * dy;

                    Position tempPos = new Position(xPos, yPos);

                    if (Board.InBound(tempPos))
                        result[direction].Add(tempPos);                    
                }
            }

            return result;
        }

        struct Directional
        {
            public DirectionHorizontal LeftRight;
            public DirectionVertical UpDown;

            public int IntLeftRight;
            public int IntUpDown;

            public Directional(DirectionHorizontal leftRight, DirectionVertical upDown)
            {
                LeftRight = leftRight;
                UpDown = upDown;

                IntLeftRight = (int)LeftRight;
                IntUpDown = (int)UpDown;
            }
        }

        enum DirectionHorizontal
        {
            Left = -1,
            Right = 1,
        }

        enum DirectionVertical
        {
            Down = -1,
            Up = 1,
        }
    }
}
