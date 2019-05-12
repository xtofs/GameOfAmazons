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
}
