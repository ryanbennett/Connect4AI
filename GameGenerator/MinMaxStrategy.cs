using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connect4AI;
using GameGenerator.MinMax;
using MinMaxSearch;
using MinMaxSearch.Cache;

namespace GameGenerator
{
    public class MinMaxStrategy
    {
        public int Run(Board board, int numToWin, int playerNumber)
        {
            var searchEngine = new SearchEngine()
            {
                MaxDegreeOfParallelism = 1,
                MaxLevelOfParallelism = 1,
                DieEarly = true,
                MinScore = BoardEvaluator.MinEvaluation,
                MaxScore = BoardEvaluator.MaxEvaluation,
                ParallelismMode = ParallelismMode.NonParallelism,
                SkipEvaluationForFirstNodeSingleNeighbor = false,
                CacheMode = CacheMode.NewCache,
                StateDefinesDepth = true
            };

            BoardSearch boardSearch = new BoardSearch(numToWin);
            var legalMoves = boardSearch.FindAllLegalMoves(board);



            var boardToEvaluate = new Board(board.MAX_COL_INDEX, board.MAX_ROW_INDEX);
            int[,] matrix = (int[,])board.Matrix.Clone();
            boardToEvaluate.Matrix = matrix;


            Player[,] boardState = BoardToPlayerBoard(boardToEvaluate, playerNumber);

            Connect4State state = new Connect4State(boardState, (Player)playerNumber);
            var searchResult = searchEngine.Search(state, 7);
            var newBoard = (Connect4State)searchResult.NextMove;

            var columnToPlay = -1;
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
