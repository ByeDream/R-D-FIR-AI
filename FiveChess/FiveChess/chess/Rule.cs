using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class Rule
    {
        public Rule() { }

        #region winner check

        public int checkWinner( ChessBoard cb)
        {
            //TODO
            return WinState.GAMING;
        }

        public int checkWinner(int[][] data, Position p)
        {
            return checkWinner(data, p.row, p.col);
        }

        static Line Test_Line = new Line();

        //检测胜负，知道最后一步落子的情况下,只需要横竖斜4个方向是否有连续的5个子
        public int checkWinner(int [][]data, int row, int col)
        {
            int check = WinState.GAMING;

            Test_Line.reset(0, 0, 0, 0, 0);
            //如果棋盘摆满了，先假设是和局
            //if (Calculator.isFullBoard(data))
            //{
            //    check = WinState.DRAW;
            //}

            //horizontal
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Test_Line.clear();
                Calculator.calHorizontalCount(data, row, col, ref Test_Line);
                //count = Calculator.hasHorizontalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //vertical
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Test_Line.clear();
                Calculator.calVerticalCount(data, row, col, ref Test_Line);
                //count = Calculator.hasVerticalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //inclined
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Test_Line.clear();
                Calculator.calInclinedCount_LT(data, row, col, ref Test_Line);
            }

            //inclined
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Test_Line.clear();
                Calculator.calInclinedCount_LB(data, row, col, ref Test_Line);
            }

            if (Test_Line.length >= Con.WIN_COUNT)
            {
                switch(Test_Line.color)
                {
                    case Color.BLACK:
                        //Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n",Logs.level4);
                        check = WinState.BLACK_WIN;
                        break;
                    case Color.WHITE:
                        //Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                        check = WinState.WHITE_WIN;
                        break;
                    default:
                        break;
                }       
            }
            //Logs.writeln("Winer check = " + check);
            return check;
        }

        #endregion
    }
}
