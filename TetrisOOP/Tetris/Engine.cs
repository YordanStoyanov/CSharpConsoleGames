using System;
using System.Collections.Generic;
using System.Threading;

namespace Tetris
{
    public class Engine
    {
        //Settings
        static int tetrisRows = 20;
        static int tetrisCols = 10;
        static int tetrisInfo = 10;
        static List<Tetrominoe> tetrisFigures = new List<Tetrominoe>()
        {
            new Tetrominoe(new bool[,]//I 
            {
                { true, true, true, true }
            }),
            new Tetrominoe(new bool[,]//O
            {
                {true, true},
                {true, true}
            }),
            new Tetrominoe(new bool[,]//S
            {
                {false, true, true },
                {true, true, false }
            }),
            new Tetrominoe(new bool[,]//T
            {
                {false, true, false },
                {true, true, true }
            }),
            new Tetrominoe(new bool[,]//Z
            {
                {true, true, false },
                {false, true, true }
            }),
            new Tetrominoe(new bool[,]//J
            {
                {true, true, true},
                {true, false, false}
            }),
            new Tetrominoe(new bool[,]//L
            {
                {false, false, true },
                {true, true, true }
            }),
        };

        //Data
        private TetrisGameState gameState;
        public void Run()
        {
            gameState = new TetrisGameState(tetrisRows, tetrisCols);
            Random random = new Random();
            var tetrisConsoleWriter = new TetrisConsoleWriter(tetrisRows, tetrisCols, tetrisInfo);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            gameState.CurrentFigure = tetrisFigures[random.Next(0, tetrisFigures.Count)];
            KeyAvailable(tetrisConsoleWriter, random);
        }

        public void KeyAvailable(TetrisConsoleWriter tetrisConsoleWriter, Random random)
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (gameState.CurrentFigureCol >= 1)
                        {
                            gameState.CurrentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (gameState.CurrentFigureCol < tetrisCols - gameState.CurrentFigure.Height)
                        {
                            gameState.CurrentFigureCol++;
                        }

                    }
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        gameState.Frame = 1;
                        gameState.Score++;
                        gameState.CurrentFigureRow++;
                    }
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W || key.Key == ConsoleKey.Spacebar)
                    {
                        var newFigure = gameState.CurrentFigure.GetRotate();

                        if (!Collision(newFigure))
                        {
                            gameState.CurrentFigure = newFigure;
                        }
                    }
                }

                if (gameState.Frame % gameState.FrameToMoveFigure == 0)
                {
                    gameState.CurrentFigureRow++;
                    gameState.Frame = 0;
                }

                if (Collision(gameState.CurrentFigure))
                {
                    AddCurrentFigureToTheTetrisField();
                    int lines = CheckFrFullLines();
                    tetrisConsoleWriter.DrawAll(gameState);
                    gameState.CurrentFigure = tetrisFigures[random.Next(0, tetrisFigures.Count)];
                    gameState.CurrentFigureRow = 0;
                    gameState.CurrentFigureCol = 0;
                    if (Collision(gameState.CurrentFigure))
                    {
                        tetrisConsoleWriter.Write("Game over!", 10, 7);
                        Console.ReadKey();
                    }
                }
                gameState.Frame++;
                tetrisConsoleWriter.DrawAll(gameState);
                Thread.Sleep(10);
            }
        }

        //Tetrominoe.Rotate();

        public int CheckFrFullLines()
        {
            int lines = 0;

            for (int row = 0; row < gameState.TetrisField.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < gameState.TetrisField.GetLength(1); col++)
                {
                    if (gameState.TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }
                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove > 0; rowToMove--)
                    {
                        for (int col = 0; col < gameState.TetrisField.GetLength(1); col++)
                        {
                            gameState.TetrisField[rowToMove, col] = gameState.TetrisField[rowToMove - 1, col]; 
                        }
                    }
                    lines++;
                }
            }
            return lines;
        }
        public void AddCurrentFigureToTheTetrisField()
        {
            for (int row = 0; row < gameState.CurrentFigure.Wigth; row++)
            {
                for (int col = 0; col < gameState.CurrentFigure.Height; col++)
                {
                    if (gameState.CurrentFigure.Body[row, col])
                    {
                        gameState.TetrisField[gameState.CurrentFigureRow + row - 1, gameState.CurrentFigureCol + col] = true;
                    }
                }
            }
        }
        public bool Collision(Tetrominoe figure) 
        {
            if (gameState.CurrentFigureCol > tetrisCols - figure.Height)
            {
                return true;
            }
            if (gameState.CurrentFigureRow + figure.Wigth - 1 == tetrisRows)
            {
                return true; 
            }

            for (int row = 0; row < figure.Wigth; row++)
            {
                for (int col = 0; col < figure.Height; col++)
                {
                    if (figure.Body[row, col] && 
                        gameState.TetrisField[gameState.CurrentFigureRow + row,
                        gameState.CurrentFigureCol + col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
