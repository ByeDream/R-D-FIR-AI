using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class DEF
    {
        public const int ROW_LEN = 15;
        public const int COL_LEN = 15;

        public const int MAX_ROW_ID = 14;// ROW_LEN - 1;
        public const int MAX_COL_ID = 14;// COL_LEN - 1;

        public const int WIN_COUNT = 5;

        //没有棋子
        public const byte CHESS_NONE = 0;
        //放了黑棋
        public const byte CHESS_BLACK = 1;
        //放了白棋
        public const byte CHESS_WHITE = 2;
    }

    //一串棋子的状态
    public enum chessCountType
    {
        BothDead        = 0, //小头（左上方）大头(右下方)都被堵死
        MinDeadMaxLive  = 1, //小头（左上方）堵死，大头(右下方)活着
        MinLiveMaxDead  = 2, //小头（左上方）活着，大头(右下方)堵死
        BothLive        = 4, //大头小头都活着
    }

    class Position
    {
        public Position(int row, int col)
        {
            _row = row > DEF.MAX_ROW_ID ? DEF.MAX_ROW_ID : row ;
            _row = _row < 0 ?  0 : _row;

            _col = col > DEF.MAX_COL_ID ? DEF.MAX_COL_ID : col;
            _col = _col < 0 ? 0 : _col;
        }
        
        private int _row;
        public int row
        {
            get { return _row; }
            set
            {
                _row = value > DEF.MAX_ROW_ID ? DEF.MAX_ROW_ID : value;
                _row = _row < 0 ? 0 : _row;
            }
        }
        private int _col;
        public int col
        {
            get { return _col; }
            set
            {
                _col = value > DEF.MAX_COL_ID ? DEF.MAX_COL_ID : value;
                _col = _col < 0 ? 0 : _col;
            }
        }
    }


    class ChessBoard
    {
        public ChessBoard()
        {
            _data = new byte[DEF.ROW_LEN][];
            for( int i = 0; i < DEF.ROW_LEN; i++)
            {
                _data[i] = new byte[DEF.COL_LEN];
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
