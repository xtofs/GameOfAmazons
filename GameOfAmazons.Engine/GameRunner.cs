using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Engine
{
    public interface IStrategy
    {
        Move Move(Game game, IReadOnlyList<Move> moves);
    }

    public class GameRunner
    {
        private readonly IStrategy white;
        private readonly IStrategy black;

        private readonly ILogger log;

        public GameRunner(IStrategy white, IStrategy black, bool trace = false)
        {
            this.white = white ?? throw new System.ArgumentNullException(nameof(white));
            this.black = black ?? throw new System.ArgumentNullException(nameof(black));
            log = trace ? (ILogger)new ConsoleLogger() : new NullLogger();
        }

        public Game Run(Game game)
        {
            for (int i = 0; true; i++)
            {
                var moves = game.LegalMoves().ToArray();
                if (!moves.Any())
                {
                    log.Write("{0} can't move", game.IsWhitesMove ? "White" : "Black");
                    break;
                }

                log.Write("{0} moves", game.IsWhitesMove ? "White" : "Black");
                var move = game.IsWhitesMove ? white.Move(game, moves) : black.Move(game, moves);
                game = game.Move(move);
                log.Write(game.ToString());
                log.Write();
            }

            log.Write("{0} wins after {1} moves", game.IsWhitesMove ? "Black" : "White", game.Moves);
            return game;
        }

        interface ILogger { void Write(string format, params object[] args); void Write(); }
        class NullLogger : ILogger
        {
            public void Write(string format, params object[] args)
            {
            }

            public void Write()
            {
            }
        }

        class ConsoleLogger : ILogger
        {
            public void Write(string format, params object[] args)
            {
                System.Console.WriteLine(format, args);
            }

            public void Write()
            {
                System.Console.WriteLine();
            }
        }
    }
}
