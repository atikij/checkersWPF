using CheckersCore.Core;
using CheckersCore.Core.Player;
using System.Windows;

namespace CheckersUI
{
    public class GameState
    {
        public readonly int CountCheckers;

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

        public bool GameIsEnded(out Color winnerColor)
        {
            winnerColor = Color.None;

            if (_takenBlackCheckers.Count >= CountCheckers || _takenWhiteCheckers.Count >= CountCheckers)
            {
                winnerColor = _takenBlackCheckers.Count >= CountCheckers ? Color.White : Color.Black;

                return true;
            }

            // check to end moves

            return false;
        }

        public void SwitchTurn()
        {
            Turn = Turn == Color.White ? Color.Black 
                                       : Color.White;
        }
    }
}
