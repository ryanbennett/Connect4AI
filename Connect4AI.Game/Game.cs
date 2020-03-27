using GameGenerator;
using System;
using System.Collections.Generic;
using System.IO;
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
            this.numberToWin = numberToWin;
        }

        MinMaxStrategy strategy1 = new MinMaxStrategy();
        MinMaxStrategy strategy2 = new MinMaxStrategy();

        private const string LeftSideBuffer = "          ";
        private readonly int numberToWin;
        private int PlayerTurn = 1;

        private string filePath;


        public bool Replay(string filePath)
        {

            GameLog log = GameLog.Load(filePath);

            foreach(var entry in log.Log)
            {
                board.Matrix = entry.BoardBeforeMove;

                var nextStep = false;
                var startNewGame = false;
                while (!nextStep)
                {
                    nextStep = RunEntry(entry, out startNewGame);
                }

                if (startNewGame)
                {
                    return true;
                }

                EndTurn(new GameLogEntry(), entry.ColumnPlayed);
            }

            
            return false;

        }

        private bool RunEntry(GameLogEntry entry, out bool startNewGame)
        {
            startNewGame = false;
            DrawBoard();
            Console.WriteLine();
            Console.ForegroundColor = PlayerTurn == 1 ? ConsoleColor.Blue : ConsoleColor.Red;
            Console.WriteLine($"Player {PlayerTurn} plays {entry.ColumnPlayed + 1}. (N)ext Move, (S)tart game here, (R)un stratgey?");
            Console.ResetColor();
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.N)
            {
                return true;
            }
            else if (key.Key == ConsoleKey.S)
            {
                startNewGame = true;
                return false;
            }
            else if (key.Key == ConsoleKey.R)
            {
                var strategy = new MinMaxStrategy();
                var column = strategy.Run(board, numberToWin, PlayerTurn);
                Console.WriteLine();
                Console.WriteLine($"Strategy chose {column}.");
                Console.ReadLine();
                return false;
            }

            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine($"Invalid column. Press any key");
            Console.ReadLine();
            return false;
        }

        public bool Play(bool twoPlayer)
        {
            GameLogEntry logEntry = NewLogEntry();

            DrawBoard();

            int column = -1;

            if (PlayerTurn == 1 || twoPlayer)
            {
                ConsoleKeyInfo key;
                string keyString;
                Prompt(out key, out keyString);

                if (!int.TryParse(keyString, out column))
                {
                    return HandleNonColumnInput(key);
                }

                column = column - 1;

            }
            else
            {
                column = strategy2.Run(board, numberToWin, PlayerTurn);
            }

            return EndTurn(logEntry, column);
        }

        public bool PlayAutomated(bool pauseOnTurn)
        {
            var random = new Random();
            var randomChance = random.Next(10);

            GameLogEntry logEntry = NewLogEntry();

            int column = -1;

            var boardSearch = new BoardSearch(numberToWin);
            var legalMoves = boardSearch.FindAllLegalMoves(board);

            if (legalMoves.Count == 0)
            {
                return EndTurn(logEntry, -1,pauseOnTurn);
            }

            if (boardSearch.IsBoardEmpty(board))
            {
            
                //Console.WriteLine("Random move. ");
                column = random.Next(6);
            }
            else
            {
                var strategy = PlayerTurn == 1 ? strategy1 : strategy2;

                if(PlayerTurn == 1 && randomChance <= 5)
                {
                    //Console.WriteLine("Random move. ");
                   
                    column = legalMoves[random.Next(legalMoves.Count - 1)].Col;
                }
                else
                {

                    column = strategy.Run(board, numberToWin, PlayerTurn);
                }
               
            }

            //Console.WriteLine();
            //Console.ForegroundColor = PlayerTurn == 1 ? ConsoleColor.Blue : ConsoleColor.Red;
            //Console.WriteLine($"Player {PlayerTurn} plays {column + 1}.");
            //if (pauseOnTurn)
            //{
            //    Console.ReadKey();
            //}
       
            //Console.ResetColor();

            return EndTurn(logEntry, column,pauseOnTurn,false);
        }

        private bool EndTurn(GameLogEntry logEntry, int column, bool pause = true, bool drawBoard = true)
        {

            if(column == -1)
            {
                if (drawBoard)
                {
                    DrawBoard();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Draw!");
                }
             
                log.Winner = 0;
                if(pause)
                    Console.ReadLine();

                return false;
            }

            if (rules.IsValidMove(column, board))
            {
                board.DropChecker(column, PlayerTurn);
                logEntry.ColumnPlayed = column;
                log.Add(logEntry);

                var winningGroups = rules.PlayerWins(PlayerTurn, board);
                if (winningGroups.Any())
                {
                    if (drawBoard)
                    {
                        DrawBoard();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.BackgroundColor = PlayerTurn == 1 ? ConsoleColor.Blue : ConsoleColor.Red;
                        Console.WriteLine($"Player {PlayerTurn} wins!");
                    }
                    log.Winner = PlayerTurn;
                    if (pause)
                        Console.ReadLine();

                    return false;
                }
            }
            else
            {
                if (drawBoard)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine($"Invalid move. Press any key");
                }

                if (pause)
                    Console.ReadLine();
                
                return true;
            }


            PlayerTurn = PlayerTurn == 1 ? 2 : 1;
            return true;
        }

        private GameLogEntry NewLogEntry()
        {
            var logEntry = new GameLogEntry();
            logEntry.BoardBeforeMove = (int[,])board.Matrix.Clone();
            logEntry.PlayerNumber = PlayerTurn;
            return logEntry;
        }

        private void Prompt(out ConsoleKeyInfo key, out string keyString)
        {
            var promptColor = ConsoleColor.Blue;
            Console.ForegroundColor = promptColor;
            Console.Write($"Select a column Player {PlayerTurn}: ");
            Console.ResetColor();

            key = Console.ReadKey();
            keyString = key.KeyChar.ToString();
        }

        private bool HandleNonColumnInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.S)
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
