using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Engine
{
    class Counter<TKey>
    {
        private readonly IDictionary<TKey, int> d = new Dictionary<TKey, int>();
        public int this[TKey key]
        {
            get => d.TryGetValue(key, out var v) ? v : 0;
            set => d[key] = value;
        }



        public override string ToString() => string.Join(", ", d.Keys.OrderBy(i => i).Select(k => $"{k}: {d[k]}"));
    }


    class Timer<TKey>
    {
        private readonly IDictionary<TKey, (int n, double sum, double sumsq)> dict = 
            new Dictionary<TKey, (int, double, double)>();

        public void Add(TKey key, TimeSpan value)
        {
            var current = dict.TryGetValue(key, out var v) ? v : init;
            current.n += 1;
            current.sum += (double)value.Ticks;
            current.sumsq += (double)value.Ticks * (double)value.Ticks;
            dict[key] = current;
        }
        private static (int n, double sum, double sumsq) init = (0, 0, 0);

        public (int n, double μ, double σ) this[TKey key]
        {
            get
            {
                var (n, sum, ssq) = dict.TryGetValue(key, out var v) ? v : init;
                var μ = sum / n;
                var σ = Math.Sqrt(ssq / n - μ * μ);
                return (n, TimeSpan.FromTicks((long)μ).TotalMilliseconds, TimeSpan.FromTicks((long)σ).TotalMilliseconds);
            }
        }
        public override string ToString() =>
            string.Join("; ", dict.Keys.OrderBy(i => i).Select(k => $"{k}: {this[k].n}")) +
            Environment.NewLine +
            string.Join("; ", dict.Keys.OrderBy(i => i).Select(k => $"{k}: {this[k].μ:#.0}")) ;

    }
}
