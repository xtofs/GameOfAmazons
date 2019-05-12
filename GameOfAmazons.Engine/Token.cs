using System;

namespace GameOfAmazons.Engine
{
    public struct Token : IEquatable<Token>, IEquatable<Square>
    {
        public readonly byte id;
        private Token(byte id) : this()
        {
            this.id = id;
        }

        public static readonly Token White = new Token(1);
        public static readonly Token Black = new Token(2);
        public static readonly Token Fire = new Token(3);

        public static bool operator ==(Token a, Token b) => a.id == b.id;

        public static bool operator !=(Token a, Token b) => a.id != b.id;

        public override bool Equals(object obj) => obj is Square other && Equals(other) || obj is Token token && Equals(token);

        public override int GetHashCode() => id.GetHashCode();

        public bool Equals(Token other) => this.id == other.id;

        public bool Equals(Square other) => this.id == other.id;


        public override string ToString() => $"{{{names[id]} token}}";

        private static string[] names = new [] { "empty", "white", "black", "fire"};
    }
}
