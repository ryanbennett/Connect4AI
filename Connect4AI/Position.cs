using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4AI
{
    public class Position
    {

        public Position(int row, int col, int mark)
        {
            Row = row;
            Col = col;
            Mark = mark;
        }

        public int Row { get;  }

        public int Col { get;  }

        public int Mark { get; set; }

        public bool Equals(Position pos)
        {
            return pos.Row == Row && pos.Col == Col;
        }
    }
}
