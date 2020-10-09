
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Connect4AI.Core;
using GameGenerator;

namespace Connect4AI.WebJob
{
    public class WebJobGame
    {

        private Board board;
        private Rules rules;

        public IGameLog log;

        public WebJobGame(IGameLog log, int numberOfColumns = 7, int numberOfRows = 6, int numberToWin = 4 )
        {
            this.board = new Board(numberOfColumns, numberOfRows);
            this.rules = new Rules(numberToWin);
            this.numberToWin = numberToWin;
            this.log = log;
        }

        IPlayerStrategy strategy1 = new RandomStrategy();
        MinMaxStrategy strategy2 = new MinMaxStrategy();

        private const string LeftSideBuffer = "          ";
        private readonly int numberToWin;
        private int PlayerTurn = 1;

        private string filePath;



        public int AutoTurns = 0;
        public int Player1SearchDepth = 2;
        public bool PlayAutomated(bool pauseOnTurn)
        {
            AutoTurns++;
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

                var depth = PlayerTurn == 1 ? Player1SearchDepth : GetSearchDepth(AutoTurns);


                column = strategy.Run(board, numberToWin, PlayerTurn, depth);

            }

            return EndTurn(logEntry, column,pauseOnTurn,true);
        }

        private int GetSearchDepth(int autoTurns)
        {
            if (autoTurns < 10) return 4;

            var x = autoTurns;

            var depth = Math.Pow(-(.135 * x - 2.8), 2) + 8;
            var depthInt = depth > 8 ? 8 : (int)Math.Ceiling(depth);
            depthInt = depth < 2 ? 2 : depthInt;
            return depthInt;
        }

        private bool EndTurn(GameLogEntry logEntry, int column, bool pause = true, bool drawBoard = true)
        {

            if(column == -1)
            {

                log.Winner = 0;
   
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

                    log.Winner = PlayerTurn;

                    return false;
                }

            }
            else
            {
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

 
    
    }
}
