using CheckersCore.Core.Player;

namespace CheckersCore.Core
{
    public class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x; 
            Y = y;
        }

        public static int ToIndex(int x, int y)
        {
            return y * 8 + x;
        }
        
        public static int ToIndex(Position position)
        {
            return position.Y * 8 + position.X;
        }

        public static (int, int) FromIndex(int index)
        {
            int x = index % 8;
            int y = index / 8;

            return (x, y);
        }
    }
}
