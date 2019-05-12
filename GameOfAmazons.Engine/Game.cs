using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace GameOfAmazons.Engine
{
    using static Token;

    public class GameRunner
    {

    }

    public class Game
    {
        private readonly int w;
        private readonly int h;
        private readonly bool isWhitesMove;

        private readonly IList<(Token token, Position pos)> initial;
        private readonly ImmutableList<Move> moves;
        private readonly IDictionary<Position, Token> board;

        public bool IsWhitesMove => isWhitesMove;

        public int Moves => moves.Count;

        public Game(int w, int h, params (Token token, int x, int y)[] initial) :
            this(w, h, true, initial.Select(t => (t.token, new Position(t.x, t.y))), ImmutableList.Create<Move>())
        {
        }

        protected Game(int w, int h, bool isWhiteMove, IEnumerable<(Token token, Position pos)> initial, ImmutableList<Move> moves)
        {
            this.isWhitesMove = isWhiteMove;
            this.w = w;
            this.h = h;
            this.initial = initial.ToList();
            this.moves = moves;
            this.board = CreateBoard(initial, moves);
        }

        public static Game Create(string v, string chars = "OX#")
        {
            var valid = new Dictionary<char, Token> { [chars[0]] = White, [chars[1]] = Black, [chars[2]] = Fire };
            var array = v.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim().ToList()).ToList();
            var w = array.Select(row => row.Count).Distinct().Single();
            var h = array.Count;
            var initial = array
                .SelectMany((row, y) => row
                    .Select((chr, x) => valid.TryGetValue(chr, out var token) ? (token, x, y) : ((Token, int, int)?)null)
                    .WithValues()
                );
            return new Game(w, h, initial.ToArray());
        }

        public bool IsLegal(Move move, out string reason)
        {
            if (move.token != (isWhitesMove ? Token.White : Token.Black))
            {
                reason = "not ${color}'s move";
                return false;
            }
            reason = null;
            var result = this[move.from] == move.token &&
               this[move.to] == null &&
               (this[move.arrow] == null || move.arrow.Equals(move.from));
            return result;
        }
        public Game Move(Move move)
        {
            if (IsLegal(move, out var _))
            {
                return new Game(w, h, !isWhitesMove, initial, moves.Add(move));
            }
            throw new Exception();
        }

        public IEnumerable<Move> LegalMoves()
        {
            var color = (isWhitesMove ? Token.White : Token.Black);

            foreach (var origin in board.Where(kv => kv.Value == color).Select(kv => kv.Key))
            {
                foreach (var moreTarget in LegalTargets(origin, null)) // all positions the amazon can move to
                {
                    foreach (var shootTarget in LegalTargets(moreTarget, origin)) // all targets including original position
                    {
                        yield return new Move(color, origin, moreTarget, shootTarget); ;
                    }
                }
            }
        }

        private IEnumerable<Position> LegalTargets(Position from, Position? include)
        {
            var n = Math.Max(w, h);
            foreach (var direction in Direction.All)
            {
                foreach (var distance in Enumerable.Range(1, n))
                {
                    var target = from.Move(direction, distance);
                    var isLegal = IsInside(target) && (target == include || this[target] == null);
                    if (! isLegal)
                        break; // break out of the inner "distance" loop.
                    yield return target;
                }
            }
        }

        private bool IsInside(Position target)
        {
            return 0 <= target.X && target.X < w &&
            0 <= target.Y && target.Y < h;
        }

        public Token? this[Position pos] =>
            board.TryGetValue(pos, out var token) ? token : default(Token?);

        public Token? this[int x, int y] =>
            this[new Position(x, y)];

        private static IDictionary<Position, Token> CreateBoard(IEnumerable<(Token token, Position pos)> initial, ImmutableList<Move> moves)
        {
            var board = initial.ToDictionary(tuple => tuple.pos, tuple => tuple.token);

            foreach (var move in moves)
            {
                // TODO check legality or use .Move()
                board.Remove(move.from);
                board.Add(move.to, move.token);
                board.Add(move.arrow, Token.Fire);
            }
            return board;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(w * (h + 2));
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var t = this[new Position(x, y)];
                    sb.Append(t == null ? '.' : t == White ? 'O' : t == Black ? 'X' : t == Fire ? '#' : '?');
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }


    public static class EnumerableExtension
    {
        public static IEnumerable<T> WithValues<T>(this IEnumerable<T?> items) where T : struct
        {
            return from item in items where item.HasValue select item.Value;
        }
    }
}
