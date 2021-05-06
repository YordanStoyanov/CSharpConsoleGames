using System;
using System.Collections.Generic;
using System.Text;
using Tetris;


namespace Tetris
{
    public class TetrisConsoleWriter
    {
        private int tetrisRows;
        private int tetrisCols;
        private int tetrisInfo;
        private int consoleRows;
        private int consoleCols ;// + 1 for forders
        public TetrisConsoleWriter(int tetrisRows, int tetrisCols, int tetrisInfo)
        {
            this.tetrisRows = tetrisRows;
            this.tetrisCols = tetrisCols;
            this.tetrisInfo = tetrisInfo;
            this.consoleRows = 1 + this.tetrisRows + 1;
            this.consoleCols = 1 + this.tetrisCols + 1 + this.tetrisInfo + 1;
            Console.WindowHeight = this.consoleRows + 1;
            Console.WindowWidth = this.consoleCols;
            Console.BufferHeight = this.consoleRows + 1;
            Console.BufferWidth = this.consoleCols;
        }
        public void DrawGameState(int startCol, TetrisGameState gameState)//3 + tetrisCols
        {
            this.Write("Score: ", 1, startCol);
            this.Write($"{gameState.Score}", 2, startCol);
            this.Write("Frame: ", 4, startCol);
            this.Write($"{gameState.Frame}", 5, startCol);
            this.Write("Keys: ", 7, startCol);
            this.Write("  ^  ", 9, startCol);
            this.Write("<   >", 10, startCol);
            this.Write("  v  ", 11, startCol);
        }
        public void DrawTetrisField(bool[,] tetrisField)
        {
            var tetrisConsoleWriter = new TetrisConsoleWriter(tetrisRows, tetrisCols, tetrisInfo);
            for (int row = 0; row < tetrisField.GetLength(0); row++)
            {
                for (int col = 0; col < tetrisField.GetLength(1); col++)
                {
                    if (tetrisField[row, col])
                    {
                        tetrisConsoleWriter.Write("*", row + 1, col + 1);
                    }
                }
            }
        }
        public void DrawCurrentFigure(Tetrominoe currentFigure, int currentFigureRow, int currentFigureCol)
        {
            var tetrisConsoleWriter = new TetrisConsoleWriter(tetrisRows, tetrisCols, tetrisInfo);
            for (int row = 0; row < currentFigure.Body.GetLength(0); row++)
            {
                for (int col = 0; col < currentFigure.Body.GetLength(1); col++)
                {
                    if (currentFigure.Body[row, col])
                    {
                        tetrisConsoleWriter.Write("*", row + 1 + currentFigureRow, col + 1 + currentFigureCol);
                    }
                }
            }
        }

        public void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.WriteLine($"{text}");

        }
        public void DrawBorder(int tetrisRows, int tetrisCols, int tetrisInfo)
        {
            Console.SetCursorPosition(0, 0);
            //StringBuilder next step!!!
            string upLine = "╔";
            upLine += new string('═', tetrisCols);
            //for (int i = 0; i < tetrisCols; i++)
            //{
            //    line += "═";
            //}
            upLine += "╦";
            upLine += new string('═', tetrisInfo);
            //for (int i = 0; i < tetrisInfo; i++)
            //{
            //    line += "═";
            //}
            upLine += "╗";
            Console.Write(upLine);

            for (int i = 0; i < tetrisRows; i++)
            {
                string middleLine = "║";
                middleLine += new string(' ', tetrisCols);
                middleLine += "║";
                middleLine += new string(' ', tetrisInfo);
                middleLine += "║";
                Console.WriteLine(middleLine);
            }

            string downLine = "╚";
            downLine += new string('═', tetrisCols);
            //for (int i = 0; i < tetrisCols; i++)
            //{
            //    line += "═";
            //}
            downLine += "╩";
            downLine += new string('═', tetrisInfo);
            //for (int i = 0; i < tetrisInfo; i++)
            //{
            //    line += "═";
            //}
            downLine += "╝";
            Console.Write(downLine);

        }

        public void DrawAll(TetrisGameState gameState)
        {
            this.DrawBorder(tetrisRows, tetrisCols, tetrisInfo);
            this.DrawGameState(3 + tetrisCols, gameState);
            this.DrawTetrisField(gameState.TetrisField);
            this.DrawCurrentFigure(gameState.CurrentFigure, gameState.CurrentFigureRow, gameState.CurrentFigureCol);
        }
    }
}
