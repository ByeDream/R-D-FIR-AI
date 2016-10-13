using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Position
    {
        public Position(int row, int col)
        {
            _row = row > 14 ? 14 : row ;
            _row = _row < 0 ?  0 : _row;

            _col = col > 14 ? 14 : col;
            _col = _col < 0 ? 0 : _col;
        }
        
        private int _row;
        public int row
        {
            get { return _row; }
            set
            {
                _row = value > 14 ? 14 : value;
                _row = _row < 0 ? 0 : _row;
            }
        }
        private int _col;
        public int col
        {
            get { return _col; }
            set
            {
                _col = value > 14 ? 14 : value;
                _col = _col < 0 ? 0 : _col;
            }
        }
    }


    class ChessBoard
    {
        //没有棋子
        public const byte CHESS_NONE = 0;
        //放了黑棋
        public const byte CHESS_BLACK = 1;
        //放了白棋
        public const byte CHESS_WHITE = 2;

        public ChessBoard()
        {
            _data = new byte[15][];
            for( int i = 0; i < 15; i++)
            {
                _data[i] = new byte[15];
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
