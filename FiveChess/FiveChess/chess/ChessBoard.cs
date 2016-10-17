using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class ChessBoard
    {
        public ChessBoard()
        {
            _data = new byte[Side.ROW][];
            for( int i = 0; i < Side.ROW; i++)
            {
                _data[i] = new byte[Side.COL];
            }
        }

        private byte[][] _data = null;

        public byte[][] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public void setData(int row, int col, byte value)
        {
            _data[row][col] = value;
        }

        public void setData(Position p, byte value)
        {
            _data[p.row][p.col] = value;
        }

    }
}
