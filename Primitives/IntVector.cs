namespace Core.GameLogic.MazeGen.Primitives
{
    public struct IntVector
    {
        public int X { get; }
        public int Y { get; }

        public IntVector(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static IntVector Right => new IntVector(1, 0);
        public static IntVector Left => new IntVector(-1, 0);
        public static IntVector Top => new IntVector(0, 1);
        public static IntVector Down => new IntVector(0, -1);
        public static IntVector RightTop => new IntVector(1, 1);
        public static IntVector RightDown => new IntVector(1, -1);
        public static IntVector LeftTop => new IntVector(-1, 1);
        public static IntVector LeftDown => new IntVector(-1, -1);
        
        public static IntVector Zero => new IntVector(0, 0);

        public static readonly IntVector[] OrtNormals =
        {
            Right,
            Left,
            Top,
            Down
        };

        public static readonly IntVector[] DiaNormals =
        {
            RightTop,
            RightDown,
            LeftTop,
            LeftDown
        };


        
        private bool Equals(IntVector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is IntVector other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator >=(IntVector n1, IntVector n2) => n1.X >= n2.X && n1.Y >= n2.Y;
        public static bool operator <=(IntVector n1, IntVector n2) => n1.X <= n2.X && n1.Y <= n2.Y;
        public static bool operator >(IntVector n1, IntVector n2) => n1.X > n2.X && n1.Y > n2.Y;
        public static bool operator <(IntVector n1, IntVector n2) => n1.X < n2.X && n1.Y < n2.Y;
        public static bool operator ==(IntVector n1, IntVector n2) => n1.Equals(n2);
        public static bool operator !=(IntVector n1, IntVector n2) => !n1.Equals(n2);
        public static IntVector operator / (IntVector n, int number) => new IntVector(n.X / number, n.Y / number);
        public static IntVector operator -(IntVector n) => new IntVector(-n.X, -n.Y);
        public static IntVector operator +(IntVector i1, IntVector i2) => new IntVector(i1.X + i2.X, i1.Y + i2.Y);
        public static IntVector operator -(IntVector i1, IntVector i2) => new IntVector(i1.X - i2.X, i1.Y - i2.Y);
    }
}