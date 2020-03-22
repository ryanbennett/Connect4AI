using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect4AI
{
    public class Rules
    {
        public Rules(int numToWin)
        {
            NumToWin = numToWin;
        }

        public int NumToWin { get; }

        public bool IsValidMove(int columnNumber, Board board)
        {
            //column is out of bounds
            if (columnNumber < 0 || columnNumber > board.MAX_COL_INDEX) return false;

            //column is filled
            if (board[board.MAX_ROW_INDEX,columnNumber] != 0) return false;

            return true;
        }

        public List<Group> PlayerWins(int player, Board board)
        {

            var boardSearch = new BoardSearch(NumToWin);

            var results = boardSearch.FindGroups(player, board, false);
          
            return results;
        }


    }
}
