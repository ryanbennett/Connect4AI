using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameGenerator
{
    public class ZugzwangDetector
    {
        public bool DoesPlayer1HaveZugzwang(Board board)
        {

            //find threats, label even or odd
            var boardSearch = new BoardSearch(4);

            var groups = boardSearch.FindGroups(1, board, true);
            var hasOddThreat = groups.Select(g => g.Coords.Any(t => t.Row % 2 != 0)).Any();

            return hasOddThreat;


        }

     
    }

   
}
