using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Engine.Console
{
    using static Token;

    class Program
    {
        static void Main(string[] args)
        {
           

            var game = new Game(6, 6,
              (White, 2, 0), (White, 3, 5),
              (Black, 0, 2), (Black, 5, 3));

            var rand = new Random();
            for (int i = 0; true; i++)
            {
                var moves = game.LegalMoves().ToArray();
                if(! moves.Any())
                    break;

                System.Console.WriteLine("{0} moves", game.IsWhitesMove ? "White" : "Black");
                var move = rand.ChooseOne(moves);
                game = game.Move(move);

                System.Console.WriteLine(game.ToString());
            }
            
            System.Console.WriteLine("{0} wins after {1} moves", game.IsWhitesMove ? "Black" : "White", game.Moves);
        }
    }


    public static class RandomExtensions
    {
        public static T ChooseOne<T>(this Random rand, IList<T> items)
        {
            var n = rand.Next(items.Count);
            return items[n];
        }
    }
}
