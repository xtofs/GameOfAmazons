using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace GameOfAmazons.Engine
{
    using static Token;

    public class Game
    {
        private readonly int w;
        private readonly int h;
        private readonly bool isWhitesMove;

        private readonly int moves;

        private readonly Square[,] board;

        // primary constructor
        protected Game(int w, int h, bool isWhiteMove, int moves, Square[,] board)
        {
            this.w = w;
            this.h = h;
            this.isWhitesMove = isWhiteMove;
            this.moves = moves;
            this.board = board;
        }

        public Game(int w, int h, params (Token token, Position position)[] initial) :
            this(w, h, true, 0, new Square[w, h])
        {
            foreach (var (token, pos) in initial)
            {
                // TODO check isInside and no duplicate positions;
                board[pos.X, pos.Y] = (Square)token;
            }
        }
        public static Game Create(string v, string chars = "OX#")
        {
            var valid = new Dictionary<char, Token> { [chars[0]] = White, [chars[1]] = Black, [chars[2]] = Fire };
            var array = v.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim().ToList()).ToList();
            var w = array.Select(row => row.Count).Distinct().Single();
            var h = array.Count;
            var initial = array
                .SelectMany((row, y) => row
                    .Select((chr, x) => valid.TryGetValue(chr, out var token) ? (token, new Position(x, y)) : ((Token, Position)?)null)
                    .WithValues()
                )
                .ToArray();
            return new Game(w, h, initial);
        }

        public bool IsWhitesTurn => isWhitesMove;

        public int Moves => moves;


        public Game ApplyMove(Move move)
        {
            if (!IsLegal(move, out var reason))
            {
                throw new Exception(reason);
            }

            var copy = board.MakeCopy();
            var original = copy[move.from.X, move.from.Y];
            copy[move.from.X, move.from.Y] = Square.Empty;
            copy[move.to.X, move.to.Y] = original;
            copy[move.arrow.X, move.arrow.Y] = Square.Fire;

            var next = new Game(w, h, !isWhitesMove, moves + 1, copy);
            return next;
        }

        public bool IsLegal(Move move, out string reason)
        {
            var color = (isWhitesMove ? Token.White : Token.Black);
            if (move.isWhite != isWhitesMove)
            {
                reason = $"not {color}'s move";
                return false;
            }
            if (this[move.from] != color)
            {
                reason = $"{move.from} doesn't contain a ${color} amazon";
                return false;
            }
            reason = null;
            if (this[move.to] != Square.Empty)
            {
                reason = $"target {move.to} isn't empty";
                return false;
            }
            if (this[move.arrow] != Square.Empty && !move.arrow.Equals(move.from))
            {
                reason = $"arrow target {move.arrow} isn't empty";
                return false;
            }
            reason = null;
            return true;
        }


        public IEnumerable<Move> LegalMoves()
        {
            foreach (var origin in GetAmazons(isWhitesMove))
            {
                foreach (var moreTarget in LegalTargets(origin, null)) // all positions the amazon can move to
                {
                    foreach (var shootTarget in LegalTargets(moreTarget, origin)) // all targets including original position
                    {
                        yield return new Move(IsWhitesTurn, origin, moreTarget, shootTarget); ;
                    }
                }
            }
        }

        public IEnumerable<(int x, int y)> GetAmazons(bool white)
        {
            var color = (white ? Square.White : Square.Black);
            return board.Where((v, x, y) => v == color).Select((triple => (triple.x, triple.y)));
        }

        public IEnumerable<Position> LegalTargets(Position from, Position? include = null)
        {
            var n = Math.Max(w, h);
            foreach (var direction in Direction.All)
            {
                foreach (var distance in Enumerable.Range(1, n))
                {
                    var target = from.Move(direction, distance);
                    var isLegal = IsInside(target) && (target == include || this[target] == Square.Empty);
                    if (!isLegal)
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

        public Square this[Position pos] =>
            board[pos.X, pos.Y];

        public Square this[int x, int y] =>
            board[x, y];


        public override string ToString()
        {
            var sb = new StringBuilder(w * (h + 2));
            for (int y = 0; y < h; y++)
            {
                if (y != 0)
                {
                    sb.Append(Environment.NewLine);
                }
                for (int x = 0; x < w; x++)
                {

                    var t = this[new Position(x, y)];
                    sb.Append(t == Square.Empty ? '_' : t == Square.White ? 'O' : t == Square.Black ? 'X' : t == Square.Fire ? '#' : '?');
                }

            }
            return sb.ToString();
        }
    }
}
