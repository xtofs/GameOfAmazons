using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Engine.Console
{
    /// <summary>
    /// chooses the move that allows the opponent the least number of reachable squares.
    /// if two amazons can reach the same square it is counted twice.
    /// </summary>
    class BlockerStrategy : IStrategy
        {
            public Move ChooseMove(Game game, IReadOnlyList<Move> moves)
            {
                var options =
                    from move in moves
                    let it = game.ApplyMove(move)
                    select (
                        move,
                        count: game.GetAmazons(!move.isWhite).Select(pos => it.LegalTargets(pos).Count()).Sum());

                var (chosen, count) = options.FindMimimum(p => p.count);
                return chosen;
            }
        }
    }

