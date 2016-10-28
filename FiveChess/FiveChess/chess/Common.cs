using System;
using System.Collections;
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

    public class Direction
    {
        public const int NONE = 0;
        public const int Horizontal = 1;
        public const int Vertical = 2;
        public const int LetfTopToRightBottom = 3;
        public const int LetfBottomToRightTop = 4;
    }

    //一串棋子的状态
    public enum LineType
    {
        BothDead = 0, //小头（左上方）大头(右下方)都被堵死
        MinDeadMaxLive = 1, //小头（左上方）堵死，大头(右下方)活着
        MinLiveMaxDead = 2, //小头（左上方）活着，大头(右下方)堵死
        BothLive = 4, //大头小头都活着
    }

    public enum WinState
    {
        //胜负未分
        GAMING = 0,
        //和局
        DRAW = 1,
        //黑胜
        BLACK_WIN = 100000,
        //白胜
        WHITE_WIN = 100001,
    }

    //棋型的价值
    public class Cost
    {
        public const int One_1_1 = 50;      //独1 一头活
        public const int One_1_2 = 100;     //独1 两头活

        public const int Tow_2_1 = 180;     //活2, 一头活
        public const int Tow_2_2 = 400;     //活2, 两头活

        public const int Three_3_1a = 410;   //活3, 一头活
        public const int Three_3_2a = 800;   //活3, 两头活
        public const int Three_111_0a = 200;   //两头被堵死的断三
        public const int Three_111_1a = 410;   //一头被堵死的断三
        public const int Three_111_2a = 500;   //两头活的断三

        public const int Four_211_0a = 200;   //两头被堵死的断四
        public const int Four_211_1a = 350;   //一头被堵死的断四, 一活着
        public const int Four_211_2a = 410;   //一头被堵死的断四, 二活着
        public const int Four_211_12a = 800;   //一头被堵死的断四, 两头都活着
        public const int Four_4_1a = 810;    //活4, 一头活
        public const int Four_4_2a = 2000;   //=200, 活4，两头活

        public const int Fiv_212_0a = 200;   //两头被堵死的断五
        public const int Fiv_212_1a = 200;   //两头被堵死的断五
        public const int Fiv_221_0a = 200;   //两头被堵死的断五
        public const int Fiv_113_0a = 810;   //两头被堵死的断五
        public const int Fiv_131_0a = 2000;   //两头被堵死的断五
        public const int Fiv_5_2_1_a = 410;   //一头被堵死的断四, 二活着
        public const int Fiv_5_2_2_a = 800;   //一头被堵死的断四, 两头都活着
        public const int Five = 100000;     //=5,胜利

        public const int One_add_1 = 50;                            //1+1 一头活, 下了堵死的那头
        public const int One_add_1_a = 150;                         //1+1 一头活, 下了活的的那头
        public const int One_add_1_b = Tow_2_2 - One_1_2;           //1+1, 两头活,下了活的的那头
        public const int Tow_add_1 = 150;                           //2+1, 一头活, 下了堵死的那头
        public const int Tow_add_1_a = 200;                         //2+1, 一头活, 下了活的的那头
        public const int Two_add_1_b = Three_3_2a - Tow_2_2;         //2+1, 两头活,下了活的的那头
        public const int Tree_add_1 = 350;                          //3+1, 一头活, 下了堵死的那头
        public const int Tree_add_1_a = 380;                        //3+1, 一头活, 下了活的的那头
        public const int Tree_add_1_b = Four_4_2a - Three_3_2a;       //3+1, 两头活,下了活的的那头

        //Three_12_1,       //12断3, 一头活
        //Three_21_1,       //21断3, 一头活
        //Three_21_2,       //21断3, 两头活
        //Four_22_1,        //22断4, 一头活
        //Four_13_1,        //13断4, 一头活
        //Four_31_1,        //31断4, 两头活
        //Four_22_2,        //22断4, 两头活
        //Four_31_2,        //31断4, 两头活
    }

    public enum DropType
    {
        CANT_NOT    = 0,
        CANT_DROP   = 1,
        Better      = 2,
    }

    public class Position
    {
        public Position(int row = 0, int col = 0, int color = 0, int value = 0)
        {
            _row = row > Side.ROW_ID ? Side.ROW_ID : row;
            _row = _row < 0 ? 0 : _row;

            _col = col > Side.COL_ID ? Side.COL_ID : col;
            _col = _col < 0 ? 0 : _col;
            _color = color;
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
            set { _value = value; }
        }
        private int _color;
        public int color
        {
            get { return _color; }
            set { _color = value; }
        }
        private int _winValue;
        public int win
        {
            get { return _winValue; }
            set { _winValue = value; }
        }
        private int _depth;
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        private bool _alone = false;
        public bool alone
        {
            get { return _alone; }
            set { _alone = value; }
        }

        private int _haveWin = -1;
        public int haveWin
        {
            get { return _haveWin; }
            set { _haveWin = value; }
        }

        private int _haveFail = -1;
        public int haveFail
        {
            get { return _haveFail; }
            set { _haveFail = value; }
        }

        public void reset(Position pos)
        {
            reset(pos._row, pos._col, pos._color, pos._value, pos.win, pos.depth, pos._haveFail, pos.haveWin, pos.alone);
        }
        public void reset(int row = 0, int col = 0, int color = 0, int value = 0, int winValue = 0,
            int depth = 0, int haveFaile = -1, int haveWin = -1, bool alone = false)
        {
            _row = row > Side.ROW_ID ? Side.ROW_ID : row;
            _row = _row < 0 ? 0 : _row;

            _col = col > Side.COL_ID ? Side.COL_ID : col;
            _col = _col < 0 ? 0 : _col;
            _color = color;
            _value = value;
            _winValue = winValue;
            _depth = depth;
            _haveFail = haveFaile;
            _haveWin = haveWin;
            _alone = alone;
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

        //public int Compare(object x, object y)
        //{

        //    if (((Position)x).val >= ((Position)y).val)
        //        return 1;
        //    else
        //        return 0;
        //}
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
            clear();
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
            minSpace = 0;
            maxSpace = 0;
            direction = Direction.NONE;
        }

        public Position p1;

        public Position p2;

        public int length = 0;

        public LineType type = LineType.BothLive;

        public int direction = Direction.NONE;

        public int cost = 0;

        public int color;

        public int minSpace = 0;

        public int maxSpace = 0;
    }
}
