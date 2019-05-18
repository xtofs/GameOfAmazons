using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfAmazons.Web.Models
{

    public class Game
    {
        public bool IsBlacksTurn { get; }

        public (int w, int h) size { get; }
        public Piece[,] board { get; }
        public List<Amazon> amazons { get; }
        public List<Coord> fires { get; }

        public Game(int[,] board)
        {
            this.board = board.Select(v => (Piece)v);
            this.fires = new List<Coord>();
            this.amazons = new List<Amazon>();

            this.size = (1 + board.GetUpperBound(0), 1 + board.GetUpperBound(1));

            for (int i = 0; i < size.w; i++)
            {
                for (int j = 0; j < size.h; j++)
                {
                    switch (this.board[i, j])
                    {
                        case Piece.Fire:
                            fires.Add(new Coord(i, j));
                            break;
                        case Piece.White:
                            amazons.Add(new Amazon(true, new Coord(i, j)));
                            break;
                        case Piece.Black:
                            amazons.Add(new Amazon(false, new Coord(i, j)));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public IEnumerable<Move> LegalMoves()
        {

            foreach (var az in amazons.Where(az => az.IsBlack == this.IsBlacksTurn))
            {
                foreach (var target in az.at.LegalMoves(this.size))
                {
                    foreach (var fire in target.LegalMoves(this.size))
                    {
                        yield return new Move
                        {
                            amazon = az.at,
                            target = target,
                            fire = fire
                        };
                    }
                }
            }
        }
    }

    public enum Direction
    {
        N = 1, E = 2, S = 4, W = 8,
        NE = 3, SE = 6, SW = 12, NW = 9
    }

    public static class Directions
    {
        private static Direction[] all = new[] { Direction.N, Direction.NE, Direction.E, Direction.SE, Direction.S, Direction.SW, Direction.W, Direction.NW };
        public static IEnumerable<Direction> All => all;

    }


    public struct Move
    {
        public Coord amazon;
        public Coord target;
        public Coord fire;
    }


    public struct Coord
    {
        public Coord(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; }

        public int y { get; }

        public static implicit operator Coord((int x, int y) tuple) =>
            new Coord(tuple.x, tuple.y);


        private Coord Transpose(Direction dir, int distance)
        {
            var dy = dir.HasFlag(Direction.N) ? -distance : dir.HasFlag(Direction.S) ? distance : 0;
            var dx = dir.HasFlag(Direction.W) ? -distance : dir.HasFlag(Direction.E) ? distance : 0;
             System.Diagnostics.Debug.Assert(this.x + dx >= 0);
             System.Diagnostics.Debug.Assert(this.y + dy >= 0);
            return new Coord(this.x + dx, this.y + dy);
        }

        public int DistanceToEdge(Direction dir, (int w, int h) size)
        {
            var (w, n, e, s) = (x, y, size.w - x - 1, size.h - y - 1);
            System.Diagnostics.Debug.Assert(w >= 0);
            System.Diagnostics.Debug.Assert(e >= 0);
            System.Diagnostics.Debug.Assert(n >= 0);
            System.Diagnostics.Debug.Assert(s >= 0);
            switch (dir)
            {
                case Direction.N:
                    return n;
                case Direction.S:
                    return s;
                case Direction.W:
                    return w;
                case Direction.E:
                    return e;
                case Direction.NW:
                    return Math.Min(n, w);
                case Direction.SW:
                    return Math.Min(s, w);
                case Direction.NE:
                    return Math.Min(n, e);
                case Direction.SE:
                    return Math.Min(s, e);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir));
            }
        }

        public IEnumerable<Coord> LegalMoves((int w, int h) size)
        {
            var c = this;
            return from dir in Directions.All
                   from distance in Enumerable.Range(1, c.DistanceToEdge(dir, size))
                   select c.Transpose(dir, distance);
        }
    }

    public struct Amazon
    {
        public Amazon(bool IsBlack, Coord coord) : this()
        {
            this.IsBlack = IsBlack;
            this.at = coord;
        }

        public Coord at { get; set; }
        public bool IsBlack { get; set; }
    }

    public enum Piece
    {
        Empty = 0, White = 1, Black = 2, Fire = 3
    }
}
