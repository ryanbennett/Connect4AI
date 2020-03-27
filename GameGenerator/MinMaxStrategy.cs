using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connect4AI;
using GameGenerator.MinMax;
using MinMaxSearch;
using MinMaxSearch.Cache;
using MinMaxSearch.Exceptions;

namespace GameGenerator
{
    public class MinMaxStrategy
    {
        private SearchEngine searchEngine = new SearchEngine()
        {
            MaxDegreeOfParallelism = 4,
            MaxLevelOfParallelism = 2,
            DieEarly = false,
            MinScore = BoardEvaluator.MinEvaluation,
            MaxScore = BoardEvaluator.MaxEvaluation,
            ParallelismMode = ParallelismMode.TotalParallelism,
            SkipEvaluationForFirstNodeSingleNeighbor = false,
            CacheMode = CacheMode.ReuseCache,
            StateDefinesDepth = true
        };

        public int Run(Board board, int numToWin, int playerNumber)
        {
          

            BoardSearch boardSearch = new BoardSearch(numToWin);



            var boardToEvaluate = new Board(board.MAX_COL_INDEX, board.MAX_ROW_INDEX);
            int[,] matrix = (int[,])board.Matrix.Clone();
            boardToEvaluate.Matrix = matrix;


            Player[,] boardState = BoardToPlayerBoard(boardToEvaluate, playerNumber);
            var columnToPlay = -1;
            Connect4State state = new Connect4State(boardState, (Player)playerNumber);
            try
            {
                var searchResult = searchEngine.Search(state, 9);
                var newBoard = (Connect4State)searchResult.NextMove;

                
                for (var r = 0; r <= board.MAX_ROW_INDEX; r++)
                {
                    for (var c = 0; c <= board.MAX_COL_INDEX; c++)
                    {
                        var value = boardState[r, c];
                        var newValue = newBoard.Board[r, c];

                        if (value != newValue)
                        {
                            columnToPlay = c;
                        }
                    }
                }
            }
            catch(NoNeighborsException e)
            {
                var legalMoves = boardSearch.FindAllLegalMoves(boardToEvaluate);
                Random rand = new Random();
                columnToPlay = legalMoves[rand.Next(legalMoves.Count - 1)].Col;
            }

            return columnToPlay;
        }

        private static Player[,] BoardToPlayerBoard(Board board, int playerNumber)
        {
            Player[,] boardState = new Player[board.MAX_ROW_INDEX+1, board.MAX_COL_INDEX+1];
            for (var r = 0; r <= board.MAX_ROW_INDEX; r++)
            {
                for (var c = 0; c <= board.MAX_COL_INDEX; c++)
                {
                    var value = board.Matrix[r, c];

                    //2 represents max player in minmax tree
                    if(playerNumber == 1)
                    {
                        value = value == 1 ? 2 : value == 2 ? 1: 0;
                    }


                    boardState[r, c] = (Player)value;
                }
            }

            return boardState;
        }
    }
      
    
}
