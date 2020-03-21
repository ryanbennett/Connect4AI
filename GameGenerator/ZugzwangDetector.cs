using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameGenerator
{
    public class ZugzwangDetector
    {
        public bool DoesPlayerHaveZugzwang(int player, Board board)
        {

            //find threats, label even or odd
            var boardSearch = new BoardSearch(4);

            var groups = boardSearch.FindGroups(1, board, true);
            var hasOddThreat = groups.Select(g => g.Coords.Any(t => t.Item1 % 2 != 0)).Any();


            if (player == 1)
            {
                return hasOddThreat;
            }
            else
            {
                return !hasOddThreat;
            }

        }

     
    }

   
}
