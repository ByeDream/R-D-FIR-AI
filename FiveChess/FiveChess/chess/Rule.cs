using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public enum WinState
    {
        //胜负未分
        GAMING = 0,
        //白胜
        WHITE_WIN = 1,
        //黑胜
        BLACK_WIN = 2,
        //和局
        DRAW = 3,
    }

    class Rule
    {
        public Rule() { }

        #region winner check

        public WinState checkWinner( ChessBoard cb)
        {
            //TODO
            return WinState.GAMING;
        }

        public WinState checkWinner(ChessBoard cb, Position p)
        {
            return checkWinner(cb, p.row, p.col);
        }

        static Line Test_Line = new Line();

        //检测胜负，知道最后一步落子的情况下,只需要横竖斜4个方向是否有连续的5个子
        public WinState checkWinner(ChessBoard cb, int row, int col)
        {
            WinState check = WinState.GAMING;
            byte chess_value = cb.Data[row][col];
            Calculator.calIncreaseValue(cb.Data, chess_value, row, col);

            //如果棋盘摆满了，先假设是和局
            if (Calculator.isFullBoard(cb))
            {
                check = WinState.DRAW;
            }

            //horizontal
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Calculator.calHorizontalCount(cb.Data, chess_value, row, col, ref Test_Line);
                //count = Calculator.hasHorizontalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //vertical
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Calculator.calVerticalCount(cb.Data, chess_value, row, col, ref Test_Line);
                //count = Calculator.hasVerticalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //inclined
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Calculator.calInclinedCount_LT(cb.Data, chess_value, row, col, ref Test_Line);
            }

            //inclined
            if (Test_Line.length < Con.WIN_COUNT)
            {
                Calculator.calInclinedCount_LB(cb.Data, chess_value, row, col, ref Test_Line);
            }

            if (Test_Line.length >= Con.WIN_COUNT)
            {
                switch(chess_value)
                {
                    case Color.BLACK:
                        Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n",Logs.level4);
                        check = WinState.BLACK_WIN;
                        break;
                    case Color.WHITE:
                        Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                        check = WinState.WHITE_WIN;
                        break;
                    default:
                        break;
                }       
            }
            Logs.writeln("Winer check = " + check);
            return check;
        }

        #endregion
    }
}
