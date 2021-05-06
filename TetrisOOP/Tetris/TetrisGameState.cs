using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    public class TetrisGameState
    {
        public TetrisGameState(int tetrisRow, int tetrisCol)
        {
            this.Score = 0;
            this.Frame = 0;
            this.FrameToMoveFigure = 30;
            this.CurrentFigureRow = 0;
            this.CurrentFigureCol = 0;
            this.CurrentFigure = null;
            this.TetrisField = new bool[tetrisRow, tetrisCol];
        }

        public int Score { get; set; }
        public int Frame { get; set; }
        public int FrameToMoveFigure { get; private set; }
        public int CurrentFigureRow { get; set; }
        public int CurrentFigureCol { get; set; }
        public Tetrominoe CurrentFigure { get; set; }
        public bool[,] TetrisField { get; private set; }
    }
}
