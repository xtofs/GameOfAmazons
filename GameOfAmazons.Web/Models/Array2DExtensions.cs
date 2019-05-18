using System;

namespace GameOfAmazons.Web.Models
{
    public static class Array2DExtensions
    {
        public static T[,] Select<S, T>(this S[,] array, Func<S, T> selector)
        {
            var n = 1 + array.GetUpperBound(0);
            int m = 1 + array.GetUpperBound(1);
            var result = new T[n,m];
            for (int i = 0; i < n; i++)
            {                
                for (int j = 0; j < m; j++)
                {
                    result[i, j] = selector(array[i, j]);
                }
            }
            return result;
        }
    }
}
