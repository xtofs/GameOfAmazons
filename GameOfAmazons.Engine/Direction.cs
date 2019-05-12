using System.Collections.Generic;

namespace GameOfAmazons.Engine
{
    public struct Direction
    {
        public readonly int DX;

        public readonly int DY;

        public static readonly IEnumerable<Direction> All = new[]{
            new Direction(-1, 0),
            new Direction(0, 1),
            new Direction(1, 0),
            new Direction(0, -1),
            new Direction(-1, 1),
            new Direction(1, 1),
            new Direction(-1, 1),
            new Direction(-1, -1),
        };

        private Direction(int dx, int dy) : this()
        {
            DX = dx;
            DY = dy;
        }
    }

}
