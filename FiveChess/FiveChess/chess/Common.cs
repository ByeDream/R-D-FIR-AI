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

    public class WinState
    {
        //胜负未分
        public const int GAMING = 0;
        //和局
        public const int DRAW = 1;
        //黑胜
        public const int BLACK_WIN = 100000;
        //白胜
        public const int WHITE_WIN = 100001;
    }

    //棋型的价值
    public class Cost
    {
        public const int One_d1 = 150;      //独1 一头活
        public const int One_1 = 300;     //独1 两头活

        public const int Tow_d11 = 600;     //活2, 一头活
        public const int Tow_11 = 900;     //活2, 两头活
        //public const int Tow_d101 = 400;     //活2, 一头活
        //public const int Tow_101 = 600;     //活2, 两头活

        public const int Three_111 = 1800;   //活3, 两头活
        public const int Three_d111 = 1200;   //活3, 一头活
        //public const int Three_1011d = 450;   //活3, 有一的那头活
        //public const int Three_d1011 = 455;   //活3, 有二的那头活
        //public const int Three_1011 = 700;   //活3, 两头活
        //public const int Three_10011d = 300;
        //public const int Three_d101001 = 200;   //两头被堵死的断三
        //public const int Three_d10101d = 200;   //两头被堵死的断三
        //public const int Three_d10101 = 410;   //一头被堵死的断三
        //public const int Three_10101 = 500;   //两头活的断三

        public const int Four_1111 = 3000;   //活4，两头活
        public const int Four_d1111 = 2400;    //活4, 一头活

        public const int Five_1011101 = 3000;    //三段棋

        //public const int Four_111001d = 850;   //断四, 两头都活着
        //public const int Four_d111001 = 560;   //断四, 两头都活着
        //public const int Four_d110101d = 900;   //两头被堵死的断四
        //public const int Four_d110101 = 1050;   //一头被堵死的断四, 一活着
        //public const int Four_110101d = 1110;   //一头被堵死的断四, 二活着
        //public const int Four_110101 = 1500;   //断四, 两头都活着
        //public const int Four_10111d = 1500;   //断四, 一活着
        //public const int Four_d10111 = 1520;   //断四, 三活着
        //public const int Four_11011d = 1500;   //断四, 一活着
        //public const int Four_11011 = 1650;   //断四, 两头都活着
        //public const int Four_10111 = 1650;   //断四, 一三活着

        //public const int Fiv_d1101011d = 200;   //两头被堵死的断五
        //public const int Fiv_1101011 = 800;   //两头活的断五
        //public const int Fiv_1101101 = 820;   //两头被堵死的断五,相当于冲四
        //public const int Fiv_221_2a = 1000;   //两头活的断五,相当于冲四
        //public const int Fiv_111011 = 2000;   //两头活的断五,相当于冲四
        //public const int Fiv_d111011 = 1800;   //两头活的断五,相当于冲四
        //public const int Fiv_111011d = 1850;   //两头活的断五,相当于冲四
        //public const int Fiv_113_0a = 810;   //两头被堵死的断五
        //public const int Fiv_113_2a = 1000;   //两头被堵死的断五
        //public const int Fiv_131_0a = 5000;   //两头被堵死的断五, 中间是3个，相当于两个冲四
        //public const int Fiv_131_2a = 5000;   //两头被堵死的断五, 中间是3个，相当于两个冲四

        public const int Five = 100000;     //=5,胜利

        //public const int One_add_1 = Tow_d11 - One_d1 - 10;                     //1+1 一头活, 下了堵死的那头
        public const int One_add_1_a = Tow_d11 - One_d1;                        //1+1 一头活, 下了活的的那头
        public const int One_add_1_b = Tow_11 - One_1;                          //1+1, 两头活,下了活的的那头
        //public const int One_add_1_c = Tow_d101 - One_1 - 20;                   //1+1, 两头活,下了堵死的那头
        //public const int One_add_1_d = Tow_d101 - One_d1;                       //1+1, 两头活,下了堵死的那头
        //public const int One_add_1_e = Tow_101 - One_1;                         //1+1, 两头活

        //public const int Tow_add_1 = Three_d111 - Tow_d11 - 10;                 //2+1, 一头活, 下了堵死的那头
        public const int Tow_add_1_a = Three_d111 - Tow_d11;                    //2+1, 一头活, 下了活的的那头, 或则中间
        public const int Two_add_1_b = Three_111 - Tow_11;                      //2+1, 两头活,下了活的那头
        //public const int One_One_add_1_a = Three_1011d - Tow_101 - 20;          //1_1+1, 一头活, 下了1堵死的那头
        //public const int One_One_add_1_b = Three_1011d - Tow_101;               //1_1+1, 一头活, 下了1活的那头,或者中间
        //public const int One_One_add_1_c = Three_d1011 - Tow_101;               //1_1+1, 一头活, 下了1活的那头
        //public const int One_One_add_1_d = Three_d1011 - Tow_101;               //1_1+1, 一头活, 下了2活的那头,或者中间
        //public const int One_One_add_1_e = Three_1011 - Tow_101;                //1_1+1, 两头活, 下了任意位置

        public const int Three_add_1 = Four_d1111 - Three_d111;                  //3+1, 一头活, 下了堵死的那头
        public const int Tree_add_1_a = Four_1111 - Three_111 + 1000;                  //3+1, 两头活

        public const int Four_skip = 20000;                                        //堵了一个四
        //public const int Tow_One_add_1_a = Four_10111d - Three_1011 - 20;       //2_1+1, 一头活, 下了堵死的3
        //public const int Tow_One_add_1_b = Four_10111d - Three_1011d;           //2_1+1, 一头活, 下了堵死的3
        //public const int Tow_One_add_1_c = Four_11011d - Three_1011 - 20;       //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_d = Four_11011d - Three_10011d;          //2_1+1, 一头活, 下了堵死的2
        //public const int Tow_One_add_1_e = Four_d10111 - Three_111;             //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_f = Four_11011d - Three_1011d;           //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_g = Four_d10111 - Three_d1011;           //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_h = Four_10111d - Three_d111;            //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_i = Four_10111 - Three_1011;             //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_j = Four_11011 - Three_1011;             //2_1+1, 一头活, 下了堵死的1
        //public const int Tow_One_add_1_k = Four_10111 - Three_111;              //2_1+1, 一头活, 下了堵死的1

        //public const int Three_One_add_1_a = Fiv_111011d - Four_111001d;          //2_1+1, 一头活, 下了堵死的1
        //public const int Three_One_add_1_b = Fiv_d111011 - Four_d111001;          //2_1+1, 一头活, 下了堵死的1
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
            //direction = Direction.NONE;
            space = 0;
            realSpace = 0;
        }

        public Position p1;

        public Position p2;

        public int length = 0;

        public LineType type = LineType.BothLive;

        //public int direction = Direction.NONE;

        public int cost = 0;

        public int color;

        public int minSpace = 0;

        public int maxSpace = 0;

        public int space = 0;

        public int realSpace = 0;
    }
}
