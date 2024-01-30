using CheckersCore.Core.Move;
namespace CheckersCore.Core.Bot
{
    public static class Bot
    {
        public static Difficult Difficult { get; private set; }

        public static void SetDifficult(Difficult difficult)
        {
            Difficult = difficult;
        }

        public static void MakeMove()
        {

        }
        /*
        private static int _getScoreOfMove(char[,] board, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            if (depth == 0 || IsGameOver(board))
            {
                // Evaluate the board position (a simple heuristic for checkers could be the difference in the number of pieces)
                return EvaluateBoard(board);
            }

            var availableMoves = Move.Move.GetAvaibleMovesPositions(board, maximizingPlayer ? Player2 : Player1);

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in availableMoves)
                {
                    char[,] newBoard = Move.Move.MoveChecker(board, maximizingPlayer ? Player2 : Player1, move.Item1, move.Item2);
                    int eval = _getScoreOfMove(newBoard, depth - 1, alpha, beta, false);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);

                    if (beta <= alpha)
                        break;
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in availableMoves)
                {
                    char[,] newBoard = Move.Move.MoveChecker()

                    int eval = _getScoreOfMove(newBoard, depth - 1, alpha, beta, true);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);

                    if (beta <= alpha)
                        break;
                }
                return minEval;
            }
        }*/
    }
}
