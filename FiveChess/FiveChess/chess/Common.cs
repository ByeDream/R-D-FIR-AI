using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class Con
    {
        public const int WIN_COUNT = 5;
    }

    public class Color
    {
        //没有棋子
        public const int NONE = 0;
        //放了黑棋
        public const int BLACK = 1;
        //放了白棋
        public const int WHITE = 2;
    }

    public class Side
    {
        public const int ROW = 15;
        public const int COL = 15;

        public const int ROW_ID = 14;// ROW - 1;
        public const int COL_ID = 14;// COL - 1;
    }

    //一串棋子的状态
    public enum LineType
    {
        BothDead = 0, //小头（左上方）大头(右下方)都被堵死
        MinDeadMaxLive = 1, //小头（左上方）堵死，大头(右下方)活着
        MinLiveMaxDead = 2, //小头（左上方）活着，大头(右下方)堵死
        BothLive = 4, //大头小头都活着
    }

    //棋型的价值
    public enum Cost
    {
        One_1_1     = 10,   //独1 一头活
        One_1_2     = 20,   //独1 两头活
        Tow_2_1     = 25,   //活2, 一头活
        Three_3_1   = 40,   //活3, 一头活
        Tow_2_2     = 45,   //活2, 两头活
        //Three_12_1,       //12断3, 一头活
        //Three_21_1,       //21断3, 一头活
        //Three_21_2,       //21断3, 两头活
        //Four_22_1,        //22断4, 一头活
        //Four_13_1,        //13断4, 一头活
        Four_4_1    = 250,   //活4, 一头活
        Three_3_2   = 300,   //活3, 两头活
        //Four_31_1,        //31断4, 两头活
        //Four_22_2,        //22断4, 两头活
        //Four_31_2,        //31断4, 两头活

        Four_4_2    = 500,  //=200, 活4，两头活
        Five        = 10000,  //=5,胜利
    }

    public enum DropType
    {
        CANT_NOT    = 0,
        CANT_DROP   = 1,
        Better      = 2,
    }

    public class Position
    {
        public Position(int row = 0, int col = 0, int value = 0)
        {
            _row = row > Side.ROW_ID ? Side.ROW_ID : row;
            _row = _row < 0 ? 0 : _row;

            _col = col > Side.COL_ID ? Side.COL_ID : col;
            _col = _col < 0 ? 0 : _col;
            _value = value;
        }

        private int _row;
        public int row
        {
            get { return _row; }
            set
            {
                _row = value > Side.ROW_ID ? Side.ROW_ID : value;
                _row = _row < 0 ? 0 : _row;
            }
        }
        private int _col;
        public int col
        {
            get { return _col; }
            set
            {
                _col = value > Side.COL_ID ? Side.COL_ID : value;
                _col = _col < 0 ? 0 : _col;
            }
        }
        private int _value;
        public int val
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
        private int _color;
        public int color
        {
            get { return _color; }
            set
            {
                _color = value;
            }
        }
        public static void sort(Position[] pos)
        {
            Position tmp = null;
            for(int i = 0; i < pos.Length; i++)
            {
                for(int j = i; j < pos.Length; j++)
                {
                    if(pos[j].val > pos[i].val)
                    {
                        tmp = pos[j];
                        pos[j] = pos[i];
                        pos[i] = tmp;
                    }
                }
            }
        }
    }

    /// <summary>
    ///     /// 0,0,0,0
    ///     /// 0,0,0,0
    ///     /// p1,0,0,p2
    ///     /// 0,0,0,0
    /// ----------------
    ///     /// 0,p1,0,0
    ///     /// 0,0,0,0
    ///     /// 0,0,0,0
    ///     /// 0,p2,0,0
    /// ---------------
    ///     /// p1,0,0,0
    ///     /// 0,0,0,0
    ///     /// 0,0,0,0
    ///     /// 0,0,0,p2
    /// ---------------
    ///     /// 0,0,0,p2
    ///     /// 0,0,0,0
    ///     /// 0,0,0,0
    ///     /// p1,0,0,0
    /// </summary>
    public  class Line
    {
        public Line()
        {
            this.p1 = new Position(0, 0);
            this.p2 = new Position(0, 0);
        }

        public Line(int col1, int row1, int col2, int row2, int color)
        {
            this.p1 = new Position(0, 0);
            this.p2 = new Position(0, 0);
            reset(col1, row1, col2, row2, color);
        }

        public Line(Position p1, Position p2, int color)
        {
            this.p1 = new Position(0, 0);
            this.p2 = new Position(0, 0);

            reset(p1.col,p1.row,p2.col,p2.row,color);
        }

        public void reset(int col1, int row1, int col2, int row2, int color)
        {
            if (col1 == col2)
            {
                if (row2 >= row1)
                {
                    p1.row = row1;
                    p1.col = col1;
                    p2.row = row2;
                    p2.col = col2;
                }
                else
                {
                    p1.row = row2;
                    p1.col = col2;
                    p2.row = row1;
                    p2.col = col1;
                }
            }
            else if (col2 > col1)
            {
                p1.row = row2;
                p1.col = col2;
                p2.row = row1;
                p2.col = col1;
            }
            else
            {
                p1.row = row1;
                p1.col = col1;
                p2.row = row2;
                p2.col = col2;
            }
            int len1 = Math.Abs(p1.row - p2.row) + 1;
            int len2 = Math.Abs(p1.col - p2.col) + 1;

            length = len1 > len2 ? len1 : len2;
            type = LineType.BothLive;
            this.color = color;
        }

        public void clear()
        {
            p1.col = p1.row = 0;
            p2.row = p2.col = 0;
            length = 0;
            cost = 0;
            type = LineType.BothLive;
            color = Color.NONE;
        }

        public Position p1;

        public Position p2;

        public int length = 0;

        public LineType type = LineType.BothLive;

        public int cost = 0;

        public int color;
    }
}
