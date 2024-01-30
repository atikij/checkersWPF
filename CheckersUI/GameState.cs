using CheckersCore.Core;
using CheckersCore.Core.Move;
using CheckersCore.Core.Player;
using System.Windows;

namespace CheckersUI
{
    public class GameState
    {
        public readonly int CountCheckers;
        //public readonly Board Board;

        public Color Turn { get; private set; } = Color.White;

        private List<Checker> _takenBlackCheckers = new List<Checker>();
        private List<Checker> _takenWhiteCheckers = new List<Checker>();

        public GameState(int countCheckersForSide)
        {
            CountCheckers = countCheckersForSide;
        }

        public void StartGame()
        {
            Board.ResetBoard();

            _takenBlackCheckers.Clear();
            _takenWhiteCheckers.Clear();

            ((MainWindow)Application.Current.MainWindow).ResetGame();
        }

        public void Delete(Checker checker)
        {
            if (checker.Color == Color.White)
                _takenWhiteCheckers.Add(checker);
            else
                _takenBlackCheckers.Add(checker);
        }

        public bool GameIsEnded(out Color winnerColor, out string reason)
        {
            winnerColor = Color.None;
            reason = "";

            // Check if one player has taken all checkers of the other player
            if (_takenBlackCheckers.Count >= CountCheckers || _takenWhiteCheckers.Count >= CountCheckers)
            {
                winnerColor = _takenBlackCheckers.Count >= CountCheckers ? Color.White : Color.Black;
                reason = $"{winnerColor} won, all enemies were destroyed";

                return true;
            }

            // Check if the current player has any available moves
            var checkers = Board.Instance.GetCheckersByColor(Turn);

            if (checkers != null && checkers.Count > 0)
            {
                foreach (Checker checker in checkers)
                {
                    // Get available moves for the current checker
                    List<int> availableMoves = Move.GetAvaibleMovesIndex(checker, checker.Color == Color.Black, out _);

                    // If at least one player has a valid move, the game is not over
                    if (availableMoves.Count > 0)
                        return false;
                }

                reason = "Moves are over";
                return true;
            }

            // No winner or end condition met
            return false;
        }

        public void SwitchTurn()
        {
            Turn = Turn == Color.White ? Color.Black : Color.White;

            //((MainWindow)Application.Current.MainWindow).TextBlockCurrentColor.Text = Turn == Color.White ? "White" : "Black";
        }
    }
}
