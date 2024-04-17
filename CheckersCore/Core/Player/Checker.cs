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
                // Заменяем эту шашку на пустое поле
                Board.Instance[_position.X, _position.Y] = new Checker(Color.None);
            }

            // Устанавливаем новую позицию
            _position = new Position(position.X, position.Y);

            // Заменяем пустое поле на эту шашку
            Board.Instance[_position.X, _position.Y] = this;
        }
        
        public void SetPosition(int x, int y)
        {
            if (_position != null)
            {
                // Заменяем эту шашку на пустое поле
                Board.Instance[_position.X, _position.Y] = new Checker(Color.None);
            }

            // Устанавливаем новую позицию
            _position = new Position(x, y);

            // Заменяем пустое поле на эту шашку
            Board.Instance[_position.X, _position.Y] = this;
        }

        public void DeleteFromGame()
        {
            // Изменяем цвет на нейтральный
            this.Color = Color.None;

            // Заменяем шашку на пустое поле
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