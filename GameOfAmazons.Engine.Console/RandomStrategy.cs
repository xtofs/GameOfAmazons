using System;
using System.Collections.Generic;

namespace GameOfAmazons.Engine.Console
{
    class RandomStrategy : IStrategy
        {
            private readonly Random rand = new Random();

            public Move ChooseMove(Game game, IReadOnlyList<Move> moves)
            {
                return rand.ChooseOne(moves);
            }
        }
    }

