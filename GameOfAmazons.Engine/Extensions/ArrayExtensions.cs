using System;
using System.Collections.Generic;

namespace GameOfAmazons.Engine
{
    public static class ArrayExtensions
    {
        public static T[,] MakeCopy<T>(this T[,] array) where T : struct
        {
            var (w,h) = (1 + array.GetUpperBound(0), 1 + array.GetUpperBound(1));
            var copy = new T[w,h];
            Array.Copy(array, copy, w*h);
            return copy;
        }

         public static (int x, int y) GetDimensions<T>(this T[,] array)
            => (1+array.GetUpperBound(0), 1+array.GetUpperBound(1));

        public static IEnumerable<(T value, int x, int y)> Where<T>(this T[,] array, Func<T, int, int, bool> predicate)
           where T : struct
        {
            var (w,h) = array.GetDimensions();
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if(predicate(array[x,y], x, y))
                        yield return (array[x,y], x, y);
                }
            }
        }
    }
}
