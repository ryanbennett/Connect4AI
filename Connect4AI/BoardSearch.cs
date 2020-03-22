using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect4AI
{
    public class BoardSearch
    {
        public int NumToWin { get; }

        public BoardSearch(int numToWin)
        {
            NumToWin = numToWin;
        }

        public List<Group> FindGroups(int player, Board board, bool searchForThreats)
        {

            var result = VerticalSearch(player, board, searchForThreats);

            //horizontal search
            result.AddRange(HorizontalSearch(player, board, searchForThreats));

            result.AddRange(RightHandDiagonalSearch(player, board, searchForThreats));

            //left hand diagonal
            result.AddRange(LeftHandDiagonalSearch(player, board, searchForThreats));


            return result;

        }

        public List<int> FindPossibleClaimEvens(Board board)
        {
            var claimEvenPlays = new List<int>();

            //Required: Two squares, directly above each other. Both squares should be empty. The upper square must be even
            var legalMoves = FindAllLegalMoves(board);

            foreach(var move in legalMoves)
            {
                if(move.Col % 2 != 0 && move.Row + 1 <= board.MAX_ROW_INDEX) //legal move is odd row and has at least one more play above it
                {
                    claimEvenPlays.Add(move.Col);
                }
            }

            return claimEvenPlays;
        }


        public List<Position> FindAllLegalMoves(Board board)
        {
            var results = new List<Position>();

            for(var row =0; row<board.MAX_ROW_INDEX; row++)
            {
                var rowArray = board.GetRow(row);

                for(var col=0; col<board.MAX_COL_INDEX; col++)
                {
                    var r = rowArray[col];
                    if( r == 0 && !results.Any(t => t.Col == col))
                    {
                        results.Add(new Position(row, col,r));
                    }
                }
            }

            return results;
        }

        private List<Group> HorizontalSearch(int player, Board board, bool searchForThreats)
        {
            var results = new List<Group>();
            for (int row = 0; row < board.MAX_ROW_INDEX; row++)
            {
                var count = 0;
                var threat = new Group();

                var blanksAllowed = searchForThreats;
                for (int col = 0; col < board.MAX_COL_INDEX; col++)
                {
                    var mark = board[row, col];
                    if(mark == 0 && blanksAllowed)
                    {
                        threat.Coords[count] = new Position(row, col, mark);
                        blanksAllowed = false;
                        count++;
                    }
                    else if (mark == player)
                    {
                        threat.Coords[count] = new Position(row, col, mark);  
                        count++;
                        
                    }
                    else
                    {
                        count = 0;
                        threat = new Group();
                    }

                    if (count == NumToWin)
                    {
                        results.Add(threat);
                    }
                }
            }

            return results;
        }

        private List<Group> VerticalSearch(int player, Board board, bool searchForThreats)
        {
             var results = new List<Group>();
            for (int col = 0; col < board.MAX_COL_INDEX; col++)
            {
                var count = 0;
                var group = new Group();

                var blanksAllowed = searchForThreats;
                for (int row = 0; row < board.MAX_ROW_INDEX; row++)
                {
                    var mark = board[row, col];
                    if (mark == 0 && blanksAllowed)
                    {
                        group.Coords[count] = new Position(row, col, mark);
                        blanksAllowed = false;
                        count++;
                    }
                    else if (mark == player)
                    {
                        group.Coords[count] = new Position(row, col, mark);
                        count++;

                    }
                    else
                    {
                        count = 0;
                        group = new Group();
                    }

                    if (count == NumToWin)
                    {
                        results.Add(group);
                    }
                }
            }

            return results;
        }

        private List<Group> RightHandDiagonalSearch(int player, Board board, bool searchForThreats)
        {
            var results = new List<Group>();
            for (int col = 0; col <= board.MAX_COL_INDEX; col++)
            {
                var count = 0;
                var group = new Group();
                var blanksAllowed = searchForThreats;
                for (int row = 0; row <= board.MAX_ROW_INDEX; row++)
                {
                    var mark = board[row, col];
                    if (mark == player || (mark == 0 && blanksAllowed))
                    {
                        if (mark == 0) blanksAllowed = false;

                        group.Coords[count] = new Position(row,col,mark);
                        count++;

                        var tempRow = row;
                        var tempCol = col;

                        //walk down and to the right
                        for (int c = 0; c < NumToWin; c++)
                        {
                            tempRow -= 1;
                            tempCol += 1;

                            if (tempCol > board.MAX_COL_INDEX || tempRow < 0)
                            {
                                count = 0;
                                group = new Group();
                                break;
                            }

                            if (board[tempRow, tempCol] == player)
                            {
                                group.Coords[count] = new Position(row, col, mark);
                                count++;
                            }

                            if (count == NumToWin)
                            {
                                results.Add(group);
                            }
                        }


                    }
                    else
                    {
                        count = 0;
                        group = new Group();
                    }

                    if (count == NumToWin)
                    {
                        results.Add(group);
                    }
                }
            }

            return results;
        }

   

        private List<Group> LeftHandDiagonalSearch(int player, Board board, bool searchForThreats)
        {
            var results = new List<Group>();
            for (int col = board.MAX_COL_INDEX; col >= 0; col--)
            {
                var count = 0;
                var group = new Group();
                var blanksAllowed = searchForThreats;
                for (int row = 0; row <= board.MAX_ROW_INDEX; row++)
                {
                    var mark = board[row, col];
                    if (mark == player || (mark == 0 && blanksAllowed))
                    {
                        if (mark == 0) blanksAllowed = false;

                        group.Coords[count] = new Position(row, col, mark);
                        count++;

                        var tempRow = row;
                        var tempCol = col;
                        //walk down and to the right
                        for (int c = 0; c < NumToWin; c++)
                        {
                            tempRow -= 1;
                            tempCol -= 1;

                            if (tempCol < 0 || tempRow < 0)
                            {
                                count = 0;
                                group = new Group();
                                break;
                            }

                            if (board[tempRow, tempCol] == player)
                            {
                                group.Coords[count] = new Position(row, col, mark);
                                count++;
                            }

                            if (count == NumToWin)
                            {
                                results.Add(group);
                            }
                        }
                    }
                    else
                    {
                        count = 0;
                    }

                    if (count == NumToWin)
                    {
                        results.Add(group);
                    }
                }
            }

            return results;
        }
    }
}
