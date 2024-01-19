using System.Drawing;

namespace CheckersCore.Core.Player
{
    public sealed class Checker
    {   
        public Color Color { get; private set; }

        public bool IsQueen { get; private set; } = false;

        private Position _position { get; set; }

        public Checker(Color color)
        {
            Color = color;
        }
        
        public void SetPosition(Position position)
        {
            if (_position != null)
            {
                // Instend of this checker, insert None
                Board.Instance[_position.X, _position.Y] = new Checker(Color.None);
            }

            // Set new position
            _position = new Position(position.X, position.Y);

            // Instend of this None, insert checker
            Board.Instance[_position.X, _position.Y] = this;
        }
        
        public void SetPosition(int x, int y)
        {
            if (_position != null)
            {
                // Instend of this checker, insert None
                Board.Instance[_position.X, _position.Y] = new Checker(Color.None);
            }

            // Set new position
            _position = new Position(x, y);

            // Instend of this None, insert checker
            Board.Instance[_position.X, _position.Y] = this;
        }

        public void DeleteFromGame()
        {
            // Change color to neutral
            this.Color = Color.None;

            // Instend of checker, insert None
            Board.Instance[_position.X, _position.Y] = new Checker(Color.None); ;
        }

        public Position GetPosition()
        {
            return _position;
        }

        public void SetToQueen()
        {
            IsQueen = true;
        }
    }
}
