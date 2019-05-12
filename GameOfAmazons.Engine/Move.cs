namespace GameOfAmazons.Engine
{
    public struct Move
    {
        public readonly Token token;
        public readonly Position from;
        public readonly Position to;
        public readonly Position arrow;

        public Move(Token token, Position from, Position to, Position arrow)
        {
            this.token = token;
            this.from = from;
            this.to = to;
            this.arrow = arrow;
        }
    }
}
