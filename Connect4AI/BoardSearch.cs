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

        public bool IsBoardEmpty(Board board)
        {
            var result = true;
            for(var c =0;c<board.MAX_COL_INDEX+1; c++)
            {
                var col = board.GetColumn(c);
                result = col.All(m => m == 0);
                if (!result) return result;
            }

            return result;
        }

        public List<Group> FindGroups(int player, Board board, int blanksAllowed = 0)
        {

            var result = VerticalSearch(player, board, blanksAllowed);

            //horizontal search
            result.AddRange(HorizontalSearch(player, board, blanksAllowed));

            result.AddRange(RightHandDiagonalSearch(player, board, blanksAllowed));

            //left hand diagonal
            result.AddRange(LeftHandDiagonalSearch(player, board, blanksAllowed));


            return result;

        }

        public List<int> FindPossibleClaimEvens(Board board)
        {
            var claimEvenPlays = new List<int>();

            //Required: Two squares, directly above each other. Both squares should be empty. The upper square must be even
            var legalMoves = FindAllLegalMoves(board);

            foreach(var move in legalMoves)
            {
                if(move.Col % 2 != 0 && move.Row <= board.MAX_ROW_INDEX) //legal move is odd row and has at least one more play above it
                {
                    claimEvenPlays.Add(move.Col);
                }
            }

            return claimEvenPlays;
        }


        public List<Position> FindAllLegalMoves(Board board)
        {
            var results = new List<Position>();

            for(var row =0; row<=board.MAX_ROW_INDEX; row++)
            {
                var rowArray = board.GetRow(row);

                for(var col=0; col<=board.MAX_COL_INDEX; col++)
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


        private List<Group> HorizontalSearch(int player, Board board, int blanksAllowed = 0)
        {
            var results = new List<Group>();
            for (int row = 0; row <= board.MAX_ROW_INDEX; row++)
            {
                var count = 0;
                var threat = new Group();

                var blankCount = 0;
                for (int col = 0; col <= board.MAX_COL_INDEX; col++)
                {
                    var mark = board[row, col];
                    if(CanCountBlank(blanksAllowed, blankCount, mark))
                    {
                        threat.Coords.Add( new Position(row, col, mark));
                        blankCount++;
                        count++;
                    }
                    else if (mark == player)
                    {
                        threat.Coords.Add(new Position(row, col, mark));  
                        count++;
                    }
                    else
                    {
                        count = 0;
                        blankCount = 0;
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

        private List<Group> VerticalSearch(int player, Board board, int blanksAllowed)
        {
             var results = new List<Group>();
            for (int col = 0; col <= board.MAX_COL_INDEX; col++)
            {
                var count = 0;
                var group = new Group();

                var blankCount = 0;
                for (int row = 0; row <= board.MAX_ROW_INDEX; row++)
                {
                    var mark = board[row, col];
                    if (CanCountBlank(blanksAllowed, blankCount, mark))
                    {
                        group.Coords.Add(new Position(row, col, mark));
                        blankCount++;
                        count++;
                    }
                    else if (mark == player)
                    {
                        group.Coords.Add(new Position(row, col, mark));
                        count++;
                    }
                    else
                    {
                        count = 0;
                        blankCount = 0;
                        group = new Group();
                    }

                    if (count == NumToWin)
                    {
                        results.Add(group);

                        break;
                    }
                }
            }

            return results;
        }

        private static bool CanCountBlank(int blanksAllowed, int blankCount, int mark)
        {
            return blanksAllowed > 0 && mark == 0 && blankCount < blanksAllowed;
        }

        private List<Group> RightHandDiagonalSearch(int player, Board board, int blanksAllowed)
        {
            var results = new List<Group>();
            for (int col = 0; col <= board.MAX_COL_INDEX; col++)
            {
                var count = 0;
                var group = new Group();

                var blankCount = 0;
                for (int row = board.MAX_ROW_INDEX; row >=0 ; row--)
                {
                    var mark = board[row, col];
                    if (mark == player || CanCountBlank(blanksAllowed, blankCount, mark))
                    {
                        if (blanksAllowed > 0 && mark == 0) blankCount++;

                        group.Coords.Add(new Position(row,col,mark));
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
                                blankCount = 0;
                                group = new Group();
                                break;
                            }

                            var tempMark = board[tempRow, tempCol];
                            if (tempMark == player || CanCountBlank(blanksAllowed, blankCount, tempMark))
                            {
                                group.Coords.Add(new Position(tempRow, tempCol, tempMark));
                                count++;

                                if(tempMark == 0)
                                {
                                    blankCount++;
                                }
                            }
                            else
                            {
                                blankCount = 0 ;
                                group = new Group();
                                count = 0;
                                break;

                            }

                            if (count == NumToWin)
                            {
                                results.Add(group);
                                blankCount = 0;
                                group = new Group();
                                count = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        count = 0;
                        blankCount = 0;
                        group = new Group();
                    }

                }
            }

            return results;
        }

   

        private List<Group> LeftHandDiagonalSearch(int player, Board board, int blanksAllowed)
        {
            var results = new List<Group>();
            for (int col = board.MAX_COL_INDEX; col >= 0; col--)
            {
                var count = 0;
                var group = new Group();
                var blankCount = 0;
                for (int row = board.MAX_ROW_INDEX; row >= 0 ; row--)
                {
                    var mark = board[row, col];
                    if (mark == player || CanCountBlank(blanksAllowed, blankCount, mark))
                    {
                        if (mark == 0) blankCount++;

                        group.Coords.Add(new Position(row, col, mark));
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
                                blankCount = 0;
                                group = new Group();
                                break;
                            }

                            var tempMark = board[tempRow, tempCol];
                            if (tempMark == player || CanCountBlank(blanksAllowed, blankCount, tempMark))
                            {
                                group.Coords.Add(new Position(tempRow, tempCol, tempMark));
                                count++;

                                if(tempMark == 0)
                                {
                                    blankCount++;
                                }
                            }
                            else
                            {
                                count = 0;
                                blankCount = 0;
                                group = new Group();
                                break;
                            }
                            
                            if (count == NumToWin)
                            {
                                results.Add(group);
                                blankCount = 0 ;
                                group = new Group();
                                count = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        count = 0;
                        blankCount = 0;
                        group = new Group();
                    }

                   
                }
            }

            return results;
        }
    }
}
