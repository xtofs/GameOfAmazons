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
    }

    public struct Square
    {
        public readonly byte id;

        public Square(byte id)
        {
            this.id = id;
        }

        public static implicit operator Square(Token token) => new Square(token.id);

        public static readonly Square Empty = new Square(0);
        public static readonly Square White = new Square(1);
        public static readonly Square Black = new Square(2);
        public static readonly Square Fire = new Square(3);

        public static bool operator ==(Square a, Square b) => a.id == b.id;

        public static bool operator !=(Square a, Square b) => a.id != b.id;

        public override bool Equals(object obj) => obj is Square other && Equals(other) || obj is Token token && Equals(token);

        public override int GetHashCode() => id.GetHashCode();

        public bool Equals(Token other) => this.id == other.id;

        public bool Equals(Square other) => this.id == other.id;
    }
}
