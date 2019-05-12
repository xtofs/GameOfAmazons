namespace GameOfAmazons.Engine
{
    public struct Move
    {
        public readonly bool isWhite;
        public readonly Position from;
        public readonly Position to;
        public readonly Position arrow;

        public Move(bool isWhite, Position from, Position to, Position arrow)
        {
            this.isWhite = isWhite;
            this.from = from;
            this.to = to;
            this.arrow = arrow;
        }
    }
}
