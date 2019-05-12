using System;
using System.Collections.Generic;

namespace GameOfAmazons.Engine.Console
{
    public static class EnumerableExtensions
    {
        public static T FindMimimum<T, K>(this IEnumerable<T> items, Func<T, K> selector, IComparer<K> comparer = null)
        {
            comparer = comparer ?? Comparer<K>.Default;
            var enumerator = items.GetEnumerator();
            if(! enumerator.MoveNext())
                throw new ArgumentOutOfRangeException();
            K minimum = selector(enumerator.Current);
            T minimalItem = enumerator.Current;
            while(enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var value = selector(item);
                if(comparer.Compare(value, minimum) < 0){
                    minimum = value;
                    minimalItem = item;
                }
            }
            return minimalItem;
        }
    }
}
