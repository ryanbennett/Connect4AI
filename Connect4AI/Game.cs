using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4AI
{
    public class Game
    {

        private Board board;
        private Rules rules;

        public Game(int numberOfColumns = 7, int numberOfRows = 6, int numberToWin = 4)
        {
            this.board = new Board(numberOfColumns, numberOfRows);
            this.rules = new Rules(numberToWin);

        }

        private const string LeftSideBuffer = "          ";

        private int PlayerTurn = 1;

        public void Play()
        {

            DrawBoard();
            var promptColor = PlayerTurn == 1 ? ConsoleColor.Blue : PlayerTurn == 2 ? ConsoleColor.Red : ConsoleColor.White;
            Console.ForegroundColor = promptColor;
            Console.Write($"Select a column Player {PlayerTurn}: ");
            Console.ResetColor();
            var key = Console.ReadKey();

            int column = -1;

            var keyString = key.KeyChar.ToString();
            if (int.TryParse(keyString, out column))
            {
                column = column - 1;
                if (rules.IsValidMove(column, board))
                {
                    board.DropChecker(column, PlayerTurn);

                    if (rules.PlayerWins(PlayerTurn, board))
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Green;
                        DrawBoard();
                        Console.WriteLine($"Player {PlayerTurn} wins!");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine($"Invalid move. Press any key");
                    Console.ReadLine();
                    DrawBoard();
                    Play();
                }
            }
            else
            {

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Invalid column. Press any key");
                Console.ReadLine();
                DrawBoard();
                Play();
            }

            PlayerTurn = PlayerTurn == 1 ? 2 : 1;
            Play();  
        }

        private void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine();

            for(var row=board.MAX_ROW_INDEX; row>=0; row--)
            {
                var rArray = board.GetRow(row);

                var index = 0;
                foreach(var a in rArray)
                {
                    if (index == 0) Console.Write(LeftSideBuffer);
                    index++;
                    var mark = a == 1 ? "O" : a==2? "X" : " ";
                    var color = a == 1 ? ConsoleColor.Blue : a==2? ConsoleColor.Red : ConsoleColor.White;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("| ");

                    Console.ForegroundColor = color;
                    Console.Write(mark + " ");
                    Console.ResetColor();
                }

                Console.Write("|\n");
            }
            Console.Write(LeftSideBuffer);
            Console.Write("_________________________________\n");
            Console.Write(LeftSideBuffer);
            Console.Write("  1   2   3   4   5   6   7   8\n");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
