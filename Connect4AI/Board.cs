﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect4AI
{
    public class Board
    {

        public string Name { get; set; }

        public Board()
        {

        }

        public Board(int cols = 7, int rows = 6)
        {
            _matrix =  new int[rows,cols];
        }

        private int[,] _matrix;
        public int MAX_ROW_INDEX { get { return _matrix.GetLength(0) - 1; } }
        public int MAX_COL_INDEX { get { return _matrix.GetLength(1) - 1; } }

        public int[,] Matrix { get => _matrix; set => _matrix = value; }

        public int this[int r, int c]
        {
            get => _matrix[r,c];
            set => _matrix[r,c] = value;
        }

        public int[] GetColumn(int columnNumber)
        {
            return Enumerable.Range(0, _matrix.GetLength(0))
                    .Select(x => _matrix[x, columnNumber])
                    .ToArray();
        }

        public int[] GetRow(int rowNumber)
        {
            return Enumerable.Range(0, _matrix.GetLength(1))
                    .Select(x => _matrix[rowNumber, x])
                    .ToArray();
        }
    
        public void DropChecker(int column, int player)
        {
            var col = this.GetColumn(column);
            for(var row=0; row<=MAX_ROW_INDEX; row++)
            {
                if(col[row] == 0)
                {
                    _matrix[row, column] = player;
                    return;
                }
            }
        }

    }
}
