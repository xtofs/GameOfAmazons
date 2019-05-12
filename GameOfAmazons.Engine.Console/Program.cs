using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameOfAmazons.Engine.Console
{
    using static Token;

    class Program
    {
        static void Main(string[] args)
        {

            var runner = new GameRunner(new RandomStrategy(), new RandomStrategy());


            var game = new Game(6, 6,
              (White, (2, 0)), (White, (3, 5)),
              (Black, (0, 2)), (Black, (5, 3)));

            var colorCounter = new Counter<bool>();
            var countCounter = new Counter<int>();

            for (int i = 0; i < 10000; i++)
            {
                var sw = Stopwatch.StartNew();
                var final = runner.Run(game);
                sw.Stop();

                colorCounter[final.IsWhitesMove] += 1;
                countCounter[final.Moves] += 1;

                if (final.Moves <= 10 || final.Moves >= 33) {
                    System.Console.WriteLine(final);
                    System.Console.WriteLine(final.IsWhitesMove);
                    System.Console.WriteLine(final.Moves);
                    System.Console.WriteLine();
                }
                // System.Console.WriteLine("{0} wins after {1} moves in {2}", w ? "White" : "Black", c, sw.Elapsed);
            }

            System.Console.WriteLine(colorCounter);
            System.Console.WriteLine(countCounter);
        }

        class RandomStrategy : IStrategy
        {
            private readonly Random rand = new Random();

            public Move Move(Game game, IReadOnlyList<Move> moves)
            {
                return rand.ChooseOne(moves);

            }
        }
    }

    class Counter<TKey>
    {
        private readonly IDictionary<TKey, int> d = new Dictionary<TKey, int>();
        public int this[TKey key]
        {
            get => d.TryGetValue(key, out var v) ? v : 0;
            set => d[key] = value;
        }



        public override string ToString() => string.Join(" ", d.Keys.OrderBy(i => i).Select(k => $"{k}={d[k]}"));
    }
}
