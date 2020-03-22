using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameGenerator
{
    public class Player2Strategy
    {

        private Random random = new Random();

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
            var claimevens = search.FindPossibleClaimEvens(board);
            foreach(var claimEven in claimevens)
            {
                foreach(var move in legalMoves)
                {
                    if (claimEven == move.Col)
                    {
                        return claimEven;
                    }
                }
            }

            var moveIndex = random.Next(0, legalMoves.Count());
            return legalMoves[moveIndex].Col;

        }

        

        private int? MatchThreatsWithLegal(List<Group> threats, List<Position> legalMoves)
        {
            foreach(var threat in threats)
            {
                var blank = threat.Coords.First(t => t.Mark == 0);
                foreach (var move in legalMoves)
                {
                    if (blank.Equals(move))
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
