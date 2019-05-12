using System.Collections.Generic;
using Xunit;

namespace GameOfAmazons.Engine.Tests
{
    public class TokenSquareTests
    {
        public static IEnumerable<object[]> GetTokenSquarePairs()
        {
            yield return new object[] { Token.White, Square.White, true };
            yield return new object[] { Token.White, Square.Black, false };
            yield return new object[] { Token.White, Square.Fire, false };
            yield return new object[] { Token.White, Square.Empty, false };
            yield return new object[] { Token.Black, Square.White, false };
            yield return new object[] { Token.Black, Square.Black, true };
            yield return new object[] { Token.Black, Square.Fire, false };
            yield return new object[] { Token.Black, Square.Empty, false };
            yield return new object[] { Token.Fire, Square.White, false };
            yield return new object[] { Token.Fire, Square.Black, false };
            yield return new object[] { Token.Fire, Square.Fire, true };
            yield return new object[] { Token.Fire, Square.Empty, false };
        }

        [Theory]
        [MemberData(nameof(GetTokenSquarePairs))]
        public void Token_eq_Square(Token token, Square square, bool expected)
        {
            Assert.Equal(expected, token == square);
        }
    }
}
