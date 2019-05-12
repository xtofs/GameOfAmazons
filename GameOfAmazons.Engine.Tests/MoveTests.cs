using System;
using System.Linq;
using Xunit;
using static GameOfAmazons.Engine.Token;

namespace GameOfAmazons.Engine.Tests
{

    public class MoveTests
    {
        [Fact]
        public void Test1()
        {
            var game = Game.Create(GameOpening.StandardSixBySix);

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
            Assert.Equal(4, moves.Count);
            Assert.True(moves.Contains(new Move(true, (0, 1), (1, 2), (0, 1))));

            Assert.True(moves.Contains(new Move(true, (3, 4), (2, 4), (3, 4))));
            Assert.True(moves.Contains(new Move(true, (3, 4), (4, 5), (3, 4))));
            Assert.True(moves.Contains(new Move(true, (3, 4), (4, 5), (5, 4))));

        }
    }
}
