using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connect4AI;
using MinMaxSearch;

namespace GameGenerator
{
    public class RandomStrategy : IPlayerStrategy
    {
        public int Run(Board board, int numToWin, int playerNumber, int searchDepth =8)
        {
            Random rand = new Random();

            var test = rand.Next(0, 2);

            if(test == 0)
            {
                var search = new BoardSearch(numToWin);
                var legalMoves = search.FindAllLegalMoves(board);

                var allColumns = legalMoves.Select(p => p.Col).ToList();

                if (!allColumns.Any())
                {
                    return -1;
                }


                var index = rand.Next(allColumns.Count() - 1);
                return allColumns[index];
            }
            else
            {
                var strat = new MinMaxStrategy();
                return strat.Run(board, numToWin, playerNumber);
            }
          

        }
    }
}
