namespace Tetris
{
    public class Tetrominoe
    {
        public Tetrominoe(bool[,] body)
        {
            this.Body = body;
        }

        public bool[,] Body { get; private set; }

        public int Wigth
        {
            get
            {
                return this.Body.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return this.Body.GetLength(1);
            }
        }

        public Tetrominoe GetRotate()
        {
            var newFigure = new bool[this.Height, this.Wigth];
            for (int row = 0; row < this.Wigth; row++)
            {
                for (int col = 0; col < this.Height; col++)
                {
                    newFigure[col, this.Wigth - row - 1] = this.Body[row, col];
                }
            }
            return new Tetrominoe(newFigure);
        }
    }
}
