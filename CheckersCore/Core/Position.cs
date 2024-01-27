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

        public static bool operator ==(Position left, Position right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            if (ReferenceEquals(right, null))
                return false;

            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Position other = (Position)obj;

            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
