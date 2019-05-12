using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameOfAmazons.Engine
{
    public interface IStrategy
    {
        Move ChooseMove(Game game, IReadOnlyList<Move> moves);
    }

    public class GameRunnerWithStats : GameRunner
    {

        private readonly Counter<bool> whiteWinCounter;
        private readonly Timer<int> timer;

        public GameRunnerWithStats(IStrategy white, IStrategy black, bool trace = false) : base(white, black, trace)
        {

            whiteWinCounter = new Counter<bool>();
            timer = new Timer<int>();

        }
        public override Game Run(Game game)
        {
            var sw = Stopwatch.StartNew();
            var final = base.Run(game);
            sw.Stop();

            timer.Add(final.Moves, sw.Elapsed);
            whiteWinCounter[!final.IsWhitesTurn] += 1;

            return final;
        }

        public string ShowStats()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine(whiteWinCounter.ToString());
            sb.AppendLine(timer.ToString());
            return sb.ToString();
        }
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


        public virtual Game Run(Game game)
        {
            for (int i = 0; true; i++)
            {
                var moves = game.LegalMoves().ToArray();
                if (!moves.Any())
                {
                    log.Write("{0} can't move", game.IsWhitesTurn ? "White" : "Black");
                    break;
                }

                log.Write("{0} moves", game.IsWhitesTurn ? "White" : "Black");
                var move = game.IsWhitesTurn ? white.ChooseMove(game, moves) : black.ChooseMove(game, moves);
                game = game.ApplyMove(move);
                log.Write(game.ToString());
                log.Write();
            }

            log.Write("{0} wins after {1} moves", game.IsWhitesTurn ? "Black" : "White", game.Moves);
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
