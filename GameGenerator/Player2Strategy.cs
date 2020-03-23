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

            var yourThreats = search.FindGroups(PLAYERNUM, board, 1);
            var oppThreats = search.FindGroups(1, board, 1);

            int? columnToPlay = 0;

            //can you win?
            columnToPlay = MatchThreatsWithLegal(yourThreats, legalMoves);
            if (columnToPlay.HasValue) return columnToPlay.Value;

            //should you block?
            columnToPlay = MatchThreatsWithLegal(oppThreats, legalMoves);
            if (columnToPlay.HasValue) return columnToPlay.Value;

            //are there hidden forks for player 1?
            //look for threats with 2 blanks
            var forkThreats = search.FindGroups(1, board, 2);
            //do any threats share a blank?
            var forkPositions = new List<Position>();
            for(var i = 0; i < forkThreats.Count; i++)
            {
                var ft = forkThreats[i];
                var blanks = ft.Coords.Where(c => c.Mark == 0);

                for(var j =0; j<forkThreats.Count; j++)
                {
                    if (j == i) continue;

                    var jft = forkThreats[j];
                    var jBlanks = jft.Coords.Where(c => c.Mark == 0);

                    var shared = jBlanks.Intersect<Position>(blanks, new PositionEqualityComparer());
                    forkPositions.AddRange(shared);
                }
            }

            //are any blanks legal moves?
            columnToPlay = MatchPositionsThreatsWithLegal(forkPositions, legalMoves);

            //can you create a fork?
            //look for threats with 2 blanks
            //do any threats share a blank?
            //can it be played?
            //should we remove a legal move if we can to force player 1 to play the step up?

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

        private int? MatchPositionsThreatsWithLegal(List<Position>  positions, List<Position> legalMoves)
        {
            foreach (var position in positions)
            {
                foreach (var move in legalMoves)
                {
                    if (move.Equals(position))
                    {
                        //this is a win.
                        return move.Col;
                    }
                }
            }

            return null;

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
