namespace GameOfAmazons.Engine
{
    public struct Move
    {
        public readonly bool isWhite;
        public readonly Position start;
        public readonly Position end;
        public readonly Position arrow;

        public Move(bool isWhite, Position from, Position to, Position arrow)
        {
            this.isWhite = isWhite;
            this.start = from;
            this.end = to;
            this.arrow = arrow;
        }
    }
}
