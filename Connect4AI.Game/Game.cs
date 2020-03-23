using GameGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect4AI.Game
{
    public class Game
    {

        private Board board;
        private Rules rules;

        public GameLog log = new GameLog();

        public Game(int numberOfColumns = 7, int numberOfRows = 6, int numberToWin = 4)
        {
            this.board = new Board(numberOfColumns, numberOfRows);
            this.rules = new Rules(numberToWin);

        }

        private const string LeftSideBuffer = "          ";

        private int PlayerTurn = 1;
        private Player2Strategy player2 = new Player2Strategy();

        public bool Play()
        {
            var logEntry = new GameLogEntry();
            logEntry.BoardBeforeMove = (int[,])board.Matrix.Clone();
            logEntry.PlayerNumber = PlayerTurn;

            DrawBoard();

            int column = -1;


            if (PlayerTurn == 1)
            {
                var promptColor = ConsoleColor.Blue ;
                Console.ForegroundColor = promptColor;
                Console.Write($"Select a column Player {PlayerTurn}: ");
                Console.ResetColor();

                var key = Console.ReadKey();
                var keyString = key.KeyChar.ToString();
                if (!int.TryParse(keyString, out column))
                {

                    if(key.Key == ConsoleKey.S)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter Name to Save: ");
                        var name = Console.ReadLine();
                        board.Name = name;
                        BoardJson.Serialize(board);
                        return true;
                    }

                    if (key.Key == ConsoleKey.L)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter Name to Load: ");
                        var name = Console.ReadLine();
                        board.Name = name;
                        board = BoardJson.Deserialize(name);
                        return true;
                    }


                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine($"Invalid column. Press any key");
                    Console.ReadLine();
                    return true;
                }
                column = column - 1;
         
            }
            else
            {
                column = player2.Run(board, 4);
                
            }
         
           
            if (rules.IsValidMove(column, board))
            {
                board.DropChecker(column, PlayerTurn);
                logEntry.ColumnPlayed = column;
                log.Add(logEntry);

                var winningGroups = rules.PlayerWins(PlayerTurn, board);
                if (winningGroups.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Green;
                    DrawBoard();
                    Console.WriteLine($"Player {PlayerTurn} wins!");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Invalid move. Press any key");
                Console.ReadLine();
                return true;
            }
            

            PlayerTurn = PlayerTurn == 1 ? 2 : 1;
            return true;  
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
            Console.Write("____________________________\n");
            Console.Write(LeftSideBuffer);
            Console.Write("  1   2   3   4   5   6   7\n");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
