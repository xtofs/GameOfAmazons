using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static GameOfAmazons.Engine.Token;

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

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var game = new Game(6, 6,
                (White, (2, 0)), (White, (3, 5)),
                (Black, (0, 4)), (Black, (5, 3)));

            game = game.ApplyMove(new Move(true, new Position(2, 0), new Position(2, 3), new Position(3, 3)));


            var moves = game.LegalMoves().ToArray();

        }
        [Fact]
        public void Test2()
        {
            var game = Game.Create(@"
##.#..
O###X#
#.###.
######
X#.O#.
####.#");
            var moves = game.LegalMoves().ToList();
            Assert.Equal(3, moves.Count);
            Assert.True(moves.Contains(new Move(true, (0, 1), (1, 2), (0, 1))));

            Assert.True(moves.Contains(new Move(true, (3, 4), (2, 4), (3, 4))));
            Assert.True(moves.Contains(new Move(true, (3, 4), (4, 5), (3, 4))));


        }
    }
}
