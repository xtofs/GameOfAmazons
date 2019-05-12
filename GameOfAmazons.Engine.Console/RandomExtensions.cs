using System;
using System.Collections.Generic;

namespace GameOfAmazons.Engine.Console
{
    public static class RandomExtensions
    {
        public static T ChooseOne<T>(this Random rand, IReadOnlyList<T> items)
        {
            var n = rand.Next(items.Count);
            return items[n];
        }
    }
}
