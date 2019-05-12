using System;

namespace GameOfAmazons.Engine
{
    public struct Position : IEquatable<Position>
    {
        public readonly int X;
        public readonly int Y;
        public Position(int x, int y) : this()
        {
            this.X = x;
            this.Y = y;
        }
        public static implicit operator Position((int x, int y) pos) =>
            new Position(pos.x, pos.y);


        public override string ToString() => $"({X},{Y})";

        public override bool Equals(object obj) => obj is Position pos && Equals(pos);

        public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode();

        public static bool operator ==(Position c1, Position c2) => c1.Equals(c2);

        public static bool operator !=(Position c1, Position c2) => !c1.Equals(c2);

        public bool Equals(Position other) => this.X == other.X && this.Y == other.Y;
        public Position Move(Direction direction, int distance)
        {
            return new Position(X + direction.DX * distance, Y + direction.DY * distance);
        }
    }
}
