using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace GameOfAmazons.Engine
{
    using static Token;

    public class GameOpening
    {
        public readonly int w;
        public readonly int h;
        public readonly (Token token, Position position)[] initialTokenPositions;

        public GameOpening(int w, int h, params (Token token, Position position)[] initialTokenPositions)
        {
            this.w = w;
            this.h = h;
            this.initialTokenPositions = initialTokenPositions ?? throw new ArgumentNullException(nameof(initialTokenPositions));
        }


        public static GameOpening StandardSixBySix = new GameOpening(6, 6,
              (White, (2, 0)), (White, (3, 5)),
              (Black, (0, 2)), (Black, (5, 3)));
    }

    public class Game
    {
        private readonly int w;
        private readonly int h;
        private readonly bool isWhitesMove;

        private readonly int moves;

        private readonly Square[,] board;

        // primary constructor
        protected Game(bool isWhiteMove, int moves, Square[,] board)
        {
            this.isWhitesMove = isWhiteMove;
            this.moves = moves;
            this.board = board;
            (w, h) = board.GetDimensions();
        }

        public static Game Create(GameOpening opening)
        {
            var (w, h) = (opening.w, opening.h);
            var board = new Square[w, h];
            foreach (var (token, pos) in opening.initialTokenPositions)
            {
                // TODO check isInside and no duplicate positions;
                board[pos.X, pos.Y] = (Square)token;
            }
            return new Game(true, 0, board);
        }

        public static Game Create(string v, string chars = "OX#")
        {
            var valid = new Dictionary<char, Token> { [chars[0]] = White, [chars[1]] = Black, [chars[2]] = Fire };
            var array = v.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim().ToList()).ToList();
            var w = array.Select(row => row.Count).Distinct().Single();
            var h = array.Count;
            var board = new Square[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var square = valid.TryGetValue(array[y][x], out var token) ? token : Square.Empty;
                    board[x, y] = square;
                }
            }
            return new Game(true, 0, board);
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
            var original = copy[move.start.X, move.start.Y];
            copy[move.start.X, move.start.Y] = Square.Empty;
            copy[move.end.X,   move.end.Y]   = original;
            copy[move.arrow.X, move.arrow.Y] = Square.Fire;

            var next = new Game(!isWhitesMove, moves + 1, copy);
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
            if (this[move.start] != color)
            {
                reason = $"{move.start} doesn't contain a ${color} amazon";
                return false;
            }
            reason = null;
            if (this[move.end] != Square.Empty)
            {
                reason = $"target {move.end} isn't empty";
                return false;
            }
            if (this[move.arrow] != Square.Empty && !move.arrow.Equals(move.start))
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

        /// <summary>
        /// Legal targets to move an Amazon or shoot an Arrow.
        /// These are all the squares that are empty, can be reached by linear 
        /// horizontal, vertical, or diagonal moves until another piece blocks it.
        /// </summary>
        /// <param name="from">starting position of the move</param>
        /// <param name="include">a single position that should be considered empty, 
        /// therefore included and not blocking the movement.</param>
        /// <returns>a sequence of legal movement targets</returns>
        public IEnumerable<Position> LegalTargets(Position from, Position? include = null)
        {
            var (w, h) = board.GetDimensions();
            var n = Math.Max(w, h);
            foreach (var direction in Direction.All)
            {
                foreach (var distance in Enumerable.Range(1, n))
                {
                    var target = from.Move(direction, distance);
                    var isLegal = board.IsValid(target) && (target == include || this[target] == Square.Empty);
                    if (!isLegal)
                        break; // break out of the inner "distance" loop.
                    yield return target;
                }
            }
        }

        public Square this[Position pos] =>
            board[pos.X, pos.Y];

        public override string ToString()
        {
            var (w, h) = board.GetDimensions();
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
