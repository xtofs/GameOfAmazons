using System.Diagnostics;
using System.Threading;

namespace GameOfAmazons.Engine.Console
{
    using static Token;

    class Program
    {
        static void Main(string[] args)
        {

            var runner = new GameRunnerWithStats(new RandomStrategy(), new BlockerStrategy());


            var game = new Game(6, 6,
              (White, (2, 0)), (White, (3, 5)),
              (Black, (0, 2)), (Black, (5, 3)));


            for (int i = 0; i < 1000; i++)
            {
                var final = runner.Run(game);


                if (final.Moves <= 6 || final.Moves >= 31)
                {
                    System.Console.WriteLine("{0} wins after {1} moves.",
                        final.IsWhitesTurn ? "Black" : "White", final.Moves);
                    System.Console.WriteLine(final);
                    System.Console.WriteLine();
                }
            }

            System.Console.WriteLine(runner.ShowStats());
        }
    }
}