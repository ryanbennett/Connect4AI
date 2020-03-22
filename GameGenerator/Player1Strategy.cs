using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameGenerator
{
    public class Player2Strategy
    {

        private const int PLAYERNUM = 2;
        public int Run(Board board, int numToWin)
        {

            var search = new BoardSearch(numToWin);

            var legalMoves = search.FindAllLegalMoves(board);

            var yourThreats = search.FindGroups(PLAYERNUM, board, true);
            var oppThreats = search.FindGroups(1, board, true);

            int? columnToPlay = 0;

            //can you win?
            columnToPlay = MatchThreatsWithLegal(yourThreats, legalMoves);
            if (columnToPlay.HasValue) return columnToPlay.Value;

            //should you block?
            columnToPlay = MatchThreatsWithLegal(oppThreats, legalMoves);
            if (columnToPlay.HasValue) return columnToPlay.Value;

            
            //claimeven


            //lowinverse




        }

        private int? MatchThreatsWithLegal(List<Group> threats, List<Position> legalMoves)
        {
            foreach(var threat in threats)
            {
                var blank = threat.Coords.First(t => t.Mark == 0);
                foreach (var move in legalMoves)
                {
                    if (blank.Row == move.Row && blank.Col == move.Col)
                    {
                        //this is a win.
                        return move.Col;
                    }
                }
            }

            return null;
            
        }
    }
}
