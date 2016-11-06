using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class ChessBoard
    {
        public ChessBoard()
        {
            //_data = new int[Side.ROW][];
            //for( int i = 0; i < Side.ROW; i++)
            //{
            //    _data[i] = new int[Side.COL];
            //}

            _data = new int[][]{
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                 };
        }

        private int[][] _data = null;

        public int[][] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public void setData(int row, int col, int value)
        {
            _data[row][col] = value;
        }

        public void setData(Position p, int value)
        {
            _data[p.row][p.col] = value;
        }

    }
}
