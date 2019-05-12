using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Engine
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> WithValues<T>(this IEnumerable<T?> items) where T : struct
        {
            return from item in items where item.HasValue select item.Value;
        }
    }
}
