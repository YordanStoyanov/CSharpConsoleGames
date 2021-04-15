using System;
using System.Collections.Generic;
using System.Threading;

namespace Tetris
{
    public class Program
    {
        //Settings
        static int tetrisRows = 20;
        static int tetrisCols = 10;
        static int tetrisInfo = 10;
        static int consoleRows = 1 + tetrisRows + 1;
        static int consoleCols = 1 + tetrisCols + 1 + tetrisInfo + 1;// + 1 for forders
        static List<bool[,]> tetrisFigures = new List<bool[,]>()
        {
            new bool[,]//I 
            {
                { true, true, true, true }
            },
            new bool[,]//O
            {
                {true, true},
                {true, true}
            },
            new bool[,]//S
            {
                {false, true, true },
                {true, true, false }
            },
            new bool[,]//T
            {
                {false, true, false },
                {true, true, true }
            },
            new bool[,]//Z
            {
                {true, true, false },
                {false, true, true }
            },
            new bool[,]//J
            {
                {true, true, true},
                {true, false, false}
            },
            new bool[,]//L
            {
                {false, false, true },
                {true, true, true }
            },
        };

        //Data
        static int score = 0;
        static int frame = 0;
        static int frameToMoveFigure = 30;
        static int currentFigureRow = 0;
        static int currentFigureCol = 0;
        static bool[,] currentFigure = null; //TODO random
        static bool[,] tetrisField = new bool[tetrisRows, tetrisCols];
        static Random random = new Random();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Title = "Tetris v1.0";
            Console.WindowHeight = consoleRows + 1;
            Console.WindowWidth = consoleCols;
            Console.BufferHeight = consoleRows + 1;
            Console.BufferWidth = consoleCols;
            Console.CursorVisible = false;
            currentFigure = tetrisFigures[random.Next(0, tetrisFigures.Count)];
            //while (true)
            {
                //update state
                //read user input
                //draw again
            }
            while (true)
            {
                //score++;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        //Environment.Exit(0);
                        return; 
                    }
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (currentFigureCol >= 1)
                        {
                            currentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (currentFigureCol < tetrisCols - currentFigure.GetLength(1))
                        {
                            currentFigureCol++;
                        }
                        
                    }
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        frame = 1;
                        score++;
                        currentFigureRow++;
                        //TODO move current figure down
                    }
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W || key.Key == ConsoleKey.Spacebar)
                    {
                        RotateCurrentFigure();
                        //TODO implement turn on 90 degrees
                    }
                }

                if (frame % frameToMoveFigure == 0)
                {
                    currentFigureRow++;
                    frame = 0;
                }

                if (Collision(currentFigure))
                {
                    AddCurrentFigureToTheTetrisField();
                    int lines = CheckFrFullLines(); // 0, 1, 2, 3, 4
                    currentFigure = tetrisFigures[random.Next(0, tetrisFigures.Count)];
                    currentFigureRow = 0;
                    currentFigureCol = 0;
                    if (Collision(currentFigure))
                    {
                        Write("Game over!", 10, 7);
                        Console.ReadKey();
                    }
                }
                frame++;
                DrawBorder();
                DrawInfo();
                DrawTetrisField();
                DrawCurrentFigure();
                Thread.Sleep(10);
            }
        }

        private static void RotateCurrentFigure()
        {
            var newFigure = new bool[currentFigure.GetLength(1), currentFigure.GetLength(0)];
            for (int row = 0; row < currentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < currentFigure.GetLength(1); col++)
                {
                    newFigure[col, currentFigure.GetLength(0) - row - 1] = currentFigure[row, col];
                }
            }

            if (!Collision(newFigure))
            {
                currentFigure = newFigure;
            }
            
        }

        static int CheckFrFullLines()
        {
            int lines = 0;

            for (int row = 0; row < tetrisField.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < tetrisField.GetLength(1); col++)
                {
                    if (tetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }
                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove > 0; rowToMove--)
                    {
                        for (int col = 0; col < tetrisField.GetLength(1); col++)
                        {
                            tetrisField[rowToMove, col] = tetrisField[rowToMove - 1, col]; 
                        }
                    }
                    lines++;
                }
            }
            return lines;
        }

        static void AddCurrentFigureToTheTetrisField()
        {
            for (int row = 0; row < currentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < currentFigure.GetLength(1); col++)
                {
                    if (currentFigure[row, col])
                    {
                        tetrisField[currentFigureRow + row - 1, currentFigureCol + col] = true;
                    }
                }
            }
        }

        static bool Collision(bool[,] figure) 
        {
            if (currentFigureCol > tetrisCols - figure.GetLength(1))
            {
                return true;
            }
            //TODO cillide with existing figures 
            if (currentFigureRow + figure.GetLength(0) - 1 == tetrisRows)
            {
                return true; 
            }

            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] && 
                        tetrisField[currentFigureRow + row,
                        currentFigureCol + col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static void DrawBorder()
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
                middleLine +=  "║";
                middleLine += new string(' ', tetrisInfo);
                middleLine +=  "║";
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
        static void DrawInfo()
        {
            Write("Score: ", 1, 3 + tetrisCols);
            Write($"{score}", 2, 3 + tetrisCols);
            Write("Frame: ", 4, 3 + tetrisCols);
            Write($"{frame}", 5, 3 + tetrisCols);
            Write("Keys: ", 7, 3 + tetrisCols);
            Write("  ^  ", 9, 3 + tetrisCols);
            Write("<   >", 10, 3 + tetrisCols);
            Write("  v  ", 11, 3 + tetrisCols);

        }
        static void DrawTetrisField()
        {
            for (int row = 0; row < tetrisField.GetLength(0); row++)
            {
                for (int col = 0; col < tetrisField.GetLength(1); col++)
                {
                    if (tetrisField[row, col])
                    {
                        Write("*", row + 1, col + 1);
                    }
                }
            }
        }
        static void DrawCurrentFigure()
        {
            for (int row = 0; row < currentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < currentFigure.GetLength(1); col++)
                {
                    if (currentFigure[row, col])
                    {
                        Write("*", row + 1 + currentFigureRow, col + 1 + currentFigureCol);
                    }
                }
            }
        }
        static void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.WriteLine($"{text}");
        }
    }
}
